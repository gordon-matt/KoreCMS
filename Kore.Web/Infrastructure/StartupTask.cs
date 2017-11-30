using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Kore.Collections;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Localization.Domain;
using Kore.Localization.Services;
using Kore.Security.Membership;
using Kore.Tenants.Domain;
using Kore.Tenants.Services;
using Kore.Threading;
using Kore.Web.Configuration;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Infrastructure
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            EnsureTenant();

            var tenantService = EngineContext.Current.Resolve<ITenantService>();
            IEnumerable<int> tenantIds = null;

            using (var connection = tenantService.OpenConnection())
            {
                tenantIds = connection.Query().Select(x => x.Id).ToList();
            }

            var membershipService = EngineContext.Current.Resolve<IMembershipService>();

            AsyncHelper.RunSync(() => EnsurePermissions(membershipService, tenantIds));
            AsyncHelper.RunSync(() => EnsureMembership(membershipService, tenantIds));

            EnsureSettings(tenantIds);
        }

        private static void EnsureTenant()
        {
            var tenantService = EngineContext.Current.Resolve<ITenantService>();
            if (tenantService.Count() == 0)
            {
                tenantService.Insert(new Tenant
                {
                    Name = "Default",
                    Url = "my-domain.com",
                    Hosts = "my-domain.com"
                });
            }
        }

        private static async Task EnsurePermissions(IMembershipService membershipService, IEnumerable<int> tenantIds)
        {
            if (membershipService.SupportsRolePermissions)
            {
                #region NULL Tenant

                var permissionProviders = EngineContext.Current.ResolveAll<IPermissionProvider>();

                var allPermissions = permissionProviders.SelectMany(x => x.GetPermissions());
                var allPermissionNames = allPermissions.Select(x => x.Name).ToHashSet();

                var installedPermissions = await membershipService.GetAllPermissions(null);
                var installedPermissionNames = installedPermissions.Select(x => x.Name).ToHashSet();

                var permissionsToAdd = allPermissions
                    .Where(x => !installedPermissionNames.Contains(x.Name))
                    .Select(x => new KorePermission
                    {
                        Name = x.Name,
                        TenantId = null,
                        Category = x.Category,
                        Description = x.Description
                    })
                    .OrderBy(x => x.Category)
                    .ThenBy(x => x.Name);

                if (!permissionsToAdd.IsNullOrEmpty())
                {
                    await membershipService.InsertPermissions(permissionsToAdd);
                }

                var permissionIdsToDelete = installedPermissions
                    .Where(x => !allPermissionNames.Contains(x.Name))
                    .Select(x => x.Id);

                if (!permissionIdsToDelete.IsNullOrEmpty())
                {
                    await membershipService.DeletePermissions(permissionIdsToDelete);
                }

                #endregion NULL Tenant

                #region Tenants

                foreach (int tenantId in tenantIds)
                {
                    installedPermissions = await membershipService.GetAllPermissions(tenantId);
                    installedPermissionNames = installedPermissions.Select(x => x.Name).ToHashSet();

                    permissionsToAdd = allPermissions
                       .Where(x => !installedPermissionNames.Contains(x.Name))
                       .Select(x => new KorePermission
                       {
                           TenantId = tenantId,
                           Name = x.Name,
                           Category = x.Category,
                           Description = x.Description
                       })
                       .OrderBy(x => x.Category)
                       .ThenBy(x => x.Name);

                    if (!permissionsToAdd.IsNullOrEmpty())
                    {
                        await membershipService.InsertPermissions(permissionsToAdd);
                    }

                    permissionIdsToDelete = installedPermissions
                       .Where(x => !allPermissionNames.Contains(x.Name))
                       .Select(x => x.Id);

                    if (!permissionIdsToDelete.IsNullOrEmpty())
                    {
                        await membershipService.DeletePermissions(permissionIdsToDelete);
                    }
                }

                #endregion Tenants
            }
        }

        private static async Task EnsureMembership(IMembershipService membershipService, IEnumerable<int> tenantIds)
        {
            // We only run this method to ensure that the admin user has been setup as part of the installation process.
            //  If there are any users already in the DB...
            if (await membershipService.GetAllUsersAsQueryable(null).AnyAsync())
            {
                // ... we assume the admin user is one of them. No need for further querying...
                return;
            }

            var dataSettings = EngineContext.Current.Resolve<DataSettings>();

            var adminUser = await membershipService.GetUserByEmail(null, dataSettings.AdminEmail);
            if (adminUser == null)
            {
                await membershipService.InsertUser(
                    new KoreUser
                    {
                        TenantId = null,
                        UserName = dataSettings.AdminEmail,
                        Email = dataSettings.AdminEmail
                    },
                    dataSettings.AdminPassword);

                adminUser = await membershipService.GetUserByEmail(null, dataSettings.AdminEmail);

                // TODO: This doesn't work. Gets error like "No owin.Environment item was found in the context."
                //// Confirm User
                //string token = await membershipService.GenerateEmailConfirmationToken(adminUser.Id);
                //await membershipService.ConfirmEmail(adminUser.Id, token);

                KoreRole administratorsRole = null;
                if (adminUser != null)
                {
                    administratorsRole = await membershipService.GetRoleByName(null, KoreWebConstants.Roles.Administrators);
                    if (administratorsRole == null)
                    {
                        await membershipService.InsertRole(new KoreRole
                        {
                            TenantId = null,
                            Name = KoreWebConstants.Roles.Administrators
                        });
                        administratorsRole = await membershipService.GetRoleByName(null, KoreWebConstants.Roles.Administrators);
                        await membershipService.AssignUserToRoles(null, adminUser.Id, new[] { administratorsRole.Id });
                    }
                }

                if (membershipService.SupportsRolePermissions && administratorsRole != null)
                {
                    var fullAccessPermission = await membershipService.GetPermissionByName(null, StandardPermissions.FullAccess.Name);
                    await membershipService.AssignPermissionsToRole(administratorsRole.Id, new[] { fullAccessPermission.Id });
                }

                dataSettings.AdminPassword = null;
                DataSettingsManager.SaveSettings(dataSettings);
            }

            if (membershipService.SupportsRolePermissions)
            {
                foreach (int tenantId in tenantIds)
                {
                    await membershipService.EnsureAdminRoleForTenant(tenantId);
                }
            }
        }

        private static void EnsureSettings(IEnumerable<int> tenantIds)
        {
            var settingsRepository = EngineContext.Current.Resolve<IRepository<Setting>>();
            var allSettings = EngineContext.Current.ResolveAll<ISettings>();
            var allSettingNames = allSettings.Select(x => x.Name).ToList();

            var settingService = EngineContext.Current.Resolve<ISettingService>();
            var dataSettings = EngineContext.Current.Resolve<DataSettings>();
            var languageService = EngineContext.Current.Resolve<ILanguageService>();

            // TODO: Probably need to remove NULL tenant in future... (from everything)
            //#region NULL Tenant (In case we want default settings)

            //var installedSettings = settingsRepository.Find(x => x.TenantId == null);
            //var installedSettingNames = installedSettings.Select(x => x.Name).ToList();

            //bool hasSettings = installedSettings.Any();

            //var settingsToAdd = allSettings.Where(x => x.IsTenantRestricted && !installedSettingNames.Contains(x.Name)).Select(x => new Setting
            //{
            //    Id = Guid.NewGuid(),
            //    TenantId = null,
            //    Name = x.Name,
            //    Type = x.GetType().FullName,
            //    Value = Activator.CreateInstance(x.GetType()).ToJson()
            //}).ToList();

            //if (!settingsToAdd.IsNullOrEmpty())
            //{
            //    settingsRepository.Insert(settingsToAdd);
            //}

            //var settingsToDelete = installedSettings.Where(x => !allSettingNames.Contains(x.Name)).ToList();

            //if (!settingsToDelete.IsNullOrEmpty())
            //{
            //    settingsRepository.Delete(settingsToDelete);
            //}

            //if (!hasSettings)
            //{
            //    EnsureDefaultSiteSettings(settingService, dataSettings, languageService, null);
            //}

            //#endregion NULL Tenant (In case we want default settings)

            #region Tenants

            foreach (var tenantId in tenantIds)
            {
                var installedSettings = settingsRepository.Find(x => x.TenantId == tenantId);
                var installedSettingNames = installedSettings.Select(x => x.Name).ToList();

                bool hasSettings = installedSettings.Any();

                var settingsToAdd = allSettings.Where(x => !x.IsTenantRestricted && !installedSettingNames.Contains(x.Name)).Select(x => new Setting
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Name = x.Name,
                    Type = x.GetType().FullName,
                    Value = Activator.CreateInstance(x.GetType()).ToJson()
                }).ToList();

                if (!settingsToAdd.IsNullOrEmpty())
                {
                    settingsRepository.Insert(settingsToAdd);
                }

                var settingsToDelete = installedSettings.Where(x => !allSettingNames.Contains(x.Name)).ToList();

                if (!settingsToDelete.IsNullOrEmpty())
                {
                    settingsRepository.Delete(settingsToDelete);
                }

                if (!hasSettings)
                {
                    EnsureDefaultSiteSettings(settingService, dataSettings, languageService, tenantId);
                }
            }

            #endregion Tenants
        }

        private static void EnsureDefaultSiteSettings(
            ISettingService settingService,
            DataSettings dataSettings,
            ILanguageService languageService,
            int? tenantId)
        {
            #region First ensure that the language exists

            string name = CultureInfo.GetCultureInfo(dataSettings.DefaultLanguage).DisplayName;

            var existing = languageService.FindOne(x =>
                x.TenantId == tenantId &&
                x.Name == name &&
                x.CultureCode == dataSettings.DefaultLanguage);

            if (existing != null && !existing.IsEnabled)
            {
                existing.IsEnabled = true;
                languageService.Update(existing);
            }
            else
            {
                languageService.Insert(new Language
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    Name = name,
                    CultureCode = dataSettings.DefaultLanguage,
                    IsEnabled = true
                });
            }

            #endregion First ensure that the language exists

            #region Then set it as the default

            var siteSettings = settingService.GetSettings<KoreSiteSettings>(tenantId);
            siteSettings.DefaultLanguage = dataSettings.DefaultLanguage;
            settingService.SaveSettings(siteSettings, tenantId);

            #endregion Then set it as the default
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IStartupTask Members
    }
}
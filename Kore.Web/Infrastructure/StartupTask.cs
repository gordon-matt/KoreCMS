using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Kore.Collections;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Infrastructure;
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

            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            AsyncHelper.RunSync(() => EnsurePermissions(membershipService, workContext));
            AsyncHelper.RunSync(() => EnsureMembership(membershipService, workContext));

            EnsureSettings();
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

        private static async Task EnsurePermissions(IMembershipService membershipService, IWorkContext workContext)
        {
            if (membershipService.SupportsRolePermissions)
            {
                var permissionProviders = EngineContext.Current.ResolveAll<IPermissionProvider>();

                var allPermissions = permissionProviders.SelectMany(x => x.GetPermissions());
                var allPermissionNames = allPermissions.Select(x => x.Name).ToHashSet();

                var installedPermissions = await membershipService.GetAllPermissions(workContext.CurrentTenant.Id);
                var installedPermissionNames = installedPermissions.Select(x => x.Name).ToHashSet();

                var permissionsToAdd = allPermissions
                    .Where(x => !installedPermissionNames.Contains(x.Name))
                    .Select(x => new KorePermission
                    {
                        Name = x.Name,
                        Category = x.Category,
                        Description = x.Description
                    })
                    .OrderBy(x => x.Category)
                    .ThenBy(x => x.Name);

                if (!permissionsToAdd.IsNullOrEmpty())
                {
                    await membershipService.InsertPermissions(workContext.CurrentTenant.Id, permissionsToAdd);
                }

                var permissionIdsToDelete = installedPermissions
                    .Where(x => !allPermissionNames.Contains(x.Name))
                    .Select(x => x.Id);

                if (!permissionIdsToDelete.IsNullOrEmpty())
                {
                    await membershipService.DeletePermissions(permissionIdsToDelete);
                }
            }
        }

        private static async Task EnsureMembership(IMembershipService membershipService, IWorkContext workContext)
        {
            // We only run this method to ensure that the admin user has been setup as part of the installation process.
            //  If there are any users already in the DB...
            if (await membershipService.GetAllUsersAsQueryable(workContext.CurrentTenant.Id).AnyAsync())
            {
                // ... we assume the admin user is one of them. No need for further querying...
                return;
            }

            var dataSettings = EngineContext.Current.Resolve<DataSettings>();

            var adminUser = await membershipService.GetUserByEmail(null, dataSettings.AdminEmail);
            if (adminUser == null)
            {
                await membershipService.InsertUser(
                    null,
                    new KoreUser
                    {
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
                        await membershipService.InsertRole(null, new KoreRole { Name = KoreWebConstants.Roles.Administrators });
                        administratorsRole = await membershipService.GetRoleByName(null, KoreWebConstants.Roles.Administrators);
                        await membershipService.AssignUserToRoles(adminUser.Id, new[] { administratorsRole.Id });
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
        }

        private static void EnsureSettings()
        {
            var settingsRepository = EngineContext.Current.Resolve<IRepository<Setting>>();
            var allSettings = EngineContext.Current.ResolveAll<ISettings>();
            var allSettingNames = allSettings.Select(x => x.Name).ToList();

            #region NULL Tenant (In case we want default settings)

            var installedSettings = settingsRepository.Find(x => x.TenantId == null);
            var installedSettingNames = installedSettings.Select(x => x.Name).ToList();

            var settingsToAdd = allSettings.Where(x => x.IsTenantRestricted && !installedSettingNames.Contains(x.Name)).Select(x => new Setting
            {
                Id = Guid.NewGuid(),
                TenantId = null,
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

            #endregion NULL Tenant (In case we want default settings)

            #region Tenants

            var tenantService = EngineContext.Current.Resolve<ITenantService>();
            IEnumerable<int> tenantIds = null;

            using (var connection = tenantService.OpenConnection())
            {
                tenantIds = connection.Query().Select(x => x.Id).ToList();
            }

            foreach (var tenantId in tenantIds)
            {
                installedSettings = settingsRepository.Find(x => x.TenantId == tenantId);
                installedSettingNames = installedSettings.Select(x => x.Name).ToList();

                settingsToAdd = allSettings.Where(x => !x.IsTenantRestricted && !installedSettingNames.Contains(x.Name)).Select(x => new Setting
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

                settingsToDelete = installedSettings.Where(x => !allSettingNames.Contains(x.Name)).ToList();

                if (!settingsToDelete.IsNullOrEmpty())
                {
                    settingsRepository.Delete(settingsToDelete);
                }
            }

            #endregion Tenants
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IStartupTask Members
    }
}
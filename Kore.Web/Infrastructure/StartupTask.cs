using System;
using System.Linq;
using Kore.Collections;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Configuration;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Infrastructure
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();

            EnsurePermissions(membershipService);
            EnsureMembership(membershipService);
            EnsureSettings();
        }

        private static void EnsurePermissions(IMembershipService membershipService)
        {
            if (membershipService.SupportsRolePermissions)
            {
                var permissionProviders = EngineContext.Current.ResolveAll<IPermissionProvider>();

                var allPermissions = permissionProviders.SelectMany(x => x.GetPermissions());
                var allPermissionNames = allPermissions.Select(x => x.Name).ToHashSet();

                var installedPermissions = membershipService.GetAllPermissions();
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
                    membershipService.InsertPermissions(permissionsToAdd);
                }

                var permissionIdsToDelete = installedPermissions
                    .Where(x => !allPermissionNames.Contains(x.Name))
                    .Select(x => x.Id);

                if (!permissionIdsToDelete.IsNullOrEmpty())
                {
                    membershipService.DeletePermissions(permissionIdsToDelete);
                }
            }
        }

        private static void EnsureMembership(IMembershipService membershipService)
        {
            // We only run this method to ensure that the admin user has been setup as part of the installation process.
            //  If there are any users already in the DB...
            if (membershipService.GetAllUsersAsQueryable().Any())
            {
                // ... we assume the admin user is one of them. No need for further querying...
                return;
            }

            var dataSettings = EngineContext.Current.Resolve<DataSettings>();

            var adminUser = membershipService.GetUserByEmail(dataSettings.AdminEmail);
            if (adminUser == null)
            {
                membershipService.InsertUser(new KoreUser { UserName = dataSettings.AdminEmail, Email = dataSettings.AdminEmail }, dataSettings.AdminPassword);
                adminUser = membershipService.GetUserByEmail(dataSettings.AdminEmail);

                KoreRole administratorsRole = null;
                if (adminUser != null)
                {
                    administratorsRole = membershipService.GetRoleByName(KoreWebConstants.Roles.Administrators);
                    if (administratorsRole == null)
                    {
                        membershipService.InsertRole(new KoreRole { Name = KoreWebConstants.Roles.Administrators });
                        administratorsRole = membershipService.GetRoleByName(KoreWebConstants.Roles.Administrators);
                        membershipService.AssignUserToRoles(adminUser.Id, new[] { administratorsRole.Id });
                    }
                }

                if (membershipService.SupportsRolePermissions && administratorsRole != null)
                {
                    var fullAccessPermission = membershipService.GetPermissionByName(StandardPermissions.FullAccess.Name);
                    membershipService.AssignPermissionsToRole(administratorsRole.Id, new[] { fullAccessPermission.Id });
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
            var installedSettings = settingsRepository.Find();
            var installedSettingNames = installedSettings.Select(x => x.Name).ToList();

            var settingsToAdd = allSettings.Where(x => !installedSettingNames.Contains(x.Name)).Select(x => new Setting
            {
                Id = Guid.NewGuid(),
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
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IStartupTask Members
    }
}
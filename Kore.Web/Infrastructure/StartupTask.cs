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
            EnsureMembership();
            EnsureSettings();
        }

        private static void EnsureMembership()
        {
            var dataSettings = EngineContext.Current.Resolve<DataSettings>();

            var membershipService = EngineContext.Current.Resolve<IMembershipService>();

            var adminUser = membershipService.GetUserByEmail(dataSettings.AdminEmail);

            if (adminUser == null)
            {
                membershipService.InsertUser(new KoreUser { UserName = dataSettings.AdminEmail, Email = dataSettings.AdminEmail }, dataSettings.AdminPassword);
                adminUser = membershipService.GetUserByEmail(dataSettings.AdminEmail);
            }

            KoreRole administratorsRole = null;
            if (adminUser != null)
            {
                administratorsRole = membershipService.GetRoleByName("Administrators");
                if (administratorsRole == null)
                {
                    membershipService.InsertRole(new KoreRole { Name = "Administrators" });
                    administratorsRole = membershipService.GetRoleByName("Administrators");
                    membershipService.AssignUserToRoles(adminUser.Id, new[] { administratorsRole.Id });
                }
            }

            //TODO: Change this to update/add/remove permissions
            //  actually, we should probably move this into Kore.Web (except last part where assiging Administrator role full permission)
            if (membershipService.SupportsRolePermissions)
            {
                var permissions = membershipService.GetAllPermissions();

                if (!permissions.Any())
                {
                    var permissionProviders = EngineContext.Current.ResolveAll<IPermissionProvider>();
                    var toInsert = permissionProviders.SelectMany(x => x.GetPermissions()).Select(x => new KorePermission
                    {
                        Name = x.Name,
                        Category = x.Category,
                        Description = x.Description
                    });
                    foreach (var permission in toInsert)
                    {
                        membershipService.InsertPermission(permission);
                    }

                    if (administratorsRole != null)
                    {
                        var fullAccessPermission = membershipService.GetPermissionByName("FullAccess");
                        membershipService.AssignPermissionsToRole(administratorsRole.Id, new[] { fullAccessPermission.Id });
                    }
                }
            }

            dataSettings.AdminPassword = null;
            DataSettingsManager.SaveSettings(dataSettings);
        }

        private static void EnsureSettings()
        {
            var settingsRepository = EngineContext.Current.Resolve<IRepository<Setting>>();
            var allSettings = EngineContext.Current.ResolveAll<ISettings>();
            var allSettingNames = allSettings.Select(x => x.Name).ToList();
            var installedSettings = settingsRepository.Table.ToList();
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
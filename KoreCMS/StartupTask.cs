using System.Linq;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Tasks.Domain;
using Kore.Web.Security.Membership.Permissions;

namespace KoreCMS
{
    public class StartupTask : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            EnsureMembership();
            EnsureTasks();
        }

        private static void EnsureMembership()
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();

            if (membershipService == null)
            {
                return;
            }

            var adminUser = membershipService.GetUserByName("admin");

            if (adminUser == null)
            {
                membershipService.InsertUser(new KoreUser { UserName = "admin" }, "admin123");
                adminUser = membershipService.GetUserByName("admin");
            }

            if (adminUser != null)
            {
                var administratorsRole = membershipService.GetRoleByName("Administrators");
                if (administratorsRole == null)
                {
                    membershipService.InsertRole(new KoreRole { Name = "Administrators" });
                    administratorsRole = membershipService.GetRoleByName("Administrators");
                    membershipService.AssignUserToRoles(adminUser.Id, new[] { administratorsRole.Id });
                }
            }

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
                }
            }
        }

        private static void EnsureTasks()
        {
            var taskRepository = EngineContext.Current.Resolve<IRepository<ScheduledTask>>();

            var clearCacheTask = taskRepository.Table.FirstOrDefault(x => x.Name == "Clear Cache");

            if (clearCacheTask == null)
            {
                clearCacheTask = new ScheduledTask
                {
                    Name = "Clear Cache",
                    Seconds = 600,
                    Type = "Kore.Caching.ClearCacheTask, Kore",
                    Enabled = false,
                    StopOnError = false,
                };
                taskRepository.Insert(clearCacheTask);
            }
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IStartupTask Members
    }
}
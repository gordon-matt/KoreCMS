using System.Linq;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Security.Membership.Permissions;

namespace KoreCMS
{
    //TODO: Add an installation page and remove this...
    public class Initialization : IStartupTask
    {
        #region IStartupTask Members

        public void Execute()
        {
            InitializeMembership();
        }

        public int Order
        {
            get { return 9999; }
        }

        #endregion IStartupTask Members

        private static void InitializeMembership()
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();

            if (membershipService == null)
            {
                return;
            }

            var adminUser = membershipService.GetUserByEmail("admin@test.com");

            if (adminUser == null)
            {
                membershipService.InsertUser(new KoreUser { UserName = "admin@test.com", Email = "admin@test.com" }, "Admin@123");
                adminUser = membershipService.GetUserByEmail("admin@test.com");
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
        }
    }
}
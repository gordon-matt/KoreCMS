using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Kore.EntityFramework;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.Models;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web
{
    public static class InstallationHelper
    {
        public static void Install<TDbContext>(HttpRequestBase httpRequest, InstallationModel model) where TDbContext : DbContext, ISupportSeed, new()
        {
            var config = WebConfigurationManager.OpenWebConfiguration(httpRequest.ApplicationPath);

            if (model.EnterConnectionString)
            {
                config.ConnectionStrings.ConnectionStrings["DefaultConnection"].ConnectionString = model.ConnectionString;
            }
            else
            {
                if (model.UseWindowsAuthentication)
                {
                    config.ConnectionStrings.ConnectionStrings["DefaultConnection"].ConnectionString = string.Format(
                        @"Server={0};Initial Catalog={1};Integrated Security=True;Persist Security Info=True;MultipleActiveResultSets=True",
                        model.DatabaseServer,
                        model.DatabaseName);
                }
                else
                {
                    config.ConnectionStrings.ConnectionStrings["DefaultConnection"].ConnectionString = string.Format(
                        @"Server={0};Initial Catalog={1};User={2};Password={3};Persist Security Info=True;MultipleActiveResultSets=True",
                        model.DatabaseServer,
                        model.DatabaseName,
                        model.DatabaseUsername,
                        model.DatabasePassword);
                }
            }

            config.Save();

            Database.SetInitializer<TDbContext>(new CreateDatabaseIfNotExists<TDbContext>());
            using (var context = new TDbContext())
            {
                // This method doesn't work and throws an exception (must be an EF bug), that's why we set Initializer above...
                //  does what we need...
                //context.Database.Create();

                context.Database.Initialize(true);
                context.Seed();
            }

            // TODO: Install localization strings

            InitializeMembership(model.AdminEmail, model.AdminPassword);

            var webHelper = EngineContext.Current.Resolve<IWebHelper>();

            webHelper.RestartAppDomain();
        }

        private static void InitializeMembership(string adminEmail, string adminPassword)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();

            var adminUser = membershipService.GetUserByEmail(adminEmail);

            if (adminUser == null)
            {
                membershipService.InsertUser(new KoreUser { UserName = adminEmail, Email = adminEmail }, adminPassword);
                adminUser = membershipService.GetUserByEmail(adminEmail);
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
        }
    }
}
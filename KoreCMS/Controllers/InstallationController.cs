using System.Data.Entity;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership.Permissions;
using KoreCMS.Data;
using KoreCMS.Models;

namespace KoreCMS.Controllers
{
    [RoutePrefix("installation")]
    public class InstallationController : Controller
    {
        private readonly IMembershipService membershipService;
        private readonly IWebHelper webHelper;

        public InstallationController(
            IMembershipService membershipService,
            IWebHelper webHelper)
        {
            this.membershipService = membershipService;
            this.webHelper = webHelper;
        }

        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            var model = new InstallationModel
            {
                AdminEmail = "admin@yourSite.com",
            };
            return View(model);
        }

        //[Compress]
        [HttpPost]
        [Route("install-post")]
        public ActionResult InstallPost(InstallationModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var config = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);

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

            Database.SetInitializer<ApplicationDbContext>(new CreateDatabaseIfNotExists<ApplicationDbContext>());
            using (var context = new ApplicationDbContext())
            {
                // This method doesn't work and throws an exception (must be an EF bug), that's why we set Initializer above...
                //  does what we need...
                //context.Database.Create();

                context.Database.Initialize(true);
                context.Seed();
            }

            // TODO: Install localization strings

            InitializeMembership(model);

            webHelper.RestartAppDomain();

            return RedirectToAction("Index", "Home");
        }

        private void InitializeMembership(InstallationModel model)
        {
            var adminUser = membershipService.GetUserByEmail(model.AdminEmail);

            if (adminUser == null)
            {
                membershipService.InsertUser(new KoreUser { UserName = model.AdminEmail, Email = model.AdminEmail }, model.AdminPassword);
                adminUser = membershipService.GetUserByEmail(model.AdminEmail);
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
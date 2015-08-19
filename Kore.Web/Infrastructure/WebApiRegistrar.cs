using System.Web.Http;
using System.Web.OData.Builder;
using Kore.Configuration.Domain;
using Kore.Logging.Domain;
using Kore.Security.Membership;
using Kore.Tasks.Domain;
using Kore.Web.Areas.Admin.Configuration.Models;
using Kore.Web.Areas.Admin.Membership.Controllers.Api;
using Kore.Web.Areas.Admin.Plugins.Models;
using System.Web.OData.Extensions;

namespace Kore.Web.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<LogEntry>("LogApi");
            builder.EntitySet<EdmPluginDescriptor>("PluginApi");
            builder.EntitySet<KorePermission>("PermissionApi");
            builder.EntitySet<KoreRole>("RoleApi");
            builder.EntitySet<KoreUser>("UserApi");
            builder.EntitySet<PublicUserInfo>("PublicUserApi");
            builder.EntitySet<ScheduledTask>("ScheduledTaskApi");
            builder.EntitySet<Setting>("SettingsApi");
            builder.EntitySet<EdmThemeConfiguration>("ThemeApi");

            RegisterLogODataActions(builder);
            RegisterMembershipODataActions(builder);
            //RegisterPluginODataActions(builder);
            RegisterScheduledTaskODataActions(builder);
            RegisterThemeODataActions(builder);

            config.MapODataServiceRoute("OData_Kore_Web", "odata/kore/web", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members

        private static void RegisterLogODataActions(ODataModelBuilder builder)
        {
            var clearAction = builder.EntityType<LogEntry>().Collection.Action("Clear");
            clearAction.Returns<IHttpActionResult>();
        }

        private static void RegisterMembershipODataActions(ODataModelBuilder builder)
        {
            var getUsersInRoleAction = builder.EntityType<KoreUser>().Collection.Action("GetUsersInRole");
            getUsersInRoleAction.Parameter<string>("roleId");
            getUsersInRoleAction.ReturnsCollectionFromEntitySet<KoreUser>("Users");

            var assignUserToRolesAction = builder.EntityType<KoreUser>().Collection.Action("AssignUserToRoles");
            assignUserToRolesAction.Parameter<string>("userId");
            assignUserToRolesAction.CollectionParameter<string>("roles");
            assignUserToRolesAction.Returns<IHttpActionResult>();

            var changePasswordAction = builder.EntityType<KoreUser>().Collection.Action("ChangePassword");
            changePasswordAction.Parameter<string>("userId");
            changePasswordAction.Parameter<string>("password");
            changePasswordAction.Returns<IHttpActionResult>();

            var getRolesForUserAction = builder.EntityType<KoreRole>().Collection.Action("GetRolesForUser");
            getRolesForUserAction.Parameter<string>("userId");
            getRolesForUserAction.ReturnsCollection<EdmKoreRole>();

            var assignPermissionsToRoleAction = builder.EntityType<KoreRole>().Collection.Action("AssignPermissionsToRole");
            assignPermissionsToRoleAction.Parameter<string>("roleId");
            assignPermissionsToRoleAction.CollectionParameter<string>("permissions");
            assignPermissionsToRoleAction.Returns<IHttpActionResult>();

            var getPermissionsForRoleAction = builder.EntityType<KorePermission>().Collection.Action("GetPermissionsForRole");
            getPermissionsForRoleAction.Parameter<string>("roleId");
            getPermissionsForRoleAction.ReturnsCollection<EdmKorePermission>();
        }

        //private static void RegisterPluginODataActions(ODataModelBuilder builder)
        //{
        //    var installAction = builder.EntityType<EdmPluginDescriptor>().Collection.Action("Install");
        //    installAction.Parameter<string>("systemName");
        //    installAction.Returns<IHttpActionResult>();

        //    var uninstallAction = builder.EntityType<EdmPluginDescriptor>().Collection.Action("Uninstall");
        //    uninstallAction.Parameter<string>("systemName");
        //    uninstallAction.Returns<IHttpActionResult>();
        //}

        private static void RegisterScheduledTaskODataActions(ODataModelBuilder builder)
        {
            var runNowAction = builder.EntityType<ScheduledTask>().Collection.Action("RunNow");
            runNowAction.Parameter<int>("taskId");
            runNowAction.Returns<IHttpActionResult>();
        }

        private static void RegisterThemeODataActions(ODataModelBuilder builder)
        {
            var setDesktopThemeAction = builder.EntityType<EdmThemeConfiguration>().Collection.Action("SetDesktopTheme");
            setDesktopThemeAction.Parameter<string>("themeName");
            setDesktopThemeAction.Returns<IHttpActionResult>();

            var setMobileThemeAction = builder.EntityType<EdmThemeConfiguration>().Collection.Action("SetMobileTheme");
            setMobileThemeAction.Parameter<string>("themeName");
            setMobileThemeAction.Returns<IHttpActionResult>();
        }
    }
}
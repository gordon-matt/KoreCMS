using System;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using Kore.Configuration.Domain;
using Kore.Localization.Domain;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Localization.Models;
using Kore.Web.ContentManagement.Areas.Admin.Membership.Controllers.Api;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain;
using Kore.Web.ContentManagement.Messaging.Domain;
using Kore.Web.Infrastructure;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Language>("Languages");
            builder.EntitySet<LocalizableString>("LocalizableStrings");
            builder.EntitySet<Menu>("Menus");
            builder.EntitySet<MenuItem>("MenuItems");
            builder.EntitySet<MessageTemplate>("MessageTemplates");
            builder.EntitySet<HistoricPage>("HistoricPages");
            builder.EntitySet<Page>("Pages");
            builder.EntitySet<PageType>("PageTypes");
            builder.EntitySet<KorePermission>("Permissions");
            builder.EntitySet<KoreRole>("Roles");
            builder.EntitySet<KoreUser>("Users");
            builder.EntitySet<QueuedEmail>("QueuedEmails");
            builder.EntitySet<Widget>("Widgets");
            builder.EntitySet<Zone>("Zones");

            // Action Configurations
            RegisterHistoricPageODataActions(builder);
            RegisterLocalizableStringODataActions(builder);
            RegisterMembershipODataActions(builder);
            RegisterPageODataActions(builder);
            RegisterWidgetODataActions(builder);

            config.Routes.MapODataRoute("OData_Kore_CMS", "odata/kore/cms", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members

        private static void RegisterHistoricPageODataActions(ODataModelBuilder builder)
        {
            var restoreVersionAction = builder.Entity<HistoricPage>().Action("RestoreVersion");
            restoreVersionAction.Returns<IHttpActionResult>();
        }

        private static void RegisterLocalizableStringODataActions(ODataModelBuilder builder)
        {
            var getComparitiveTableAction = builder.Entity<LocalizableString>().Collection.Action("GetComparitiveTable");
            getComparitiveTableAction.Parameter<string>("cultureCode");
            getComparitiveTableAction.ReturnsCollection<ComparitiveLocalizableString>();

            var putComparitiveAction = builder.Entity<LocalizableString>().Collection.Action("PutComparitive");
            putComparitiveAction.Parameter<string>("cultureCode");
            putComparitiveAction.Parameter<string>("key");
            putComparitiveAction.Parameter<ComparitiveLocalizableString>("entity");
            putComparitiveAction.Returns<IHttpActionResult>();

            var deleteComparitiveAction = builder.Entity<LocalizableString>().Collection.Action("DeleteComparitive");
            deleteComparitiveAction.Parameter<string>("cultureCode");
            deleteComparitiveAction.Parameter<string>("key");
            deleteComparitiveAction.Returns<IHttpActionResult>();
        }

        private static void RegisterMembershipODataActions(ODataModelBuilder builder)
        {
            var getUsersInRoleAction = builder.Entity<KoreUser>().Collection.Action("GetUsersInRole");
            getUsersInRoleAction.Parameter<string>("roleId");
            getUsersInRoleAction.ReturnsCollectionFromEntitySet<KoreUser>("Users");

            var assignUserToRolesAction = builder.Entity<KoreUser>().Collection.Action("AssignUserToRoles");
            assignUserToRolesAction.Parameter<string>("userId");
            assignUserToRolesAction.CollectionParameter<string>("roles");
            assignUserToRolesAction.Returns<IHttpActionResult>();

            var changePasswordAction = builder.Entity<KoreUser>().Collection.Action("ChangePassword");
            changePasswordAction.Parameter<string>("userId");
            changePasswordAction.Parameter<string>("password");
            changePasswordAction.Returns<IHttpActionResult>();

            var getRolesForUserAction = builder.Entity<KoreRole>().Collection.Action("GetRolesForUser");
            getRolesForUserAction.Parameter<string>("userId");
            getRolesForUserAction.ReturnsCollection<EdmKoreRole>();

            var assignPermissionsToRoleAction = builder.Entity<KoreRole>().Collection.Action("AssignPermissionsToRole");
            assignPermissionsToRoleAction.Parameter<string>("roleId");
            assignPermissionsToRoleAction.CollectionParameter<string>("permissions");
            assignPermissionsToRoleAction.Returns<IHttpActionResult>();

            var getPermissionsForRoleAction = builder.Entity<KorePermission>().Collection.Action("GetPermissionsForRole");
            getPermissionsForRoleAction.Parameter<string>("roleId");
            getPermissionsForRoleAction.ReturnsCollection<EdmKorePermission>();
        }

        private static void RegisterPageODataActions(ODataModelBuilder builder)
        {
            var translateAction = builder.Entity<Page>().Collection.Action("Translate");
            translateAction.Parameter<Guid>("pageId");
            translateAction.Parameter<string>("cultureCode");
            translateAction.Returns<EdmPage>();
        }

        private static void RegisterWidgetODataActions(ODataModelBuilder builder)
        {
            var getByPageIdAction = builder.Entity<Widget>().Collection.Action("GetByPageId");
            getByPageIdAction.Parameter<Guid>("pageId");
            getByPageIdAction.ReturnsCollectionFromEntitySet<Widget>("Widgets");
        }
    }
}
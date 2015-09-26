using System;
using System.Web.Http;
using System.Web.OData.Builder;
using Kore.Localization.Domain;
using Kore.Localization.Models;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Newsletters.Controllers.Api;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Sitemap.Models;
using Kore.Web.Infrastructure;
using System.Web.OData.Extensions;
using System.Web.OData;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public class WebApiRegistrar : IWebApiRegistrar
    {
        #region IWebApiRegistrar Members

        public void Register(HttpConfiguration config)
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            // Blog
            builder.EntitySet<BlogCategory>("BlogCategoryApi");
            builder.EntitySet<BlogPost>("BlogPostApi");
            builder.EntitySet<BlogPostTag>("BlogPostTagApi").EntityType.HasKey(x => new { x.PostId, x.TagId });
            builder.EntitySet<BlogTag>("BlogTagApi");

            // Content Blocks
            builder.EntitySet<ContentBlock>("ContentBlockApi");
            builder.EntitySet<EntityTypeContentBlock>("EntityTypeContentBlockApi");
            builder.EntitySet<Zone>("ZoneApi");

            // Localization
            builder.EntitySet<Language>("LanguageApi");
            builder.EntitySet<LocalizableString>("LocalizableStringApi");

            // Menus
            builder.EntitySet<Menu>("MenuApi");
            builder.EntitySet<MenuItem>("MenuItemApi");

            // Messaging
            builder.EntitySet<MessageTemplate>("MessageTemplateApi");
            builder.EntitySet<QueuedEmail>("QueuedEmailApi");

            // Pages
            builder.EntitySet<Page>("PageApi");
            builder.EntitySet<PageType>("PageTypeApi");
            builder.EntitySet<PageVersion>("PageVersionApi");
            builder.EntitySet<PageTreeItem>("PageTreeApi");

            // Other
            builder.EntitySet<SitemapConfig>("XmlSitemapApi");
            builder.EntitySet<Subscriber>("SubscriberApi");

            // Action Configurations
            RegisterContentBlockODataActions(builder);
            RegisterLanguageODataActions(builder);
            RegisterLocalizableStringODataActions(builder);
            RegisterMessageTemplateODataActions(builder);
            RegisterPageVersionODataActions(builder);
            RegisterXmlSitemapODataActions(builder);

            config.MapODataServiceRoute("OData_Kore_CMS", "odata/kore/cms", builder.GetEdmModel());
        }

        #endregion IWebApiRegistrar Members

        private static void RegisterContentBlockODataActions(ODataModelBuilder builder)
        {
            var getByPageIdAction = builder.EntityType<ContentBlock>().Collection.Action("GetByPageId");
            getByPageIdAction.Parameter<Guid>("pageId");
            getByPageIdAction.ReturnsCollectionFromEntitySet<ContentBlock>("ContentBlocks");

            var getLocalizedActionFunction = builder.EntityType<ContentBlock>().Collection.Function("GetLocalized");
            getLocalizedActionFunction.Parameter<Guid>("id");
            getLocalizedActionFunction.Parameter<string>("cultureCode");
            getLocalizedActionFunction.ReturnsFromEntitySet<ContentBlock>("ContentBlockApi");

            var saveLocalizedAction = builder.EntityType<ContentBlock>().Collection.Action("SaveLocalized");
            saveLocalizedAction.Parameter<string>("cultureCode");
            saveLocalizedAction.Parameter<ContentBlock>("entity");
            saveLocalizedAction.Returns<IHttpActionResult>();
        }

        private static void RegisterLanguageODataActions(ODataModelBuilder builder)
        {
            var resetLocalizableStringsAction = builder.EntityType<Language>().Collection.Action("ResetLocalizableStrings");
            resetLocalizableStringsAction.Returns<IHttpActionResult>();
        }

        private static void RegisterLocalizableStringODataActions(ODataModelBuilder builder)
        {
            var getComparitiveTableFunction = builder.EntityType<LocalizableString>().Collection.Function("GetComparitiveTable");
            getComparitiveTableFunction.Parameter<string>("cultureCode");
            getComparitiveTableFunction.ReturnsCollection<ComparitiveLocalizableString>();

            var putComparitiveAction = builder.EntityType<LocalizableString>().Collection.Action("PutComparitive");
            putComparitiveAction.Parameter<string>("cultureCode");
            putComparitiveAction.Parameter<string>("key");
            putComparitiveAction.Parameter<ComparitiveLocalizableString>("entity");

            var deleteComparitiveAction = builder.EntityType<LocalizableString>().Collection.Action("DeleteComparitive");
            deleteComparitiveAction.Parameter<string>("cultureCode");
            deleteComparitiveAction.Parameter<string>("key");
        }

        private static void RegisterMessageTemplateODataActions(ODataModelBuilder builder)
        {
            var getTokensAction = builder.EntityType<MessageTemplate>().Collection.Action("GetTokens");
            getTokensAction.Parameter<string>("templateName");
            getTokensAction.ReturnsCollection<string>();
        }

        private static void RegisterPageVersionODataActions(ODataModelBuilder builder)
        {
            var restoreVersionAction = builder.EntityType<PageVersion>().Action("RestoreVersion");
            restoreVersionAction.Returns<IHttpActionResult>();

            var translateAction = builder.EntityType<PageVersion>().Collection.Action("GetCurrentVersion");
            translateAction.Parameter<Guid>("pageId");
            translateAction.Parameter<string>("cultureCode");
            translateAction.Returns<EdmPageVersion>();
        }

        private static void RegisterXmlSitemapODataActions(ODataModelBuilder builder)
        {
            var getConfigAction = builder.EntityType<SitemapConfig>().Collection.Action("GetConfig");
            getConfigAction.ReturnsCollection<SitemapConfigModel>();

            var setConfigAction = builder.EntityType<SitemapConfig>().Collection.Action("SetConfig");
            setConfigAction.Parameter<int>("id");
            setConfigAction.Parameter<byte>("changeFrequency");
            setConfigAction.Parameter<float>("priority");
            setConfigAction.Returns<IHttpActionResult>();

            var generateAction = builder.EntityType<SitemapConfig>().Collection.Action("Generate");
            generateAction.Returns<IHttpActionResult>();
        }
    }
}
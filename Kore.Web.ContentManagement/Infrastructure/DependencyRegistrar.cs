using Autofac;
using ElFinder;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Tasks;
using Kore.Web.Configuration;
using Kore.Web.ContentManagement.Areas.Admin.Blog;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.RuleEngine;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripting;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.ContentManagement.Areas.Admin.Localization;
using Kore.Web.ContentManagement.Areas.Admin.Media.Services;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
using Kore.Web.ContentManagement.Areas.Admin.Messaging;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Services;
using Kore.Web.ContentManagement.Areas.Admin.Newsletters;
using Kore.Web.ContentManagement.Areas.Admin.Newsletters.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.Newsletters.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.FileSystems.Media;
using Kore.Web.Indexing.Services;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            #region Services

            // Blog
            builder.RegisterType<BlogService>().As<IBlogService>().InstancePerDependency();

            // Menus
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerDependency();
            builder.RegisterType<MenuItemService>().As<IMenuItemService>().InstancePerDependency();

            // Messaging
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerDependency();
            builder.RegisterType<MessageService>().As<IQueuedMessageProvider>().InstancePerDependency();
            builder.RegisterType<MessageTemplateService>().As<IMessageTemplateService>().InstancePerDependency();
            builder.RegisterType<QueuedEmailService>().As<IQueuedEmailService>().InstancePerDependency();

            // Pages
            builder.RegisterType<PageService>().As<IPageService>().InstancePerDependency();
            builder.RegisterType<PageTypeService>().As<IPageTypeService>().InstancePerDependency();
            builder.RegisterType<PageVersionService>().As<IPageVersionService>().InstancePerDependency();

            // Content Blocks
            builder.RegisterType<EntityTypeContentBlockService>().As<IEntityTypeContentBlockService>().InstancePerDependency();
            builder.RegisterType<ContentBlockService>().As<IContentBlockService>().InstancePerDependency();
            builder.RegisterType<ZoneService>().As<IZoneService>().InstancePerDependency();

            builder.RegisterType<NewsletterService>().As<INewsletterService>().InstancePerDependency();

            #endregion Services

            #region Localization

            builder.RegisterType<LanguagePackInvariant>().As<ILanguagePack>().SingleInstance();

            #endregion Localization

            #region Navigation

            builder.RegisterType<CmsNavigationProvider>().As<INavigationProvider>().SingleInstance();

            #endregion Navigation

            #region Security

            // Permissions
            builder.RegisterType<CmsPermissions>().As<IPermissionProvider>().SingleInstance();

            // User Profile Providers
            builder.RegisterType<NewsletterUserProfileProvider>().As<IUserProfileProvider>().SingleInstance();

            #endregion Security

            #region Themes

            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();

            #endregion Themes

            #region Configuration

            builder.RegisterType<BlogSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<PageSettings>().As<ISettings>().SingleInstance();

            #endregion Configuration

            #region Content Blocks

            builder.RegisterType<FormBlock>().As<IContentBlock>().InstancePerDependency();
            builder.RegisterType<HtmlBlock>().As<IContentBlock>().InstancePerDependency();
            builder.RegisterType<LanguageSwitchBlock>().As<IContentBlock>().InstancePerDependency();
            builder.RegisterType<NewsletterSubscriptionBlock>().As<IContentBlock>().InstancePerDependency();

            #endregion Content Blocks

            #region Other: Content Blocks

            builder.RegisterType<BuiltinRuleProvider>().As<IRuleProvider>().InstancePerDependency();
            builder.RegisterType<DisabledRuleProvider>().As<IRuleProvider>().InstancePerDependency();
            builder.RegisterType<UrlRuleProvider>().As<IRuleProvider>().InstancePerDependency();

            builder.RegisterType<DefaultContentBlockProvider>().As<IContentBlockProvider>().InstancePerDependency();
            builder.RegisterType<DefaultEntityTypeContentBlockProvider>().As<IEntityTypeContentBlockProvider>().InstancePerDependency();

            builder.RegisterType<RuleManager>().As<IRuleManager>().InstancePerDependency();
            builder.RegisterType<ScriptExpressionEvaluator>().As<IScriptExpressionEvaluator>().InstancePerDependency();

            #endregion Other: Content Blocks

            #region Other: Media

            builder.RegisterType<ConfigurationMimeTypeProvider>().As<IMimeTypeProvider>().InstancePerDependency();
            builder.RegisterType<FileSystemStorageProvider>().As<IStorageProvider>().InstancePerDependency();
            builder.RegisterType<MediaService>().As<IMediaService>().InstancePerDependency();
            builder.RegisterType<MediaPathProvider>().As<IMediaPathProvider>().InstancePerDependency();
            builder.RegisterType<ImageService>().As<IImageService>().InstancePerDependency();
            //builder.RegisterType<SystemMediaDriver>().As<IDriver>().InstancePerDependency();
            builder.RegisterType<FileSystemDriver>().As<IDriver>().InstancePerDependency();

            #endregion Other: Media

            #region Other: Messaging

            //builder.RegisterType<SimpleTextParserEngine>().As<IParserEngine>().InstancePerDependency();
            //builder.RegisterType<UrlContentParserEngine>().As<IParserEngine>().InstancePerDependency();
            builder.RegisterType<Tokenizer>().As<ITokenizer>().InstancePerDependency();

            #endregion Other: Messaging

            // Other
            builder.RegisterType<ResourceBundleRegistrar>().As<IResourceBundleRegistrar>().SingleInstance();
            builder.RegisterType<WebApiRegistrar>().As<IWebApiRegistrar>().SingleInstance();

            // Scheduled Tasks
            builder.RegisterType<ProcessQueuedMailTask>().As<ITask>().SingleInstance();

            // Indexing
            builder.RegisterType<PagesIndexingContentProvider>().As<IIndexingContentProvider>().InstancePerDependency();
            builder.RegisterType<BlogIndexingContentProvider>().As<IIndexingContentProvider>().InstancePerDependency();
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IDependencyRegistrar Members
    }
}
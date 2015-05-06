using Autofac;
using ElFinder;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Tasks;
using Kore.Web.Configuration;
using Kore.Web.ContentManagement.Areas.Admin.Blog;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.RuleEngine;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Scripting;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.ContentManagement.Areas.Admin.Localization;
using Kore.Web.ContentManagement.Areas.Admin.Localization.Services;
using Kore.Web.ContentManagement.Areas.Admin.Media;
using Kore.Web.ContentManagement.Areas.Admin.Media.Services;
using Kore.Web.ContentManagement.Areas.Admin.Menus;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
using Kore.Web.ContentManagement.Areas.Admin.Newsletters;
using Kore.Web.ContentManagement.Areas.Admin.Newsletters.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.Pages;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.FileSystems.Media;
using Kore.Web.ContentManagement.Messaging;
using Kore.Web.ContentManagement.Messaging.Services;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership;
using Kore.Web.Security.Membership.Permissions;
using KoreCMS.Areas.Admin.Navigation;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            #region Services

            // Localization
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerDependency();
            builder.RegisterType<LocalizableStringService>().As<ILocalizableStringService>().InstancePerDependency();

            // Media
            builder.RegisterType<ImageService>().As<IImageService>().InstancePerDependency();
            builder.RegisterType<MediaService>().As<IImageService>().InstancePerDependency();

            // Menus
            builder.RegisterType<MenuService>().As<IMenuService>().InstancePerDependency();
            builder.RegisterType<MenuItemService>().As<IMenuItemService>().InstancePerDependency();

            // Messaging
            builder.RegisterType<MessageService>().As<IMessageService>().InstancePerDependency();
            builder.RegisterType<MessageService>().As<IQueuedMessageProvider>().InstancePerDependency();
            builder.RegisterType<MessageTemplateService>().As<IMessageTemplateService>().InstancePerDependency();

            // Pages
            builder.RegisterType<PageService>().As<IPageService>().InstancePerDependency();
            builder.RegisterType<PageTypeService>().As<IPageTypeService>().InstancePerDependency();
            builder.RegisterType<HistoricPageService>().As<IHistoricPageService>().InstancePerDependency();

            // Content Blocks
            builder.RegisterType<ContentBlockService>().As<IContentBlockService>().InstancePerDependency();
            builder.RegisterType<ZoneService>().As<IZoneService>().InstancePerDependency();

            #endregion Services

            #region Localization

            builder.RegisterType<DefaultLocalizableStringsProvider>().As<IDefaultLocalizableStringsProvider>().SingleInstance();

            #endregion Localization

            #region Navigation

            builder.RegisterType<CmsNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<BlogNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<ContentBlockNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<LocalizationNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<MediaNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<MembershipNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<MenusNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<MessagingNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<NewslettersNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<PagesNavigationProvider>().As<INavigationProvider>().SingleInstance();

            #endregion Navigation

            #region Security

            // Permissions
            builder.RegisterType<ContentBlockPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<BlogPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<MediaPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<MenusPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<PagesPermissions>().As<IPermissionProvider>().SingleInstance();

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

            // TODO: These should probably be moved to another assembly (add-ons)
            //builder.RegisterType<NivoSliderBlock>().As<IContentBlock>().InstancePerDependency();
            //builder.RegisterType<PhotoGalleryBlock>().As<IContentBlock>().InstancePerDependency();
            //builder.RegisterType<SupersizedBlock>().As<IContentBlock>().InstancePerDependency();

            #endregion Content Blocks

            #region Other: Content Blocks

            builder.RegisterType<BuiltinRuleProvider>().As<IRuleProvider>().InstancePerDependency();
            builder.RegisterType<DisabledRuleProvider>().As<IRuleProvider>().InstancePerDependency();
            builder.RegisterType<UrlRuleProvider>().As<IRuleProvider>().InstancePerDependency();

            builder.RegisterType<DefaultContentBlockProvider>().As<IContentBlockProvider>().InstancePerDependency();

            builder.RegisterType<RuleManager>().As<IRuleManager>().InstancePerDependency();
            builder.RegisterType<ScriptExpressionEvaluator>().As<IScriptExpressionEvaluator>().InstancePerDependency();

            #endregion Other: Content Blocks

            #region Other: Media

            builder.RegisterType<ConfigurationMimeTypeProvider>().As<IMimeTypeProvider>().InstancePerDependency();
            builder.RegisterType<FileSystemStorageProvider>().As<IStorageProvider>().InstancePerDependency();
            builder.RegisterType<MediaService>().As<IImageService>().InstancePerDependency();
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
        }

        public int Order
        {
            get { return 1; }
        }

        #endregion IDependencyRegistrar Members
    }
}
using Autofac;
using ElFinder;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Tasks;
using Kore.Web.Configuration;
using Kore.Web.ContentManagement.Areas.Admin.Localization;
using Kore.Web.ContentManagement.Areas.Admin.Localization.Services;
using Kore.Web.ContentManagement.Areas.Admin.Media;
using Kore.Web.ContentManagement.Areas.Admin.Media.Services;
using Kore.Web.ContentManagement.Areas.Admin.Menus;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.RuleEngine;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Scripting;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Services;
using Kore.Web.ContentManagement.Configuration;
using Kore.Web.ContentManagement.FileSystems.Media;
using Kore.Web.ContentManagement.Messaging;
using Kore.Web.ContentManagement.Messaging.Services;
using Kore.Web.Indexing.Services;
using Kore.Web.Infrastructure;
using Kore.Web.Mvc.Themes;
using Kore.Web.Navigation;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;
using KoreCMS.Areas.Admin.Navigation;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar<ContainerBuilder>
    {
        #region IDependencyRegistrar Members

        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            //if (!PluginManager.IsPluginInstalled("Kore.Web.ContentManagement"))
            //{
            //    return;
            //}

            #region Services

            // Localization
            builder.RegisterType<LanguageService>().As<ILanguageService>().InstancePerDependency();
            builder.RegisterType<LocalizableStringService>().As<ILocalizableStringService>().InstancePerDependency();

            // Media
            builder.RegisterType<MediaPartService>().As<IMediaPartService>().InstancePerDependency();
            builder.RegisterType<MediaService>().As<IMediaService>().InstancePerDependency();

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

            // Widgets
            builder.RegisterType<WidgetService>().As<IWidgetService>().InstancePerDependency();
            builder.RegisterType<ZoneService>().As<IZoneService>().InstancePerDependency();

            #endregion Services

            #region Localization

            builder.RegisterType<DefaultLocalizableStringsProvider>().As<IDefaultLocalizableStringsProvider>().SingleInstance();

            #endregion Localization

            #region Navigation

            builder.RegisterType<CmsNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<LocalizationNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<MediaNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<MembershipNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<MenusNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<MessagingNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<PagesNavigationProvider>().As<INavigationProvider>().SingleInstance();
            builder.RegisterType<WidgetNavigationProvider>().As<INavigationProvider>().SingleInstance();

            #endregion Navigation

            #region Security

            builder.RegisterType<MediaPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<MenusPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<PagesPermissions>().As<IPermissionProvider>().SingleInstance();
            builder.RegisterType<WidgetPermissions>().As<IPermissionProvider>().SingleInstance();

            #endregion Security

            #region Themes

            builder.RegisterType<LocationFormatProvider>().As<ILocationFormatProvider>().SingleInstance();

            #endregion Themes

            #region Configuration

            builder.RegisterType<ContentManagementSettings>().As<ISettings>().SingleInstance();
            builder.RegisterType<PageSettings>().As<ISettings>().SingleInstance();

            #endregion Configuration

            #region Widgets

            builder.RegisterType<FormWidget>().As<IWidget>().InstancePerDependency();
            builder.RegisterType<GoogleAdsenseWidget>().As<IWidget>().InstancePerDependency();
            builder.RegisterType<HtmlWidget>().As<IWidget>().InstancePerDependency();

            // TODO: These should probably be moved to another assembly (add-ons)
            //builder.RegisterType<NivoSliderWidget>().As<IWidget>().InstancePerDependency();
            //builder.RegisterType<PhotoGalleryWidget>().As<IWidget>().InstancePerDependency();
            //builder.RegisterType<SupersizedWidget>().As<IWidget>().InstancePerDependency();

            #endregion Widgets

            #region Other: Media

            builder.RegisterType<ConfigurationMimeTypeProvider>().As<IMimeTypeProvider>().InstancePerDependency();
            builder.RegisterType<FileSystemStorageProvider>().As<IStorageProvider>().InstancePerDependency();
            builder.RegisterType<MediaService>().As<IMediaService>().InstancePerDependency();
            builder.RegisterType<MediaPathProvider>().As<IMediaPathProvider>().InstancePerDependency();
            builder.RegisterType<MediaPartService>().As<IMediaPartService>().InstancePerDependency();
            builder.RegisterType<SystemMediaDriver>().As<IDriver>().InstancePerDependency();

            #endregion Other: Media

            #region Other: Widgets

            builder.RegisterType<BuiltinRuleProvider>().As<IRuleProvider>().InstancePerDependency();
            builder.RegisterType<DisabledRuleProvider>().As<IRuleProvider>().InstancePerDependency();
            builder.RegisterType<UrlRuleProvider>().As<IRuleProvider>().InstancePerDependency();

            builder.RegisterType<DefaultWidgetProvider>().As<IWidgetProvider>().InstancePerDependency();

            builder.RegisterType<RuleManager>().As<IRuleManager>().InstancePerDependency();
            builder.RegisterType<ScriptExpressionEvaluator>().As<IScriptExpressionEvaluator>().InstancePerDependency();

            #endregion Other: Widgets

            #region Other: Messaging

            //builder.RegisterType<SimpleTextParserEngine>().As<IParserEngine>().InstancePerDependency();
            //builder.RegisterType<UrlContentParserEngine>().As<IParserEngine>().InstancePerDependency();
            builder.RegisterType<Tokenizer>().As<ITokenizer>().InstancePerDependency();

            #endregion

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
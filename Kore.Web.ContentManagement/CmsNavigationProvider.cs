using System.Web.Mvc;
using Kore.Localization;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement
{
    public class CmsNavigationProvider : INavigationProvider
    {
        public CmsNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS), "2", BuildCmsMenu);
        }

        private void BuildCmsMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-cms");

            // Blog
            builder.Add(T(KoreCmsLocalizableStrings.Blog.Title), "5", item => item
                .Action("Index", "Blog", new { area = CmsConstants.Areas.Blog })
                .IconCssClass("kore-icon kore-icon-blog")
                .Permission(CmsPermissions.BlogRead));

            // Content Blocks
            builder.Add(T(KoreCmsLocalizableStrings.ContentBlocks.Title), "5", item => item
                .Action("Index", "ContentBlock", new { area = CmsConstants.Areas.ContentBlocks, pageId = UrlParameter.Optional })
                .IconCssClass("kore-icon kore-icon-content-blocks")
                .Permission(CmsPermissions.ContentBlocksRead));

            // Localization
            builder.Add(T(KoreCmsLocalizableStrings.Localization.Title), "5", item => item
                .Action("Index", "Language", new { area = CmsConstants.Areas.Localization })
                .IconCssClass("kore-icon kore-icon-localization")
                .Permission(StandardPermissions.FullAccess));

            // Media
            builder.Add(T(KoreCmsLocalizableStrings.Media.Title), "5", item => item
                .Action("Index", "Media", new { area = CmsConstants.Areas.Media })
                .IconCssClass("kore-icon kore-icon-media")
                .Permission(CmsPermissions.MediaRead));

            // Menus
            builder.Add(T(KoreCmsLocalizableStrings.Menus.Title), "5", item => item
                .Action("Index", "Menu", new { area = CmsConstants.Areas.Menus })
                .IconCssClass("kore-icon kore-icon-menus")
                .Permission(CmsPermissions.MenusRead));

            // Messaging
            builder.Add(T(KoreCmsLocalizableStrings.Messaging.MessageTemplates), "5", item => item
                .Action("Index", "MessageTemplate", new { area = CmsConstants.Areas.Messaging })
                .IconCssClass("kore-icon kore-icon-message-templates")
                .Permission(CmsPermissions.MessageTemplatesRead));

            // Pages
            builder.Add(T(KoreCmsLocalizableStrings.Pages.Title), "5", item => item
                .Action("Index", "Page", new { area = CmsConstants.Areas.Pages })
                .IconCssClass("kore-icon kore-icon-pages")
                .Permission(CmsPermissions.PagesRead));

            // Queued Emails
            builder.Add(T(KoreCmsLocalizableStrings.Messaging.QueuedEmails), "5", item => item
                .Action("Index", "QueuedEmail", new { area = CmsConstants.Areas.Messaging })
                .IconCssClass("kore-icon kore-icon-message-queue")
                .Permission(CmsPermissions.QueuedEmailsRead));

            // Subscribers
            builder.Add(T(KoreCmsLocalizableStrings.Newsletters.Subscribers), "5", item => item
                .Action("Index", "Subscriber", new { area = CmsConstants.Areas.Newsletters })
                .IconCssClass("kore-icon kore-icon-subscribers")
                .Permission(StandardPermissions.FullAccess));
        }
    }
}
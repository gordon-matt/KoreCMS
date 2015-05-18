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
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS), "10", b => b.IconCssClass("kore-icon kore-icon-cms"));

            // Blog
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Blog.Title), "5", item => item
                    .Action("Index", "Blog", new { area = Constants.Areas.Blog })
                    .IconCssClass("kore-icon kore-icon-blog")
                    .Permission(CmsPermissions.BlogRead)));

            // Content Blocks
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.ContentBlocks.Title), "5", item => item
                    .Action("Index", "ContentBlock", new { area = Constants.Areas.ContentBlocks, pageId = UrlParameter.Optional })
                    .IconCssClass("kore-icon kore-icon-content-blocks")
                    .Permission(CmsPermissions.ContentBlocksRead)));

            // Localization
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Localization.Title), "5", item => item
                    .Action("Index", "Language", new { area = Constants.Areas.Localization })
                    .IconCssClass("kore-icon kore-icon-localization")
                    .Permission(StandardPermissions.FullAccess)));

            // Media
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Media.Title), "5", item => item
                    .Action("Index", "Media", new { area = Constants.Areas.Media })
                    .IconCssClass("kore-icon kore-icon-media")
                    .Permission(CmsPermissions.MediaRead)));

            // Membership
            builder.Add(T(KoreCmsLocalizableStrings.Membership.Title), "5", BuildMembershipMenu);

            // Menus
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Menus.Title), "5", item => item
                    .Action("Index", "Menu", new { area = Constants.Areas.Menus })
                    .IconCssClass("kore-icon kore-icon-menus")
                    .Permission(CmsPermissions.MenusRead)));

            // Messaging
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Messaging.MessageTemplates), "5", item => item
                    .Action("Index", "MessageTemplate", new { area = Constants.Areas.Messaging })
                    .IconCssClass("kore-icon kore-icon-message-templates")
                    .Permission(CmsPermissions.MessageTemplatesRead)));

            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Messaging.QueuedEmails), "5", item => item
                    .Action("Index", "QueuedEmail", new { area = Constants.Areas.Messaging })
                    .IconCssClass("kore-icon kore-icon-message-queue")
                    .Permission(CmsPermissions.QueuedEmailsRead)));

            // Newsletters
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Newsletters.Subscribers), "5", item => item
                    .Action("Index", "Subscriber", new { area = Constants.Areas.Newsletters })
                    .IconCssClass("kore-icon kore-icon-subscribers")
                    .Permission(StandardPermissions.FullAccess)));

            // Pages
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Pages.Title), "5", item => item
                    .Action("Index", "Page", new { area = Constants.Areas.Pages })
                    .IconCssClass("kore-icon kore-icon-pages")
                    .Permission(CmsPermissions.PagesRead)));
        }

        private void BuildMembershipMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-membership");
            builder.Permission(StandardPermissions.FullAccess);

            builder.Add(T(KoreCmsLocalizableStrings.Membership.Users), "1", item => item.Action("Users", "Membership", new { area = Constants.Areas.Membership })
                .IconCssClass("kore-icon kore-icon-users")
                .Permission(StandardPermissions.FullAccess));

            builder.Add(T(KoreCmsLocalizableStrings.Membership.Roles), "2", item => item.Action("Roles", "Membership", new { area = Constants.Areas.Membership })
                .IconCssClass("kore-icon kore-icon-roles")
                .Permission(StandardPermissions.FullAccess));
        }
    }
}
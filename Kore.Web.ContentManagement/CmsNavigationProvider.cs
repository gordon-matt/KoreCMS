using Kore.Localization;
using Kore.Web.Navigation;

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
            builder.IconCssClass("fa fa-edit");

            // Blog
            builder.Add(T(KoreCmsLocalizableStrings.Blog.Title), "5", item => item
                .Url("#blog")
                .IconCssClass("fa fa-newspaper-o")
                .Permission(CmsPermissions.BlogRead));

            // Content Blocks
            builder.Add(T(KoreCmsLocalizableStrings.ContentBlocks.Title), "5", item => item
                .Url("#blocks/content-blocks")
                .IconCssClass("fa fa-cubes")
                .Permission(CmsPermissions.ContentBlocksRead));

            // Media
            builder.Add(T(KoreCmsLocalizableStrings.Media.Title), "5", item => item
                .Url("#media")
                .IconCssClass("fa fa-picture-o")
                .Permission(CmsPermissions.MediaRead));

            // Menus
            builder.Add(T(KoreCmsLocalizableStrings.Menus.Title), "5", item => item
                .Url("#menus")
                .IconCssClass("fa fa-bars")
                .Permission(CmsPermissions.MenusRead));

            // Messaging
            builder.Add(T(KoreCmsLocalizableStrings.Messaging.MessageTemplates), "5", item => item
                .Url("#messaging/templates")
                .IconCssClass("fa fa-crop")
                .Permission(CmsPermissions.MessageTemplatesRead));

            // Pages
            builder.Add(T(KoreCmsLocalizableStrings.Pages.Title), "5", item => item
                .Url("#pages")
                .IconCssClass("fa fa-file-o")
                .Permission(CmsPermissions.PagesRead));

            // Queued Emails
            builder.Add(T(KoreCmsLocalizableStrings.Messaging.QueuedEmails), "5", item => item
                .Url("#messaging/queued-email")
                .IconCssClass("fa fa-envelope-o")
                .Permission(CmsPermissions.QueuedEmailsRead));

            // Subscribers
            builder.Add(T(KoreCmsLocalizableStrings.Newsletters.Subscribers), "5", item => item
                .Url("#newsletters/subscribers")
                .IconCssClass("fa fa-users")
                .Permission(CmsPermissions.NewsletterRead));

            // XML Sitemap
            builder.Add(T(KoreCmsLocalizableStrings.Sitemap.XMLSitemap), "5", item => item
                .Url("#sitemap/xml-sitemap")
                .IconCssClass("fa fa-sitemap")
                .Permission(CmsPermissions.SitemapRead));
        }
    }
}
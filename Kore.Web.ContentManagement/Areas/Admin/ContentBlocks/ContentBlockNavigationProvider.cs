using System.Web.Mvc;
using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public class ContentBlockNavigationProvider : INavigationProvider
    {
        public ContentBlockNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.ContentBlocks.Title), "5", item => item
                    .Action("Index", "ContentBlock", new { area = Constants.Areas.ContentBlocks, pageId = UrlParameter.Optional })
                    .IconCssClass("kore-icon kore-icon-content-blocks")
                    .Permission(ContentBlockPermissions.ManageContentBlocks)));
        }
    }
}
using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class PagesNavigationProvider : INavigationProvider
    {
        public PagesNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Pages.Title), "5", item => item
                    .Action("Index", "Page", new { area = Constants.Areas.Pages })
                    .IconCssClass("kore-icon kore-icon-pages")
                    .Permission(CmsPermissions.PagesRead)));
        }
    }
}
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
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS), "10", b => b.IconCssClass("kore-icon kore-icon-cms"));
        }
    }
}
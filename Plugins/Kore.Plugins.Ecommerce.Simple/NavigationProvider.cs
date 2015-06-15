using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Plugins.Ecommerce.Simple
{
    public class NavigationProvider : INavigationProvider
    {
        public NavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(Kore.Web.KoreWebLocalizableStrings.Plugins.Title),
                menu => menu.Add(T(LocalizableStrings.Store), "5", item => item
                    .Action("Index", "AdminHome", new { area = Constants.RouteArea })
                    .IconCssClass("kore-icon kore-icon-shopping-cart")
                    .Permission(SimpleCommercePermissions.ViewMenu)));
        }
    }
}
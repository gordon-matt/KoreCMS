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
            builder.Add(T(LocalizableStrings.Store), "99", BuildGoogleMenu);
        }

        private void BuildGoogleMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-shopping-cart");

            builder.Add(T(LocalizableStrings.Categories), "5", item => item
                .Action("Index", "CategoryAdmin", new { area = Constants.RouteArea })
                .IconCssClass("kore-icon kore-icon-categories")
                .Permission(SimpleCommercePermissions.ReadCategories));

            builder.Add(T(LocalizableStrings.Orders), "5", item => item
                .Action("Index", "OrderAdmin", new { area = Constants.RouteArea })
                .IconCssClass("kore-icon kore-icon-orders")
                .Permission(SimpleCommercePermissions.ReadOrders));

            //builder.Add(T(LocalizableStrings.Products), "5", item => item
            //    .Action("Index", "Product", new { area = Constants.RouteArea })
            //    .IconCssClass("kore-icon kore-icon-products")
            //    .Permission(SimpleCommercePermissions.ReadProducts));
        }
    }
}
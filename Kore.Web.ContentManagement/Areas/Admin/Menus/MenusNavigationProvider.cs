using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus
{
    public class MenusNavigationProvider : INavigationProvider
    {
        public MenusNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Menus.Title), "5", item => item
                    .Action("Index", "Menu", new { area = Constants.Areas.Menus })
                    .IconCssClass("kore-icon kore-icon-menus")
                    .Permission(CmsPermissions.MenusRead)));
        }
    }
}
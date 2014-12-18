using Kore.Localization;
using Kore.Web.ContentManagement;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Indexing
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
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T("Search Index"), "5", item => item
                    .Action("Index", "Indexing", new { area = Constants.Area })
                    .IconCssClass("kore-icon kore-icon-search")
                    .Permission(StandardPermissions.FullAccess)));
        }
    }
}
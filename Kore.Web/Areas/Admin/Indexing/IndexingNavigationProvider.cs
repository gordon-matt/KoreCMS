using Kore.Localization;
using Kore.Web;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Indexing
{
    public class IndexingNavigationProvider : INavigationProvider
    {
        public IndexingNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreWebLocalizableStrings.General.Configuration),
                menu => menu.Add(T("Search Index"), "5", item => item
                    .Action("Index", "Indexing", new { area = KoreWebConstants.Areas.Indexing })
                    .IconCssClass("kore-icon kore-icon-search")
                    .Permission(StandardPermissions.FullAccess)));
        }
    }
}
using Kore.Localization;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization
{
    public class LocalizationNavigationProvider : INavigationProvider
    {
        public LocalizationNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Localization.Title), "5", item => item
                    .Action("Index", "Language", new { area = Constants.Areas.Localization })
                    .IconCssClass("kore-icon kore-icon-localization")
                    .Permission(StandardPermissions.FullAccess)));
        }
    }
}
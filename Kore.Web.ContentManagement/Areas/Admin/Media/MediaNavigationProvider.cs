using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.ContentManagement.Areas.Admin.Media
{
    public class MediaNavigationProvider : INavigationProvider
    {
        public MediaNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Media.Title), "5", item => item
                    .Action("Index", "Media", new { area = Constants.Areas.Media })
                    .IconCssClass("kore-icon kore-icon-media")
                    .Permission(MediaPermissions.ManageMedia)));
        }
    }
}
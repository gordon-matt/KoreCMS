using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer
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
                menu => menu.Add(T(LocalizableStrings.RoyalVideoPlayer), "5", item => item
                    .Url("#plugins/royalvideoplayer")
                    //.Action("Index", "Playlist", new { area = Constants.RouteArea })
                    .IconCssClass("kore-icon kore-icon-video")
                    .Permission(Permissions.Read)));
        }
    }
}
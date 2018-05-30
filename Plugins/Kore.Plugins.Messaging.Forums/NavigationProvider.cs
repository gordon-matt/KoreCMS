using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Plugins.Messaging.Forums
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
                menu => menu.Add(T(LocalizableStrings.Forums), "5", item => item
                    .Url("#plugins/messaging/forums")
                    .IconCssClass("fa fa-comments-o")
                    .Permission(ForumPermissions.ReadForums)));
        }
    }
}
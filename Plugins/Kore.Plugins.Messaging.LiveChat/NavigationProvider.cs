using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Plugins.Messaging.LiveChat
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
                menu => menu.Add(T(LocalizableStrings.LiveChat), "5", item => item
                    .Action("Index", "Chat", new { area = Constants.RouteArea })
                    .Url("#plugins/messaging/livechat")
                    .Permission(LiveChatPermissions.Manage)));
        }
    }
}
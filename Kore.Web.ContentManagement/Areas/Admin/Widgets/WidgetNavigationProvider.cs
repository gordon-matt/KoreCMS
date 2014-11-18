using System.Web.Mvc;
using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets
{
    public class WidgetNavigationProvider : INavigationProvider
    {
        public WidgetNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Widgets.Title), "5", item => item
                    .Action("Index", "Widget", new { area = Constants.Areas.Widgets, pageId = UrlParameter.Optional })
                    .IconCssClass("kore-icon kore-icon-widgets")
                    .Permission(WidgetPermissions.ManageWidgets)));
        }
    }
}
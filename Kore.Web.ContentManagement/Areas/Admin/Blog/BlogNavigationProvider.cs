using Kore.Localization;
using Kore.Web.Navigation;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog
{
    public class BlogNavigationProvider : INavigationProvider
    {
        public BlogNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Navigation.CMS),
                menu => menu.Add(T(KoreCmsLocalizableStrings.Blog.Title), "5", item => item
                    .Action("Index", "Blog", new { area = Constants.Areas.Blog })
                    .IconCssClass("kore-icon kore-icon-blog")
                    .Permission(BlogPermissions.ManageBlog)));
        }
    }
}
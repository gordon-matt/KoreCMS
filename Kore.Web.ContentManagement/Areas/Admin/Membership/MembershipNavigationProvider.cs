using Kore.Localization;
using Kore.Web.ContentManagement;
using Kore.Web.Navigation;
using Kore.Web.Security.Membership.Permissions;

namespace KoreCMS.Areas.Admin.Navigation
{
    public class MembershipNavigationProvider : INavigationProvider
    {
        public MembershipNavigationProvider()
        {
            T = NullLocalizer.Instance;
        }

        public Localizer T { get; set; }

        public void GetNavigation(NavigationBuilder builder)
        {
            builder.Add(T(KoreCmsLocalizableStrings.Membership.Title), "5", BuildMembershipMenu);
        }

        private void BuildMembershipMenu(NavigationItemBuilder builder)
        {
            builder.IconCssClass("kore-icon kore-icon-membership");
            builder.Permission(StandardPermissions.FullAccess);

            builder.Add(T(KoreCmsLocalizableStrings.Membership.Users), "1", item => item.Action("Users", "Membership", new { area = Constants.Areas.Membership })
                .IconCssClass("kore-icon kore-icon-users")
                .Permission(StandardPermissions.FullAccess));

            builder.Add(T(KoreCmsLocalizableStrings.Membership.Roles), "2", item => item.Action("Roles", "Membership", new { area = Constants.Areas.Membership })
                .IconCssClass("kore-icon kore-icon-roles")
                .Permission(StandardPermissions.FullAccess));
        }
    }
}
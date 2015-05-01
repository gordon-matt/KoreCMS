using System.Linq;
using System.Security.Principal;
using Kore.Infrastructure;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.Blog;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public static class PageSecurityHelper
    {
        public static bool CheckUserHasAccessToBlog(IPrincipal user)
        {
            var blogSettings = EngineContext.Current.Resolve<BlogSettings>();

            if (!string.IsNullOrEmpty(blogSettings.AccessRestrictions))
            {
                dynamic accessRestrictions = JObject.Parse(blogSettings.AccessRestrictions);
                string roleIds = accessRestrictions.Roles;

                if (!string.IsNullOrEmpty(roleIds))
                {
                    var membershipService = EngineContext.Current.Resolve<IMembershipService>();

                    var roleNames = roleIds.Split(',')
                        .Select(id =>
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                var role = membershipService.GetRoleById(id);
                                return role == null ? null : role.Name;
                            }
                            return null;
                        })
                        .Where(x => x != null)
                        .ToList();

                    bool authorized = roleNames.Any(x => user.IsInRole(x));
                    if (!authorized)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool CheckUserHasAccessToPage(Page page, IPrincipal user)
        {
            if (!string.IsNullOrEmpty(page.AccessRestrictions))
            {
                dynamic accessRestrictions = JObject.Parse(page.AccessRestrictions);
                string roleIds = accessRestrictions.Roles;

                if (!string.IsNullOrEmpty(roleIds))
                {
                    var membershipService = EngineContext.Current.Resolve<IMembershipService>();

                    var roleNames = roleIds.Split(',')
                        .Select(id =>
                        {
                            if (!string.IsNullOrEmpty(id))
                            {
                                var role = membershipService.GetRoleById(id);
                                return role == null ? null : role.Name;
                            }
                            return null;
                        })
                        .Where(x => x != null)
                        .ToList();

                    bool authorized = roleNames.Any(x => user.IsInRole(x));
                    if (!authorized)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
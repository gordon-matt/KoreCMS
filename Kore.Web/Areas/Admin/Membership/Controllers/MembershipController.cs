using System.Web.Mvc;
using Kore.Security.Membership;
using Kore.Web.Collections;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Membership.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Membership)]
    public class MembershipController : KoreController
    {
        private readonly IMembershipService membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        protected virtual bool CheckPermissions()
        {
            if (membershipService.SupportsRolePermissions)
            {
                return CheckPermission(StandardPermissions.FullAccess);
            }
            return true;
        }

        [Compress]
        [Route("users")]
        public virtual ActionResult Users()
        {
            if (!CheckPermissions())
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Membership.Title));
            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Membership.Users));

            ViewBag.Title = T(KoreWebLocalizableStrings.Membership.Title);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.Membership.Users);

            ViewBag.SelectList = membershipService.GetAllRoles().ToSelectList(v => v.Id.ToString(), t => t.Name, T(KoreWebLocalizableStrings.Membership.AllRolesSelectListOption));

            ViewBag.InitialView = "User";
            return PartialView("Kore.Web.Areas.Admin.Membership.Views.Membership.Index");
        }

        [Compress]
        [Route("roles")]
        public virtual ActionResult Roles()
        {
            if (!CheckPermissions())
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Membership.Title));
            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Membership.Roles));

            ViewBag.Title = T(KoreWebLocalizableStrings.Membership.Title);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.Membership.Roles);

            ViewBag.InitialView = "Role";
            return PartialView("Kore.Web.Areas.Admin.Membership.Views.Membership.Index");
        }
    }
}
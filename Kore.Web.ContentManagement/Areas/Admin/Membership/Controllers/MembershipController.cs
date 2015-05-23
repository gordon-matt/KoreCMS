using System.Web.Mvc;
using Kore.Security.Membership;
using Kore.Web.Collections;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Membership.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Membership)]
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

        [CompressFilter]
        [Route("users")]
        public virtual ActionResult Users()
        {
            if (!CheckPermissions())
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Membership.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Membership.Users));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Membership.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Membership.Users);

            ViewBag.SelectList = membershipService.GetAllRoles().ToSelectList(v => v.Id.ToString(), t => t.Name, T(KoreCmsLocalizableStrings.Membership.AllRolesSelectListOption));

            ViewBag.InitialView = "User";
            return View("Kore.Web.ContentManagement.Areas.Admin.Membership.Views.Membership.Index");
        }

        [CompressFilter]
        [Route("roles")]
        public virtual ActionResult Roles()
        {
            if (!CheckPermissions())
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Membership.Title));
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Membership.Roles));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Membership.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Membership.Roles);

            ViewBag.InitialView = "Role";
            return View("Kore.Web.ContentManagement.Areas.Admin.Membership.Views.Membership.Index");
        }
    }
}
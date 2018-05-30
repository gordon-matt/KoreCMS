using System.Threading.Tasks;
using System.Web.Mvc;
using Kore.Security.Membership;
using Kore.Web.Collections;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

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
            return CheckPermission(KoreWebPermissions.MembershipManage);
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public virtual async Task<ActionResult> Index()
        {
            if (!CheckPermissions())
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Membership.Title));

            ViewBag.Title = T(KoreWebLocalizableStrings.Membership.Title);

            ViewBag.SelectList = (await membershipService.GetAllRoles(WorkContext.CurrentTenant.Id))
                .ToSelectList(v => v.Id.ToString(), t => t.Name, T(KoreWebLocalizableStrings.Membership.AllRolesSelectListOption));

            return PartialView("Kore.Web.Areas.Admin.Membership.Views.Membership.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                ChangePasswordError = T(KoreWebLocalizableStrings.Membership.ChangePasswordError).Text,
                ChangePasswordSuccess = T(KoreWebLocalizableStrings.Membership.ChangePasswordSuccess).Text,
                Create = T(KoreWebLocalizableStrings.General.Create).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                Password = T(KoreWebLocalizableStrings.Membership.Password).Text,
                Permissions = T(KoreWebLocalizableStrings.Membership.Permissions).Text,
                Roles = T(KoreWebLocalizableStrings.Membership.Roles).Text,
                SavePermissionsError = T(KoreWebLocalizableStrings.Membership.SavePermissionsError).Text,
                SavePermissionsSuccess = T(KoreWebLocalizableStrings.Membership.SavePermissionsSuccess).Text,
                SaveRolesError = T(KoreWebLocalizableStrings.Membership.SaveRolesError).Text,
                SaveRolesSuccess = T(KoreWebLocalizableStrings.Membership.SaveRolesSuccess).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    User = new
                    {
                        IsActive = T(KoreWebLocalizableStrings.Membership.UserModel.IsActive).Text,
                        Roles = T(KoreWebLocalizableStrings.Membership.UserModel.Roles).Text,
                        UserName = T(KoreWebLocalizableStrings.Membership.UserModel.UserName).Text,
                    },
                    Role = new
                    {
                        Name = T(KoreWebLocalizableStrings.Membership.RoleModel.Name).Text,
                    },
                    Permission = new
                    {
                        Category = T(KoreWebLocalizableStrings.Membership.PermissionModel.Category).Text,
                        Name = T(KoreWebLocalizableStrings.Membership.PermissionModel.Name).Text,
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System.Web.Mvc;
using Kore.Security.Membership;
using Kore.Web.Collections;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Kore.Web.Security.Membership.Permissions;
using Newtonsoft.Json.Linq;

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
        [Route("")]
        public virtual ActionResult Index()
        {
            if (!CheckPermissions())
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Membership.Title));

            ViewBag.Title = T(KoreWebLocalizableStrings.Membership.Title);

            ViewBag.SelectList = membershipService.GetAllRoles().ToSelectList(v => v.Id.ToString(), t => t.Name, T(KoreWebLocalizableStrings.Membership.AllRolesSelectListOption));

            return PartialView("Kore.Web.Areas.Admin.Membership.Views.Membership.Index");
        }

        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    ChangePasswordError: '{0}',
    ChangePasswordSuccess: '{1}',
    Create: '{2}',
    Delete: '{3}',
    DeleteRecordConfirm: '{4}',
    DeleteRecordError: '{5}',
    DeleteRecordSuccess: '{6}',
    Edit: '{7}',
    GetRecordError: '{8}',
    InsertRecordError: '{9}',
    InsertRecordSuccess: '{10}',
    Password: '{11}',
    Permissions: '{12}',
    Roles: '{13}',
    SavePermissionsError: '{14}',
    SavePermissionsSuccess: '{15}',
    SaveRolesError: '{16}',
    SaveRolesSuccess: '{17}',
    UpdateRecordError: '{18}',
    UpdateRecordSuccess: '{19}',

    Columns: {{
        User: {{
            IsActive: '{20}',
            Roles: '{21}',
            UserName: '{22}',
        }},
        Role: {{
            Name: '{23}',
        }},
        Permission: {{
            Category: '{24}',
            Name: '{25}',
        }}
    }}
}}",
   T(KoreWebLocalizableStrings.Membership.ChangePasswordError),
   T(KoreWebLocalizableStrings.Membership.ChangePasswordSuccess),
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreWebLocalizableStrings.Membership.Password),
   T(KoreWebLocalizableStrings.Membership.Permissions),
   T(KoreWebLocalizableStrings.Membership.Roles),
   T(KoreWebLocalizableStrings.Membership.SavePermissionsError),
   T(KoreWebLocalizableStrings.Membership.SavePermissionsSuccess),
   T(KoreWebLocalizableStrings.Membership.SaveRolesError),
   T(KoreWebLocalizableStrings.Membership.SaveRolesSuccess),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(KoreWebLocalizableStrings.Membership.UserModel.IsActive),
   T(KoreWebLocalizableStrings.Membership.UserModel.Roles),
   T(KoreWebLocalizableStrings.Membership.UserModel.UserName),
   T(KoreWebLocalizableStrings.Membership.RoleModel.Name),
   T(KoreWebLocalizableStrings.Membership.PermissionModel.Category),
   T(KoreWebLocalizableStrings.Membership.PermissionModel.Name));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}
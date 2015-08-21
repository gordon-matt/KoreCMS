using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Plugins.Widgets.FullCalendar.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    public class CalendarController : KoreController
    {
        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(FullCalendarPermissions.ReadCalendar))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Plugins.Title));
            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.FullCalendar));

            ViewBag.Title = T(LocalizableStrings.FullCalendar);

            return PartialView();
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    Create: '{0}',
    Delete: '{1}',
    DeleteRecordConfirm: '{2}',
    DeleteRecordError: '{3}',
    DeleteRecordSuccess: '{4}',
    Edit: '{5}',
    Events: '{6}',
    GetRecordError: '{7}',
    InsertRecordError: '{8}',
    InsertRecordSuccess: '{9}',
    UpdateRecordError: '{10}',
    UpdateRecordSuccess: '{11}',
    Columns: {{
        Calendar: {{
            Name: '{12}'
        }},
        Event: {{
            Name: '{13}',
            StartDateTime: '{14}',
            EndDateTime: '{15}'
        }}
    }}
}}",
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(LocalizableStrings.Events),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(LocalizableStrings.CalendarModel.Name),
   T(LocalizableStrings.CalendarEventModel.Name),
   T(LocalizableStrings.CalendarEventModel.StartDateTime),
   T(LocalizableStrings.CalendarEventModel.EndDateTime));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}
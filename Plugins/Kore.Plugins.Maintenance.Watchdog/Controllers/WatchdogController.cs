using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Plugins.Maintenance.Watchdog.Controllers
{
    [Authorize]
    [RouteArea(Constants.RouteArea)]
    [RoutePrefix("watchdog")]
    public class WatchdogController : KoreController
    {
        private readonly WatchdogSettings settings;

        public WatchdogController(WatchdogSettings settings)
        {
            this.settings = settings;
        }

        [Compress]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(WatchdogPermissions.Read))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Watchdog));
            ViewBag.Title = T(LocalizableStrings.ManageServices);

            ViewBag.AllowAddRemove = settings.AllowAddRemove;
            ViewBag.OnlyShowWatched = settings.OnlyShowWatched;

            return PartialView();
        }

        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            string json = string.Format(
@"{{
    AddServiceError: '{0}',
    AddServiceSuccess: '{1}',
    ConfirmStopService: '{2}',
    ConfirmRemoveService: '{3}',
    DeleteRecordConfirm: '{4}',
    DeleteRecordError: '{5}',
    DeleteRecordSuccess: '{6}',
    InsertRecordError: '{7}',
    InsertRecordSuccess: '{8}',
    RemoveServiceError: '{9}',
    RemoveServiceSuccess: '{10}',
    RestartServiceError: '{11}',
    RestartServiceSuccess: '{12}',
    StartServiceError: '{13}',
    StartServiceSuccess: '{14}',
    StopServiceError: '{15}',
    StopServiceSuccess: '{16}',

    Columns: {{
        DisplayName: '{17}',
        Password: '{18}',
        ServiceName: '{19}',
        Status: '{20}',
        Url: '{21}',
    }}
}}",
   T(LocalizableStrings.AddServiceError),
   T(LocalizableStrings.AddServiceSuccess),
   T(LocalizableStrings.ConfirmStopService),
   T(LocalizableStrings.ConfirmRemoveService),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(LocalizableStrings.RemoveServiceError),
   T(LocalizableStrings.RemoveServiceSuccess),
   T(LocalizableStrings.RestartServiceError),
   T(LocalizableStrings.RestartServiceSuccess),
   T(LocalizableStrings.StartServiceError),
   T(LocalizableStrings.StartServiceSuccess),
   T(LocalizableStrings.StopServiceError),
   T(LocalizableStrings.StopServiceSuccess),
   T(LocalizableStrings.Model.DisplayName),
   T(LocalizableStrings.Model.Password),
   T(LocalizableStrings.Model.ServiceName),
   T(LocalizableStrings.Model.Status),
   T(LocalizableStrings.Model.Url));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }
    }
}
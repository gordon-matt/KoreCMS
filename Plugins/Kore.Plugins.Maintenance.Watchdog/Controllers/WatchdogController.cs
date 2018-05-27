using System.Web.Mvc;
using Kore.Web;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

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
        [OutputCache(Duration = 86400, VaryByParam = "none")]
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

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                AddServiceError = T(LocalizableStrings.AddServiceError).Text,
                AddServiceSuccess = T(LocalizableStrings.AddServiceSuccess).Text,
                ConfirmStopService = T(LocalizableStrings.ConfirmStopService).Text,
                ConfirmRemoveService = T(LocalizableStrings.ConfirmRemoveService).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                RemoveServiceError = T(LocalizableStrings.RemoveServiceError).Text,
                RemoveServiceSuccess = T(LocalizableStrings.RemoveServiceSuccess).Text,
                RestartServiceError = T(LocalizableStrings.RestartServiceError).Text,
                RestartServiceSuccess = T(LocalizableStrings.RestartServiceSuccess).Text,
                StartServiceError = T(LocalizableStrings.StartServiceError).Text,
                StartServiceSuccess = T(LocalizableStrings.StartServiceSuccess).Text,
                StopServiceError = T(LocalizableStrings.StopServiceError).Text,
                StopServiceSuccess = T(LocalizableStrings.StopServiceSuccess).Text,
                Columns = new
                {
                    DisplayName = T(LocalizableStrings.Model.DisplayName).Text,
                    Password = T(LocalizableStrings.Model.Password).Text,
                    ServiceName = T(LocalizableStrings.Model.ServiceName).Text,
                    Status = T(LocalizableStrings.Model.Status).Text,
                    Url = T(LocalizableStrings.Model.Url).Text
                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
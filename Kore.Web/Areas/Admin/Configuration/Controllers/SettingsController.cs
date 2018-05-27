using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Web.Configuration;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.Areas.Admin.Configuration.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Configuration)]
    [RoutePrefix("Settings")]
    public class SettingsController : KoreController
    {
        private readonly Lazy<IEnumerable<ISettings>> settings;

        public SettingsController(Lazy<IEnumerable<ISettings>> settings)
            : base()
        {
            this.settings = settings;
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(ConfigurationPermissions.ReadSettings))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Configuration));
            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Settings));

            ViewBag.Title = T(KoreWebLocalizableStrings.General.Configuration);
            ViewBag.SubTitle = T(KoreWebLocalizableStrings.General.Settings);

            return PartialView("Kore.Web.Areas.Admin.Configuration.Views.Settings.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Name = T(KoreWebLocalizableStrings.Settings.Model.Name).Text,
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        [Route("get-editor-ui/{type}")]
        public ActionResult GetEditorUI(string type)
        {
            var model = settings.Value.FirstOrDefault(x => x.GetType().FullName == type.Replace('-', '.'));

            if (model == null)
            {
                return HttpNotFound();
            }

            //WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Settings), Url.Action("Index", new { area = KoreWebConstants.Areas.Admin }));
            //WorkContext.Breadcrumbs.Add(model.Name);
            //WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Edit));

            string content = RenderRazorPartialViewToString(model.EditorTemplatePath, model);
            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }
    }
}
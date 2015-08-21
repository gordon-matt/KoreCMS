﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Web.Configuration;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

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
            // NOTE: It would be better if this was in the Installation Controller, however...
            //  ... doing so would not ensure we ever have the site registered.. because a dev can install
            //  locally on his machine and then just copy the site and DB to production thereby skipping the
            //  registration process... so we put it here to be called once when the Site Settings are accessed
            SiteRegistrar.Register(this.HttpContext);

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
            string json = string.Format(
@"{{
    Edit: '{0}',
    GetRecordError: '{1}',
    UpdateRecordError: '{2}',
    UpdateRecordSuccess: '{3}',
    Columns: {{
        Name: '{4}',
    }}
}}",
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(KoreWebLocalizableStrings.Settings.Model.Name));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
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
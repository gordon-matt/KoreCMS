using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Common.Areas.Admin.Regions.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Regions)]
    public class RegionController : KoreController
    {
        private readonly Lazy<IRegionService> regionService;
        private readonly Lazy<IEnumerable<IRegionSettings>> regionSettings;

        public RegionController(
            Lazy<IRegionService> regionService,
            Lazy<IEnumerable<IRegionSettings>> regionSettings)
        {
            this.regionService = regionService;
            this.regionSettings = regionSettings;
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(Permissions.RegionsRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Regions.Title));

            ViewBag.Title = T(LocalizableStrings.Regions.Title);
            ViewBag.RegionSettings = regionSettings.Value;

            //var model = regionService.Value.GetContinents(true).Select(x => (RegionModel)x);
            return PartialView("Kore.Web.Common.Areas.Admin.Regions.Views.Region.Index");
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
    GetRecordError: '{6}',
    InsertRecordError: '{7}',
    InsertRecordSuccess: '{8}',
    Localize: '{9}',
    Settings: '{10}',
    UpdateRecordError: '{11}',
    UpdateRecordSuccess: '{12}',
    Cities: '{13}',
    States: '{14}',
    Columns: {{
        Name: '{15}'
    }}
}}",
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreWebLocalizableStrings.General.Localize),
   T(KoreWebLocalizableStrings.General.Settings),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(LocalizableStrings.Regions.Cities),
   T(LocalizableStrings.Regions.States),
   T(LocalizableStrings.Regions.Model.Name));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
        }

        [Compress]
        [Route("get-editor-ui/{settingsId}")]
        public ActionResult GetEditorUI(string settingsId)
        {
            var dictionary = regionSettings.Value.ToDictionary(k => k.Name.ToSlugUrl(), v => v);

            if (!dictionary.ContainsKey(settingsId))
            {
                return Json(new { Content = string.Empty }, JsonRequestBehavior.AllowGet);
            }

            var model = dictionary[settingsId];

            if (model == null)
            {
                return HttpNotFound();
            }

            //WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Regions.Title), Url.Action("Index", new { area = Constants.Areas.Regions }));
            //WorkContext.Breadcrumbs.Add(model.Name);
            //WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Edit));

            string content = RenderRazorPartialViewToString(model.EditorTemplatePath, model);
            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("get-cities/{countryId}")]
        public JsonResult GetCities(int countryId)
        {
            var cities = regionService.Value
                .GetSubRegions(countryId, RegionType.City, WorkContext.CurrentCultureCode)
                .ToDictionary(k => k.Id, v => v);

            var data = cities
                .OrderBy(x => x.Value.Order == null)
                .ThenBy(x => x.Value.Order)
                .ThenBy(x => x.Value.Name)
                .Select(x => new
                {
                    Id = x.Key,
                    Name = x.Value.Name
                });

            return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}
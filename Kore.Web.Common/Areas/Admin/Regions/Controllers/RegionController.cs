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
            return Json(new
            {
                Create = T(KoreWebLocalizableStrings.General.Create).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                Localize = T(KoreWebLocalizableStrings.General.Localize).Text,
                Settings = T(KoreWebLocalizableStrings.General.Settings).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Cities = T(LocalizableStrings.Regions.Cities).Text,
                States = T(LocalizableStrings.Regions.States).Text,
                Columns = new
                {
                    Name = T(LocalizableStrings.Regions.Model.Name).Text
                }
            }, JsonRequestBehavior.AllowGet);
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
        [Route("get-states/{countryId}")]
        public JsonResult GetStates(int countryId)
        {
            var states = regionService.Value
                .GetStates(countryId, WorkContext.CurrentCultureCode)
                .ToDictionary(k => k.Id, v => v);

            if (!states.Any())
            {
                return Json(new { Data = new[] { new { Id = -1, Name = "N/A" } } }, JsonRequestBehavior.AllowGet);
            }

            var data = states.Values
                .OrderBy(x => x.Order == null)
                .ThenBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [Route("get-cities/{regionId}")]
        public JsonResult GetCities(int regionId)
        {
            var cities = regionService.Value
                .GetSubRegions(regionId, RegionType.City, WorkContext.CurrentCultureCode)
                .ToDictionary(k => k.Id, v => v);

            var data = cities.Values
                .OrderBy(x => x.Order == null)
                .ThenBy(x => x.Order)
                .ThenBy(x => x.Name)
                .Select(x => new
                {
                    Id = x.Id,
                    Name = x.Name
                });

            return Json(new { Data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kore.Web.Common.Areas.Admin.Regions.Models;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.Common.Areas.Admin.Regions.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Regions)]
    public class RegionController : KoreController
    {
        private readonly Lazy<IEnumerable<IRegionSettings>> regionSettings;

        public RegionController(Lazy<IEnumerable<IRegionSettings>> regionSettings)
        {
            this.regionSettings = regionSettings;
        }

        [Compress]
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
            return View("Kore.Web.Common.Areas.Admin.Regions.Views.Region.Index");
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

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Regions.Title), Url.Action("Index", new { area = Constants.Areas.Regions }));
            WorkContext.Breadcrumbs.Add(model.Name);
            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.General.Edit));

            string content = RenderRazorPartialViewToString(model.EditorTemplatePath, model);
            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }
    }
}
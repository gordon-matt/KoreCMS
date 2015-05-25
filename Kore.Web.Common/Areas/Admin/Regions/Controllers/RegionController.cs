using System;
using System.Linq;
using System.Web.Mvc;
using Kore.Web.Common.Areas.Admin.Regions.Models;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Mvc;

namespace Kore.Web.Common.Areas.Admin.Regions.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Regions)]
    public class RegionController : KoreController
    {
        private readonly Lazy<IRegionService> regionService;

        public RegionController(Lazy<IRegionService> regionService)
        {
            this.regionService = regionService;
        }

        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(Permissions.RegionsRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(LocalizableStrings.Regions.Title));

            ViewBag.Title = T(LocalizableStrings.Regions.Title);

            //var model = regionService.Value.GetContinents(true).Select(x => (RegionModel)x);
            return View("Kore.Web.Common.Areas.Admin.Regions.Views.Region.Index");
        }
    }
}
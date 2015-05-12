using System;
using System.Text;
using System.Web.Mvc;
using Kore.Web.Indexing.Services;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Indexing.Controllers
{
    [Authorize]
    [RouteArea(KoreWebConstants.Areas.Indexing)]
    public class IndexingController : KoreController
    {
        private readonly IIndexingService indexingService;

        public IndexingController(IIndexingService indexingService)
        {
            this.indexingService = indexingService;
        }

        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreWebLocalizableStrings.Indexing.Title));

            IndexEntry indexEntry;

            try
            {
                indexEntry = indexingService.GetIndexEntry(KoreWebConstants.Indexing.DefaultIndexName);

                if (indexEntry == null)
                {
                    ViewBag.Message = string.Format(
                        "<div class=\"alert alert-info\">{0}</div>", T(KoreWebLocalizableStrings.Indexing.NoSearchIndexToManage));
                }
            }
            catch (Exception e)
            {
                indexEntry = null;
                Logger.ErrorFormat(e, "Search index couldn't be read.");

                ViewBag.Message = string.Format(
                    "<div class=\"alert alert-warning\">{0}</div>", T(KoreWebLocalizableStrings.Indexing.SearchIndexMayBeCorrupted));
            }

            ViewBag.Title = T(KoreWebLocalizableStrings.Indexing.Title);
            return View("Kore.Web.Areas.Admin.Indexing.Views.Indexing.Index", indexEntry);
        }

        [HttpPost]
        [Route("rebuild")]
        public ActionResult Rebuild()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            indexingService.RebuildIndex(KoreWebConstants.Indexing.DefaultIndexName);

            return RedirectToAction("Index");
        }
    }
}
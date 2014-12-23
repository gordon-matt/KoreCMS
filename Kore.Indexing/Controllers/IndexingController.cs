using System;
using System.Text;
using System.Web.Mvc;
using Kore.Web.Indexing.Services;
using Kore.Web.Mvc;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Indexing.Controllers
{
    [Authorize]
    [RouteArea(Constants.Area)]
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

            WorkContext.Breadcrumbs.Add(T("Indexing"));

            IndexEntry indexEntry;

            try
            {
                indexEntry = indexingService.GetIndexEntry(Constants.DefaultIndexName);

                if (indexEntry == null)
                {
                    ViewBag.Message = string.Format(
                        "<div class=\"alert alert-info\">{0}</div>", T("This site does not have a search index to manage."));
                }
            }
            catch (Exception e)
            {
                indexEntry = null;
                Logger.ErrorFormat(e, "Search index couldn't be read.");

                ViewBag.Message = string.Format(
                    "<div class=\"alert alert-warning\">{0}</div>", T("The index might be corrupted. If you can't recover click on Rebuild."));
            }

            ViewBag.Title = T("Search Index");
            return View("Kore.Indexing.Views.Indexing.Index", indexEntry);
        }

        [HttpPost]
        [Route("rebuild")]
        public ActionResult Rebuild()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            indexingService.RebuildIndex(Constants.DefaultIndexName);

            return RedirectToAction("Index");
        }
    }
}
using System;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Kore.Indexing.Services;
using Kore.Web.Indexing.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.RoboUI;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Indexing.Controllers
{
    [Authorize]
    [RouteArea(Constants.Area)]
    public class IndexingController : KoreController
    {
        private const string DefaultIndexName = "Search";
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
            var sb = new StringBuilder();

            try
            {
                indexEntry = indexingService.GetIndexEntry(DefaultIndexName);

                if (indexEntry == null)
                {
                    sb.AppendFormat("<div class=\"alert alert-info\">{0}</div>", T("This site does not have a search index to manage."));
                }
            }
            catch (Exception e)
            {
                indexEntry = null;
                Logger.ErrorFormat(e, "Search index couldn't be read.");

                sb.AppendFormat("<div class=\"alert alert-info\">{0}</div>", T("The index might be corrupted. If you can't recover click on Rebuild."));
            }

            if (indexEntry != null)
            {
                if (indexEntry.LastUpdateUtc == DateTime.MinValue)
                {
                    sb.AppendFormat(
                        "<div class=\"alert alert-info\">{0}</div>",
                        T("The search index has not been built yet."));
                }
                else
                {
                    if (indexEntry.DocumentCount == 0)
                    {
                        sb.AppendFormat(
                            "<div class=\"alert alert-info\">{0}</div>",
                            T("The search index does not contain any documents."));
                    }
                    else
                    {
                        sb.AppendFormat(
                            "<div class=\"alert alert-info\">{0}</div>",
                            string.Format(T("The search index contains {0} document/s."), indexEntry.DocumentCount));
                    }

                    if (indexEntry.Fields.Any())
                    {
                        sb.AppendFormat(
                            "<div class=\"alert alert-info\">{0}</div>",
                            string.Format(T("The search index contains the following fields: {0}."), string.Join(", ", indexEntry.Fields)));
                    }
                    else
                    {
                        sb.AppendFormat(
                            "<div class=\"alert alert-info\">{0}</div>",
                            T("The search index does not contain any fields."));
                    }

                    sb.AppendFormat(
                        "<div class=\"alert alert-info\">{0}</div>",
                        string.Format(T("The search index was last updated {0}."), indexEntry.LastUpdateUtc));

                    switch (indexEntry.IndexingStatus)
                    {
                        case IndexingStatus.Rebuilding:
                            sb.AppendFormat(
                                "<div class=\"alert alert-info\">{0}</div>",
                                T("The index is currently being rebuilt."));
                            break;

                        case IndexingStatus.Updating:
                            sb.AppendFormat(
                                "<div class=\"alert alert-info\">{0}</div>",
                                T("The index is currently being updated."));
                            break;
                    }
                }

                sb.AppendFormat("<form method=\"post\" action=\"{0}\">", Url.Action("Rebuild"));

                sb.AppendFormat(
                    "<div class=\"form-group\"><label>{0}</label><br />",
                    T("Rebuild the search index for a fresh start:"));

                sb.AppendFormat(
                    "<button class=\"btn btn-primary\" type=\"submit\"><i class=\"kore-icon kore-icon-refresh\"></i>&nbsp;{0}</button></div>",
                    T("Rebuild"));
                sb.Append("</form>");
            }

            var result = new RoboUIContentResult(sb.ToString())
            {
                Title = T("Search Index")
            };

            return result;
        }

        [HttpPost]
        [Route("rebuild")]
        public ActionResult Rebuild()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return new HttpUnauthorizedResult();
            }

            indexingService.RebuildIndex(DefaultIndexName);

            return RedirectToAction("Index");
        }
    }
}
using System;
using System.Text;
using System.Web.Mvc;
using Kore.Collections.Generic;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Configuration;
using Kore.Web.Indexing.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Notify;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Indexing.Controllers
{
    [Authorize]
    [RouteArea(Constants.Area)]
    [RoutePrefix("Search")]
    public class SearchController : KoreController
    {
        private readonly INotifier notifier;
        private readonly ISearchService searchService;
        private readonly SearchSettings searchSettings;

        public SearchController(INotifier notifier, ISearchService searchService, SearchSettings searchSettings)
        {
            this.notifier = notifier;
            this.searchService = searchService;
            this.searchSettings = searchSettings;
        }

        [Route("")]
        public ActionResult Search(string q = "")
        {
            q = q.Trim();

            if (string.IsNullOrEmpty(q))
            {
                return Redirect(Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Content("~/"));
            }

            PagedList<ISearchHit> searchHits;

            try
            {
                searchHits = searchService.Query(q, 0, 20, searchSettings.SearchedFields, WorkContext.CurrentCultureCode, searchHit => searchHit);
            }
            catch (Exception exception)
            {
                string message = string.Format(
                    T("Invalid search query: {0}"),
                    exception.GetBaseException().Message);

                Logger.Error(message);
                notifier.Error(message);
                searchHits = new PagedList<ISearchHit>(new ISearchHit[] { });
            }

            var sb = new StringBuilder();
            sb.Append("<div class=\"search-results\">");

            if (searchHits.ItemCount == 0)
            {
                sb.AppendFormat(
                    "<div class=\"resultStats\">{0}</div>",
                    string.Format(T("Your search - <strong>{0}</strong> - did not match any documents."), q));
            }
            else
            {
                sb.AppendFormat(
                    "<div class=\"resultStats\">{0}</div>",
                    string.Format(T("Your search - <strong>{0}</strong> - resulted in {1} documents."), q, searchHits.ItemCount));

                sb.Append("<ul class=\"thumbnails search-results\">");
                foreach (var searchHit in searchHits)
                {
                    sb.Append("<li>");

                    sb.AppendFormat("<a href=\"{1}\">{0}</a>", searchHit.GetString("title"), searchHit.GetString("url"));
                    sb.AppendFormat("<div class=\"description\">{0}</div>", searchHit.GetString("description") ?? searchHit.GetString("body"));

                    sb.Append("</li>");
                }
                sb.Append("</ul>");
            }

            sb.Append("</div>");

            var result = new RoboUIContentResult(sb.ToString())
            {
                Title = T("Search")
            };

            return result;
        }
    }
}
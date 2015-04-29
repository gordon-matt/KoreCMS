using System;
using System.Web.Mvc;
using Kore.Collections.Generic;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Configuration;
using Kore.Web.Indexing.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Notify;

namespace Kore.Web.Areas.Admin.Indexing.Controllers
{
    [RouteArea("")]
    [RoutePrefix("Search")]
    public class SearchController : KoreController
    {
        private readonly INotifier notifier;
        private readonly ISearchService searchService;

        public SearchController(INotifier notifier, ISearchService searchService)
        {
            this.notifier = notifier;
            this.searchService = searchService;
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
                searchHits = searchService.Query(q, WorkContext.CurrentCultureCode, 0, 20);
            }
            catch (Exception x)
            {
                string message = string.Format(
                    T("Invalid search query: {0}"),
                    x.GetBaseException().Message);

                Logger.Error(message);
                notifier.Error(message);
                searchHits = new PagedList<ISearchHit>(new ISearchHit[] { });
            }

            ViewBag.Title = T("Search");
            ViewBag.Query = q;
            return View("Kore.Web.Areas.Admin.Indexing.Views.Search.Results", searchHits);
        }
    }
}
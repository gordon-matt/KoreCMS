﻿using System;
using System.Web.Mvc;
using Kore.Collections.Generic;
using Kore.Web.Indexing;
using Kore.Web.Indexing.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Notify;
using Kore.Web.Mvc.Optimization;

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

        [Compress]
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
                    T(KoreWebLocalizableStrings.Indexing.InvalidSearchQuery),
                    x.GetBaseException().Message);

                Logger.Error(message);
                notifier.Error(message);
                searchHits = new PagedList<ISearchHit>(new ISearchHit[] { });
            }

            ViewBag.Title = T(KoreWebLocalizableStrings.General.Search);
            ViewBag.Query = q;
            return View("Kore.Web.Areas.Admin.Indexing.Views.Search.Results", searchHits);
        }
    }
}
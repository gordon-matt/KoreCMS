using System;
using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Pages)]
    public class PageController : KoreController
    {
        private readonly Lazy<IPageService> pageService;

        public PageController(Lazy<IPageService> pageService)
            : base()
        {
            this.pageService = pageService;
        }

        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(PagesPermissions.ManagePages))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Pages.ManagePages);

            return View("Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Page.Index");
        }

        [Route("{pageId}/history")]
        public ActionResult History(Guid pageId)
        {
            if (!CheckPermission(PagesPermissions.ManagePages))
            {
                return new HttpUnauthorizedResult();
            }

            var page = pageService.Value.Find(pageId);

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.Title), Url.Action("Index"));
            WorkContext.Breadcrumbs.Add(page.Title);
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.History));

            ViewBag.PageId = pageId;

            ViewBag.Title = T(KoreCmsLocalizableStrings.Pages.PageHistory);

            return View("Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Page.History");
        }
    }
}
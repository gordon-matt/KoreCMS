using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
{
    [RouteArea(Constants.Areas.Pages)]
    public class PageContentController : KoreController
    {
        private readonly IPageService pageService;

        public PageContentController(
            IPageService pageService)
            : base()
        {
            this.pageService = pageService;
        }

        public ActionResult Index(string slug)
        {
            // Hack to make it search the correct path for the view
            if (!this.ControllerContext.RouteData.DataTokens.ContainsKey("area"))
            {
                this.ControllerContext.RouteData.DataTokens.Add("area", Constants.Areas.Pages);
            }

            var currentCulture = WorkContext.CurrentCultureCode;

            var page = pageService.GetPageBySlug(slug, currentCulture);

            if (page == null)
            {
                page = pageService.GetPageBySlug(slug, null);
            }

            if (page != null && page.IsEnabled)
            {
                WorkContext.SetState("CurrentPage", page);
                WorkContext.Breadcrumbs.Add(page.Title);

                return View("Kore.Web.ContentManagement.Areas.Admin.Pages.Views.PageContent.PageContent", page);
            }

            return HttpNotFound();
        }
    }
}
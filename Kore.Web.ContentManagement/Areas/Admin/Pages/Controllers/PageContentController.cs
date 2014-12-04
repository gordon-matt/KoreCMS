using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
{
    [RouteArea(Constants.Areas.Pages)]
    public class PageContentController : KoreController
    {
        private readonly IPageService pageService;
        private readonly IPageTypeService pageTypeService;

        public PageContentController(
            IPageService pageService,
            IPageTypeService pageTypeService)
            : base()
        {
            this.pageService = pageService;
            this.pageTypeService = pageTypeService;
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
                WorkContext.Breadcrumbs.Add(page.Name);

                var pageType = pageTypeService.Find(page.PageTypeId);
                var korePageType = pageTypeService.GetKorePageType(pageType.Name);
                korePageType.InstanceName = page.Name;
                korePageType.LayoutPath = pageType.LayoutPath;
                korePageType.InitializeInstance(page);
                return View(pageType.DisplayTemplatePath, korePageType);
            }

            return HttpNotFound();
        }
    }
}
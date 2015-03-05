using System;
using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Mvc;
using System.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Pages)]
    public class PageController : KoreController
    {
        private readonly Lazy<IPageService> pageService;
        private readonly Lazy<IPageTypeService> pageTypeService;

        public PageController(Lazy<IPageService> pageService, Lazy<IPageTypeService> pageTypeService)
            : base()
        {
            this.pageService = pageService;
            this.pageTypeService = pageTypeService;
        }

        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(PagesPermissions.ManagePages))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Pages.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Pages.ManagePages);

            return View("Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Page.Index");
        }

        [Route("get-editor-ui/{pageId}")]
        public ActionResult GetEditorUI(Guid pageId)
        {
            var page = pageService.Value.Find(pageId);
            var pageType = pageTypeService.Value.Find(page.PageTypeId);
            var korePageTypes = pageTypeService.Value.GetKorePageTypes();

            var korePageType = pageTypeService.Value.GetKorePageType(pageType.Name);
            korePageType.InitializeInstance(page);

            string content = RenderRazorPartialViewToString(korePageType.EditorTemplatePath, korePageType);
            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
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
            WorkContext.Breadcrumbs.Add(page.Name);
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.History));

            ViewBag.PageId = pageId;

            ViewBag.Title = T(KoreCmsLocalizableStrings.Pages.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Pages.PageHistory);

            return View("Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Page.History");
        }
    }
}
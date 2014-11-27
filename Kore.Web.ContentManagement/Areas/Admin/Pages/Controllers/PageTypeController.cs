//using System;
//using System.Linq;
//using System.Web.Mvc;
//using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
//using Kore.Web.Mvc;

//namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
//{
//    [Authorize]
//    [RouteArea(Constants.Areas.Pages)]
//    [RoutePrefix("page-types")]
//    public class PageTypeController : KoreController
//    {
//        private readonly IPageTypeService service;

//        public PageTypeController(IPageTypeService service)
//        {
//            this.service = service;
//        }

//        [Route("get-editor-ui/{pageTypeId}")]
//        public ActionResult GetEditorUI(Guid pageTypeId)
//        {
//            var pageType = service.Find(pageTypeId);
//            var korePageTypes = service.GetKorePageTypes();
//            var korePageType = korePageTypes.First(x => x.Name == pageType.Name);

//            string content = RenderRazorPartialViewToString(korePageType.EditorTemplatePath, korePageType);
//            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
//        }
//    }
//}
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.ContentBlocks)]
    public class ContentBlockController : KoreController
    {
        private readonly Lazy<IContentBlockService> contentBlockService;

        public ContentBlockController(Lazy<IContentBlockService> contentBlockService)
        {
            this.contentBlockService = contentBlockService;
        }

        [Route("{pageId?}")]
        public ActionResult Index(Guid? pageId)
        {
            if (!CheckPermission(CmsPermissions.ContentBlocksRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.ContentBlocks.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.ContentBlocks.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.ContentBlocks.ManageContentBlocks);

            if (pageId.HasValue)
            {
                WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.ManagePages), Url.Action("Index", "Page", new { area = Constants.Areas.Pages }));
            }

            ViewBag.PageId = pageId;

            return View("Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.ContentBlock.Index");
        }

        [Route("get-editor-ui/{contentBlockId}")]
        public ActionResult GetEditorUI(Guid contentBlockId)
        {
            var contentBlock = contentBlockService.Value.FindOne(contentBlockId);
            var blockType = Type.GetType(contentBlock.BlockType);

            var contentBlocks = EngineContext.Current.ResolveAll<IContentBlock>();
            var iContentBlock = contentBlocks.First(x => x.GetType() == blockType);

            string content;

            try
            {
                // TODO: See if we can make EditorTemplatePath not so specific a path (just the name), so we can override it in themes, etc
                content = RenderRazorPartialViewToString(iContentBlock.EditorTemplatePath, iContentBlock);
            }
            catch (NotSupportedException)
            {
                content = string.Empty;
            }

            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }
    }
}
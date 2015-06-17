using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.ContentBlocks)]
    public class ContentBlockController : KoreController
    {
        private readonly Lazy<IContentBlockService> contentBlockService;
        private readonly Lazy<IPluginSupportedBlockService> pluginSupportedBlockService;

        public ContentBlockController(
            Lazy<IContentBlockService> contentBlockService,
            Lazy<IPluginSupportedBlockService> pluginSupportedBlockService)
        {
            this.contentBlockService = contentBlockService;
            this.pluginSupportedBlockService = pluginSupportedBlockService;
        }

        [Compress]
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
                WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.ManagePages), Url.Action("Index", "Page", new { area = CmsConstants.Areas.Pages }));
            }

            ViewBag.PageId = pageId;

            return View("Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.ContentBlock.Index");
        }

        [Compress]
        [Route("get-editor-ui/{contentBlockId}")]
        public ActionResult GetEditorUI(Guid contentBlockId)
        {
            var blockEntity = contentBlockService.Value.FindOne(contentBlockId);
            var blockType = Type.GetType(blockEntity.BlockType);

            var blocks = EngineContext.Current.ResolveAll<IContentBlock>();
            var block = blocks.First(x => x.GetType() == blockType);

            string content;

            try
            {
                // TODO: See if we can make EditorTemplatePath not so specific a path (just the name), so we can override it in themes, etc
                content = RenderRazorPartialViewToString(block.EditorTemplatePath, block);
            }
            catch (NotSupportedException)
            {
                content = string.Empty;
            }

            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }

        //[Compress]
        //[Route("get-plugin-editor-ui/{contentBlockId}")]
        //public ActionResult GetPluginEditorUI(Guid contentBlockId, string entityType, string entityId)
        //{
        //    var blockEntity = contentBlockService.Value.FindOne(contentBlockId);

        //    var pluginSupportedBlockEntity = pluginSupportedBlockService.Value.FindOne(x =>
        //        x.ContentBlockId == contentBlockId &&
        //        x.EntityType == entityType &&
        //        x.EntityId == entityId);

        //    var blockType = Type.GetType(blockEntity.BlockType);

        //    var blocks = EngineContext.Current.ResolveAll<IPluginSupportedBlock>();
        //    var block = blocks.First(x => x.GetType() == blockType);

        //    string content;

        //    try
        //    {
        //        // TODO: See if we can make EditorTemplatePath not so specific a path (just the name), so we can override it in themes, etc
        //        content = RenderRazorPartialViewToString(block.PluginEditorTemplatePath, block);
        //    }
        //    catch (NotSupportedException)
        //    {
        //        content = string.Empty;
        //    }

        //    return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        //}
    }
}
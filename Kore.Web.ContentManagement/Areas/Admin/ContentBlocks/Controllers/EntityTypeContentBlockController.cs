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
    [RouteArea(CmsConstants.Areas.Blocks)]
    [RoutePrefix("entity-type-content-blocks")]
    public class EntityTypeContentBlockController : KoreController
    {
        private readonly Lazy<IEntityTypeContentBlockService> entityTypeContentBlockService;

        public EntityTypeContentBlockController(Lazy<IEntityTypeContentBlockService> entityTypeContentBlockService)
        {
            this.entityTypeContentBlockService = entityTypeContentBlockService;
        }

        [Compress]
        [Route("{entityType}/{entityId}")]
        public ActionResult Index(string entityType, string entityId)
        {
            if (!CheckPermission(CmsPermissions.ContentBlocksRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.ContentBlocks.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.ContentBlocks.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.ContentBlocks.ManageContentBlocks);
            ViewBag.EntityType = entityType;
            ViewBag.EntityId = entityId;

            return View("Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.EntityTypeContentBlock.Index");
        }

        [Compress]
        [Route("get-editor-ui/{contentBlockId}")]
        public ActionResult GetEditorUI(Guid contentBlockId)
        {
            var blockEntity = entityTypeContentBlockService.Value.FindOne(contentBlockId);
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
    }
}
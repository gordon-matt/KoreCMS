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
    [RoutePrefix("content-blocks")]
    public class ContentBlockController : KoreController
    {
        private readonly Lazy<IContentBlockService> contentBlockService;

        public ContentBlockController(
            Lazy<IContentBlockService> contentBlockService)
        {
            this.contentBlockService = contentBlockService;
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("{pageId?}")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.ContentBlocksRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.ContentBlocks.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.ContentBlocks.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.ContentBlocks.ManageContentBlocks);

            //if (pageId.HasValue)
            //{
            //    WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.ManagePages), Url.Action("Index", "Page", new { area = CmsConstants.Areas.Pages }));
            //}

            //ViewBag.PageId = pageId;

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.ContentBlock.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                Create = T(KoreWebLocalizableStrings.General.Create).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                Localize = T(KoreWebLocalizableStrings.General.Localize).Text,
                Toggle = T(KoreWebLocalizableStrings.General.Toggle).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                Columns = new
                {
                    Title = T(KoreCmsLocalizableStrings.ContentBlocks.Model.Title).Text,
                    BlockType = T(KoreCmsLocalizableStrings.ContentBlocks.Model.BlockType).Text,
                    Order = T(KoreCmsLocalizableStrings.ContentBlocks.Model.Order).Text,
                    IsEnabled = T(KoreCmsLocalizableStrings.ContentBlocks.Model.IsEnabled).Text,
                    Name = T(KoreCmsLocalizableStrings.ContentBlocks.ZoneModel.Name).Text,
                }
            }, JsonRequestBehavior.AllowGet);
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
    }
}
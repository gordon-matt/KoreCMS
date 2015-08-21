using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

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
            string json = string.Format(
@"{{
    Create: '{0}',
    Delete: '{1}',
    DeleteRecordConfirm: '{2}',
    DeleteRecordError: '{3}',
    DeleteRecordSuccess: '{4}',
    Edit: '{5}',
    GetRecordError: '{6}',
    InsertRecordError: '{7}',
    InsertRecordSuccess: '{8}',
    Toggle: '{9}',
    UpdateRecordError: '{10}',
    UpdateRecordSuccess: '{11}',

    Columns: {{
        Title: '{12}',
        BlockType: '{13}',
        Order: '{14}',
        IsEnabled: '{15}',
        Name: '{16}',
    }}
}}",
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordError),
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreWebLocalizableStrings.General.Toggle),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(KoreCmsLocalizableStrings.ContentBlocks.Model.Title),
   T(KoreCmsLocalizableStrings.ContentBlocks.Model.BlockType),
   T(KoreCmsLocalizableStrings.ContentBlocks.Model.Order),
   T(KoreCmsLocalizableStrings.ContentBlocks.Model.IsEnabled),
   T(KoreCmsLocalizableStrings.ContentBlocks.ZoneModel.Name));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
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
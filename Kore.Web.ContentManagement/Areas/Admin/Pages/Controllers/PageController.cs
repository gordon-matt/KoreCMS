using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Pages)]
    public class PageController : KoreController
    {
        protected static Regex ContentZonePattern = new Regex(@"\[\[ContentZone:(?<Zone>.*)\]\]", RegexOptions.Compiled);

        private readonly Lazy<IContentBlockService> contentBlockService;
        private readonly Lazy<IPageService> pageService;
        private readonly Lazy<IPageTypeService> pageTypeService;
        private readonly Lazy<IPageVersionService> pageVersionService;
        private readonly Lazy<IZoneService> zoneService;

        public PageController(
            Lazy<IContentBlockService> contentBlockService,
            Lazy<IPageService> pageService,
            Lazy<IPageTypeService> pageTypeService,
            Lazy<IPageVersionService> pageVersionService,
            Lazy<IZoneService> zoneService)
            : base()
        {
            this.contentBlockService = contentBlockService;
            this.pageService = pageService;
            this.pageTypeService = pageTypeService;
            this.pageVersionService = pageVersionService;
            this.zoneService = zoneService;
        }

        [Compress]
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(CmsPermissions.PagesRead))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Pages.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Pages.ManagePages);

            return PartialView("Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Page.Index");
        }

        [OutputCache(Duration = 86400, VaryByParam = "none")]
        [Route("get-translations")]
        public JsonResult GetTranslations()
        {
            return Json(new
            {
                CircularRelationshipError = T(KoreCmsLocalizableStrings.Messages.CircularRelationshipError).Text,
                Create = T(KoreWebLocalizableStrings.General.Create).Text,
                ContentBlocks = T(KoreCmsLocalizableStrings.ContentBlocks.Title).Text,
                Delete = T(KoreWebLocalizableStrings.General.Delete).Text,
                DeleteRecordConfirm = T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord).Text,
                DeleteRecordError = T(KoreWebLocalizableStrings.General.DeleteRecordError).Text,
                DeleteRecordSuccess = T(KoreWebLocalizableStrings.General.DeleteRecordSuccess).Text,
                Details = T(KoreWebLocalizableStrings.General.Details).Text,
                Edit = T(KoreWebLocalizableStrings.General.Edit).Text,
                GetRecordError = T(KoreWebLocalizableStrings.General.GetRecordError).Text,
                GetTranslationError = T(KoreCmsLocalizableStrings.Messages.GetTranslationError).Text,
                PageHistory = T(KoreCmsLocalizableStrings.Pages.PageHistory).Text,
                InsertRecordError = T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
                InsertRecordSuccess = T(KoreWebLocalizableStrings.General.InsertRecordSuccess).Text,
                Localize = T(KoreWebLocalizableStrings.General.Localize).Text,
                Move = T(KoreWebLocalizableStrings.General.Move).Text,
                Preview = T(KoreWebLocalizableStrings.General.Preview).Text,
                Restore = T(KoreCmsLocalizableStrings.Pages.Restore).Text,
                Toggle = T(KoreWebLocalizableStrings.General.Toggle).Text,
                Translations = T(KoreCmsLocalizableStrings.Pages.Translations).Text,
                UpdateRecordError = T(KoreWebLocalizableStrings.General.UpdateRecordError).Text,
                UpdateRecordSuccess = T(KoreWebLocalizableStrings.General.UpdateRecordSuccess).Text,
                UpdateTranslationError = T(KoreCmsLocalizableStrings.Messages.UpdateTranslationError).Text,
                UpdateTranslationSuccess = T(KoreCmsLocalizableStrings.Messages.UpdateTranslationSuccess).Text,
                View = T(KoreWebLocalizableStrings.General.View).Text,
                PageHistoryRestoreConfirm = T(KoreCmsLocalizableStrings.Pages.PageHistoryRestoreConfirm).Text,
                PageHistoryRestoreError = T(KoreCmsLocalizableStrings.Pages.PageHistoryRestoreError).Text,
                PageHistoryRestoreSuccess = T(KoreCmsLocalizableStrings.Pages.PageHistoryRestoreSuccess).Text,
                Columns = new
                {
                    Page = new
                    {
                        Name = T(KoreCmsLocalizableStrings.Pages.PageModel.Name).Text,
                        IsEnabled = T(KoreCmsLocalizableStrings.Pages.PageModel.IsEnabled).Text,
                    },
                    PageType = new
                    {
                        Name = T(KoreCmsLocalizableStrings.Pages.PageTypeModel.Name).Text,
                    },
                    PageVersion = new
                    {
                        Title = T(KoreCmsLocalizableStrings.Pages.PageVersionModel.Title).Text,
                        Slug = T(KoreCmsLocalizableStrings.Pages.PageVersionModel.Slug).Text,
                        DateModifiedUtc = T(KoreCmsLocalizableStrings.Pages.PageVersionModel.DateModified).Text,
                    }
                }
            }, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        [Route("get-editor-ui/{pageVersionId}")]
        public ActionResult GetEditorUI(Guid pageVersionId)
        {
            PageVersion pageVersion;
            using (var connection = pageVersionService.Value.OpenConnection())
            {
                pageVersion = connection.Query()
                    .Include(x => x.Page)
                    .FirstOrDefault(x => x.Id == pageVersionId);
            }

            var pageType = pageTypeService.Value.FindOne(pageVersion.Page.PageTypeId);
            var korePageTypes = pageTypeService.Value.GetKorePageTypes();

            var korePageType = pageTypeService.Value.GetKorePageType(pageType.Name);
            korePageType.InitializeInstance(pageVersion);

            string content = RenderRazorPartialViewToString(korePageType.EditorTemplatePath, korePageType);
            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        [Route("preview/{pageId}")]
        public async Task<ActionResult> Preview(Guid pageId)
        {
            var currentCulture = WorkContext.CurrentCultureCode;
            var tenantId = WorkContext.CurrentTenant.Id;

            var pageVersion = pageVersionService.Value.GetCurrentVersion(
                tenantId,
                pageId,
                currentCulture,
                enabledOnly: false,
                shownOnMenusOnly: false,
                allowReturnDraftVersion: true);

            return await PagePreview(pageVersion);
        }

        [Compress]
        [Route("preview-version/{pageVersionId}")]
        public async Task<ActionResult> PreviewVersion(Guid pageVersionId)
        {
            PageVersion pageVersion;
            using (var connection = pageVersionService.Value.OpenConnection())
            {
                pageVersion = await connection.Query()
                    .Include(x => x.Page)
                    .FirstOrDefaultAsync(x => x.Id == pageVersionId);
            }

            return await PagePreview(pageVersion);
        }

        private async Task<ActionResult> PagePreview(PageVersion pageVersion)
        {
            if (pageVersion != null)
            {
                pageVersion.Page.IsEnabled = true; // Override here to make sure it passes the check here: PageSecurityHelper.CheckUserHasAccessToPage

                // If there are access restrictions
                if (!await PageSecurityHelper.CheckUserHasAccessToPage(pageVersion.Page, User))
                {
                    return new HttpUnauthorizedResult();
                }

                // Else no restrictions (available for anyone to view)
                WorkContext.SetState("CurrentPageId", pageVersion.Id);
                WorkContext.Breadcrumbs.Add(pageVersion.Title);

                var pageType = await pageTypeService.Value.FindOneAsync(pageVersion.Page.PageTypeId);
                var korePageType = pageTypeService.Value.GetKorePageType(pageType.Name);
                korePageType.InstanceName = pageVersion.Title;
                korePageType.InstanceParentId = pageVersion.Page.ParentId;

                korePageType.LayoutPath = string.IsNullOrWhiteSpace(pageType.LayoutPath)
                    ? KoreWebConstants.DefaultFrontendLayoutPath
                    : pageType.LayoutPath;

                korePageType.InitializeInstance(pageVersion);

                var contentBlocks = contentBlockService.Value.GetContentBlocks(pageVersion.Id, WorkContext.CurrentCultureCode);
                korePageType.ReplaceContentTokens(x => InsertContentBlocks(x, contentBlocks));

                return View(korePageType.DisplayTemplatePath, korePageType);
            }

            return HttpNotFound();
        }

        private string InsertContentBlocks(string content, IEnumerable<IContentBlock> contentBlocks)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            foreach (Match match in ContentZonePattern.Matches(content))
            {
                string zoneName = match.Groups["Zone"].Value;

                var zone = zoneService.Value.FindOne(x => x.Name == zoneName);
                var contentBlocksByZone = contentBlocks.Where(x => x.ZoneId == zone.Id);

                string html = RenderRazorPartialViewToString("Kore.Web.ContentManagement.Views.Frontend.ContentBlocksByZone", contentBlocksByZone);

                content = content.Replace(match.Value, html);
            }
            return content;
        }
    }
}
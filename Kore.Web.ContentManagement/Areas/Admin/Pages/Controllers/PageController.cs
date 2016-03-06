using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Optimization;
using Newtonsoft.Json.Linq;

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
            string json = string.Format(
@"{{
    CircularRelationshipError: '{0}',
    Create: '{1}',
    ContentBlocks: '{2}',
    Delete: '{3}',
    DeleteRecordConfirm: '{4}',
    DeleteRecordError: '{5}',
    DeleteRecordSuccess: '{6}',
    Details: '{7}',
    Edit: '{8}',
    GetRecordError: '{9}',
    GetTranslationError: '{10}',
    PageHistory: '{11}',
    InsertRecordError: '{12}',
    InsertRecordSuccess: '{13}',
    Localize: '{14}',
    Move: '{15}',
    Preview: '{16}',
    Restore: '{17}',
    Toggle: '{18}',
    Translations: '{19}',
    UpdateRecordError: '{20}',
    UpdateRecordSuccess: '{21}',
    UpdateTranslationError: '{22}',
    UpdateTranslationSuccess: '{23}',
    View: '{24}',

    PageHistoryRestoreConfirm: '{25}',
    PageHistoryRestoreError: '{26}',
    PageHistoryRestoreSuccess: '{27}',

    Columns: {{
        Page: {{
            Name: '{28}',
            IsEnabled: '{29}',
            ShowOnMenus: '{30}',
        }},
        PageType: {{
            Name: '{31}',
        }},
        PageVersion: {{
            Title: '{32}',
            Slug: '{33}',
            DateModifiedUtc: '{34}',
        }}
    }}
}}",
   T(KoreCmsLocalizableStrings.Messages.CircularRelationshipError),
   T(KoreWebLocalizableStrings.General.Create),
   T(KoreCmsLocalizableStrings.ContentBlocks.Title),
   T(KoreWebLocalizableStrings.General.Delete),
   T(KoreWebLocalizableStrings.General.ConfirmDeleteRecord),
   T(KoreWebLocalizableStrings.General.DeleteRecordError),
   T(KoreWebLocalizableStrings.General.DeleteRecordSuccess),
   T(KoreWebLocalizableStrings.General.Details),
   T(KoreWebLocalizableStrings.General.Edit),
   T(KoreWebLocalizableStrings.General.GetRecordError).Text,
   T(KoreCmsLocalizableStrings.Messages.GetTranslationError),
   T(KoreCmsLocalizableStrings.Pages.PageHistory),
   T(KoreWebLocalizableStrings.General.InsertRecordError).Text,
   T(KoreWebLocalizableStrings.General.InsertRecordSuccess),
   T(KoreWebLocalizableStrings.General.Localize),
   T(KoreWebLocalizableStrings.General.Move),
   T(KoreWebLocalizableStrings.General.Preview),
   T(KoreCmsLocalizableStrings.Pages.Restore),
   T(KoreWebLocalizableStrings.General.Toggle),
   T(KoreCmsLocalizableStrings.Pages.Translations),
   T(KoreWebLocalizableStrings.General.UpdateRecordError),
   T(KoreWebLocalizableStrings.General.UpdateRecordSuccess),
   T(KoreCmsLocalizableStrings.Messages.UpdateTranslationError),
   T(KoreCmsLocalizableStrings.Messages.UpdateTranslationSuccess),
   T(KoreWebLocalizableStrings.General.View),
   T(KoreCmsLocalizableStrings.Pages.PageHistoryRestoreConfirm),
   T(KoreCmsLocalizableStrings.Pages.PageHistoryRestoreError),
   T(KoreCmsLocalizableStrings.Pages.PageHistoryRestoreSuccess),
   T(KoreCmsLocalizableStrings.Pages.PageModel.Name),
   T(KoreCmsLocalizableStrings.Pages.PageModel.IsEnabled),
   T(KoreCmsLocalizableStrings.Pages.PageModel.ShowOnMenus),
   T(KoreCmsLocalizableStrings.Pages.PageTypeModel.Name),
   T(KoreCmsLocalizableStrings.Pages.PageVersionModel.Title),
   T(KoreCmsLocalizableStrings.Pages.PageVersionModel.Slug),
   T(KoreCmsLocalizableStrings.Pages.PageVersionModel.DateModified));

            return Json(JObject.Parse(json), JsonRequestBehavior.AllowGet);
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
        public ActionResult Preview(Guid pageId)
        {
            var currentCulture = WorkContext.CurrentCultureCode;
            var pageVersion = pageVersionService.Value.GetCurrentVersion(pageId, currentCulture, false, false);

            return PagePreview(pageVersion);
        }

        [Compress]
        [Route("preview-version/{pageVersionId}")]
        public ActionResult PreviewVersion(Guid pageVersionId)
        {
            PageVersion pageVersion;
            using (var connection = pageVersionService.Value.OpenConnection())
            {
                pageVersion = connection.Query()
                    .Include(x => x.Page)
                    .FirstOrDefault(x => x.Id == pageVersionId);
            }

            return PagePreview(pageVersion);
        }

        private ActionResult PagePreview(PageVersion pageVersion)
        {
            if (pageVersion != null)
            {
                pageVersion.Page.IsEnabled = true; // Override here to make sure it passes the check here: PageSecurityHelper.CheckUserHasAccessToPage

                // If there are access restrictions
                if (!PageSecurityHelper.CheckUserHasAccessToPage(pageVersion.Page, User))
                {
                    return new HttpUnauthorizedResult();
                }

                // Else no restrictions (available for anyone to view)
                WorkContext.SetState("CurrentPageId", pageVersion.Id);
                WorkContext.Breadcrumbs.Add(pageVersion.Title);

                var pageType = pageTypeService.Value.FindOne(pageVersion.Page.PageTypeId);
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
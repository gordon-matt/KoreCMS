using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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

            return View("Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Page.Index");
        }

        [Compress]
        [Route("get-editor-ui/{pageVersionId}")]
        public ActionResult GetEditorUI(Guid pageVersionId)
        {
            var pageVersion = pageVersionService.Value.Repository.Table
                .Include(x => x.Page)
                .FirstOrDefault(x => x.Id == pageVersionId);

            var pageType = pageTypeService.Value.FindOne(pageVersion.Page.PageTypeId);
            var korePageTypes = pageTypeService.Value.GetKorePageTypes();

            var korePageType = pageTypeService.Value.GetKorePageType(pageType.Name);
            korePageType.InitializeInstance(pageVersion);

            string content = RenderRazorPartialViewToString(korePageType.EditorTemplatePath, korePageType);
            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        [Route("preview/{pageId}")]
        public ActionResult Preview(Guid pageVersionId)
        {
            var currentCulture = WorkContext.CurrentCultureCode;

            var pageVersion = pageVersionService.Value.Repository.Table
                .Include(x => x.Page)
                .FirstOrDefault(x => x.Id == pageVersionId);

            pageVersion.Page.IsEnabled = true; // Override here to make sure it passes the check here: PageSecurityHelper.CheckUserHasAccessToPage

            //if (page != null && page.IsEnabled)
            if (pageVersion != null)
            {
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
                korePageType.LayoutPath = pageType.LayoutPath;
                korePageType.InitializeInstance(pageVersion);

                var contentBlocks = contentBlockService.Value.GetContentBlocks(pageVersion.Id);
                korePageType.ReplaceContentTokens(x => InsertContentBlocks(x, contentBlocks));

                return View(pageType.DisplayTemplatePath, korePageType);
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
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Linq;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Pages)]
    public class PageController : KoreController
    {
        protected static Regex ContentZonePattern = new Regex(@"\[\[ContentZone:(?<Zone>.*)\]\]", RegexOptions.Compiled);

        private readonly Lazy<IContentBlockService> contentBlockService;
        private readonly Lazy<IHistoricPageService> historicPageService;
        private readonly Lazy<IPageService> pageService;
        private readonly Lazy<IPageTypeService> pageTypeService;
        private readonly Lazy<IZoneService> zoneService;

        public PageController(
            Lazy<IContentBlockService> contentBlockService,
            Lazy<IHistoricPageService> historicPageService,
            Lazy<IPageService> pageService,
            Lazy<IPageTypeService> pageTypeService,
            Lazy<IZoneService> zoneService)
            : base()
        {
            this.contentBlockService = contentBlockService;
            this.historicPageService = historicPageService;
            this.pageService = pageService;
            this.pageTypeService = pageTypeService;
            this.zoneService = zoneService;
        }

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

        [Route("get-editor-ui/{pageId}")]
        public ActionResult GetEditorUI(Guid pageId)
        {
            var page = pageService.Value.FindOne(pageId);
            var pageType = pageTypeService.Value.FindOne(page.PageTypeId);
            var korePageTypes = pageTypeService.Value.GetKorePageTypes();

            var korePageType = pageTypeService.Value.GetKorePageType(pageType.Name);
            korePageType.InitializeInstance(page);

            string content = RenderRazorPartialViewToString(korePageType.EditorTemplatePath, korePageType);
            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }

        [Route("{pageId}/history")]
        public ActionResult History(Guid pageId)
        {
            if (!CheckPermission(CmsPermissions.PageHistoryRead))
            {
                return new HttpUnauthorizedResult();
            }

            var page = pageService.Value.FindOne(pageId);

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.Title), Url.Action("Index"));
            WorkContext.Breadcrumbs.Add(page.Name);
            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.History));

            ViewBag.PageId = pageId;

            ViewBag.Title = T(KoreCmsLocalizableStrings.Pages.Title);
            ViewBag.SubTitle = T(KoreCmsLocalizableStrings.Pages.PageHistory);

            return View("Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Page.History");
        }


        [Route("preview/{pageId}/{isHistoricPage}")]
        public ActionResult Preview(Guid pageId, bool isHistoricPage)
        {
            var currentCulture = WorkContext.CurrentCultureCode;

            Page page = null;

            if (isHistoricPage)
            {
                var historicPage = historicPageService.Value.FindOne(pageId);
                page = new Page
                {
                    AccessRestrictions= historicPage.AccessRestrictions,
                    CultureCode = historicPage.CultureCode,
                    DateCreatedUtc = historicPage.DateCreatedUtc,
                    DateModifiedUtc = historicPage.DateModifiedUtc,
                    Fields = historicPage.Fields,
                    Id = historicPage.PageId,
                    IsEnabled = historicPage.IsEnabled,
                    Name = historicPage.Name,
                    Order = historicPage.Order,
                    PageTypeId = historicPage.PageTypeId,
                    ParentId = historicPage.ParentId,
                    RefId = historicPage.RefId,
                    ShowOnMenus = historicPage.ShowOnMenus,
                    Slug = historicPage.Slug
                };
            }
            else
            {
                page = pageService.Value.FindOne(pageId);
            }

            page.IsEnabled = true; // Override here to make sure it passes the check here: PageSecurityHelper.CheckUserHasAccessToPage

            //if (page != null && page.IsEnabled)
            if (page != null)
            {
                // If there are access restrictions
                if (!PageSecurityHelper.CheckUserHasAccessToPage(page, User))
                {
                    return new HttpUnauthorizedResult();
                }

                // Else no restrictions (available for anyone to view)
                WorkContext.SetState("CurrentPageId", page.Id);
                WorkContext.Breadcrumbs.Add(page.Name);

                var pageType = pageTypeService.Value.FindOne(page.PageTypeId);
                var korePageType = pageTypeService.Value.GetKorePageType(pageType.Name);
                korePageType.InstanceName = page.Name;
                korePageType.InstanceParentId = page.ParentId;
                korePageType.LayoutPath = pageType.LayoutPath;
                korePageType.InitializeInstance(page);

                var contentBlocks = contentBlockService.Value.GetContentBlocks(page.Id);
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
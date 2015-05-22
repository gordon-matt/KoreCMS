using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Kore.Data;
using Kore.Security.Membership;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
{
    [RouteArea(Constants.Areas.Pages)]
    public class PageContentController : KoreController
    {
        protected static Regex ContentZonePattern = new Regex(@"\[\[ContentZone:(?<Zone>.*)\]\]", RegexOptions.Compiled);
        private readonly IContentBlockService contentBlockService;
        private readonly Lazy<IMembershipService> membershipService;
        private readonly IPageService pageService;
        private readonly IPageTypeService pageTypeService;
        private readonly IRepository<Zone> zoneRepository;

        public PageContentController(
            IPageService pageService,
            IPageTypeService pageTypeService,
            IContentBlockService contentBlockService,
            IRepository<Zone> zoneRepository,
            Lazy<IMembershipService> membershipService)
            : base()
        {
            this.pageService = pageService;
            this.pageTypeService = pageTypeService;
            this.contentBlockService = contentBlockService;
            this.zoneRepository = zoneRepository;
            this.membershipService = membershipService;
        }

        //[OutputCache(Duration = 600, VaryByParam = "slug")] //TODO: Uncomment this when ready
        public ActionResult Index(string slug)
        {
            // Hack to make it search the correct path for the view
            if (!this.ControllerContext.RouteData.DataTokens.ContainsKey("area"))
            {
                this.ControllerContext.RouteData.DataTokens.Add("area", Constants.Areas.Pages);
            }

            var currentCulture = WorkContext.CurrentCultureCode;

            var page = pageService.GetPageBySlug(slug, currentCulture);

            if (page == null)
            {
                page = pageService.GetPageBySlug(slug, null);
            }

            if (page != null && page.IsEnabled)
            {
                // If there are access restrictions
                if (!PageSecurityHelper.CheckUserHasAccessToPage(page, User))
                {
                    return new HttpUnauthorizedResult();
                }

                // Else no restrictions (available for anyone to view)
                WorkContext.SetState("CurrentPageId", page.Id);
                WorkContext.Breadcrumbs.Add(page.Name);

                var pageType = pageTypeService.FindOne(page.PageTypeId);
                var korePageType = pageTypeService.GetKorePageType(pageType.Name);
                korePageType.InstanceName = page.Name;
                korePageType.InstanceParentId = page.ParentId;
                korePageType.LayoutPath = pageType.LayoutPath;
                korePageType.InitializeInstance(page);

                var contentBlocks = contentBlockService.GetContentBlocks(page.Id);
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

                var zone = zoneRepository.Table.FirstOrDefault(x => x.Name == zoneName);
                var contentBlocksByZone = contentBlocks.Where(x => x.ZoneId == zone.Id);

                string html = RenderRazorPartialViewToString("Kore.Web.ContentManagement.Views.Frontend.ContentBlocksByZone", contentBlocksByZone);

                content = content.Replace(match.Value, html);
            }
            return content;
        }
    }
}
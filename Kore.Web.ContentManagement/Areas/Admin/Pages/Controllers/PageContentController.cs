using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Services;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers
{
    [RouteArea(Constants.Areas.Pages)]
    public class PageContentController : KoreController
    {
        protected static Regex WidgetZonePattern = new Regex(@"\[\[WidgetZone:(?<Zone>.*)\]\]", RegexOptions.Compiled);
        private readonly IPageService pageService;
        private readonly IPageTypeService pageTypeService;
        private readonly IWidgetService widgetService;
        private readonly IRepository<Zone> zoneRepository;

        public PageContentController(
            IPageService pageService,
            IPageTypeService pageTypeService,
            IWidgetService widgetService,
            IRepository<Zone> zoneRepository)
            : base()
        {
            this.pageService = pageService;
            this.pageTypeService = pageTypeService;
            this.widgetService = widgetService;
            this.zoneRepository = zoneRepository;
        }

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
                WorkContext.SetState("CurrentPageId", page.Id);
                WorkContext.Breadcrumbs.Add(page.Name);

                var pageType = pageTypeService.Find(page.PageTypeId);
                var korePageType = pageTypeService.GetKorePageType(pageType.Name);
                korePageType.InstanceName = page.Name;
                korePageType.InstanceParentId = page.ParentId;
                korePageType.LayoutPath = pageType.LayoutPath;
                korePageType.InitializeInstance(page);

                var widgets = widgetService.GetWidgets(page.Id);
                korePageType.ReplaceContentTokens(x => InsertWidgets(x, widgets));

                return View(pageType.DisplayTemplatePath, korePageType);
            }

            return HttpNotFound();
        }

        private string InsertWidgets(string content, IEnumerable<IWidget> widgets)
        {
            if (string.IsNullOrEmpty(content))
            {
                return null;
            }

            foreach (Match match in WidgetZonePattern.Matches(content))
            {
                string zoneName = match.Groups["Zone"].Value;

                var zone = zoneRepository.Table.FirstOrDefault(x => x.Name == zoneName);
                var widgetsByZone = widgets.Where(x => x.ZoneId == zone.Id);

                string html = RenderRazorPartialViewToString("Kore.Web.ContentManagement.Views.Frontend.WidgetsByZone", widgetsByZone);

                content = content.Replace(match.Value, html);
            }
            return content;
        }
    }
}
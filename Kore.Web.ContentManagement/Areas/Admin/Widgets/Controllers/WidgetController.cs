using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Services;
using Kore.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Widgets)]
    public class WidgetController : KoreController
    {
        private readonly Lazy<IWidgetService> widgetService;

        public WidgetController(Lazy<IWidgetService> widgetService)
        {
            this.widgetService = widgetService;
        }

        [Route("{pageId?}")]
        public ActionResult Index(Guid? pageId)
        {
            if (!CheckPermission(WidgetPermissions.ManageWidgets))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Widgets.Title));

            ViewBag.Title = T(KoreCmsLocalizableStrings.Widgets.ManageWidgets);

            if (pageId.HasValue)
            {
                WorkContext.Breadcrumbs.Add(T(KoreCmsLocalizableStrings.Pages.ManagePages), Url.Action("Index", "Page", new { area = Constants.Areas.Pages }));
            }

            ViewBag.PageId = pageId;

            return View("Kore.Web.ContentManagement.Areas.Admin.Widgets.Views.Widget.Index");
        }

        [Route("get-editor-ui/{widgetId}")]
        public ActionResult GetEditorUI(Guid widgetId)
        {
            var widget = widgetService.Value.Find(widgetId);
            var widgetType = Type.GetType(widget.WidgetType);

            var widgets = EngineContext.Current.ResolveAll<IWidget>();
            var iWidget = widgets.First(x => x.GetType() == widgetType);

            // TODO: See if we can make EditorTemplatePath not so specific a path (just the name), so we can override it in themes, etc
            string content = RenderRazorPartialViewToString(iWidget.EditorTemplatePath, iWidget);
            return Json(new { Content = content }, JsonRequestBehavior.AllowGet);
        }
    }
}
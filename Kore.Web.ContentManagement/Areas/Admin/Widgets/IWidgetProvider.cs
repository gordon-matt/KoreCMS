using System.Collections.Generic;
using System.Linq;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.RuleEngine;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Services;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets
{
    public interface IWidgetProvider
    {
        IEnumerable<IWidget> GetWidgets(string zoneName, string currentCultureCode);
    }

    public class DefaultWidgetProvider : IWidgetProvider
    {
        private readonly IWidgetService widgetService;
        private readonly IRuleManager ruleManager;

        public DefaultWidgetProvider(IWidgetService widgetService, IRuleManager ruleManager)
        {
            this.widgetService = widgetService;
            this.ruleManager = ruleManager;
        }

        public virtual IEnumerable<IWidget> GetWidgets(string zoneName, string currentCultureCode)
        {
            var widgets = widgetService.GetWidgets(zoneName: zoneName);
            return widgets.Where(x => IsVisibleWidget(widgets, x, currentCultureCode)).ToList();
        }

        protected bool IsVisibleWidget(IEnumerable<IWidget> widgets, IWidget widget, string currentCulture)
        {
            if (widget.RefId.HasValue)
            {
                if (!string.Equals(widget.CultureCode, currentCulture))
                {
                    return false;
                }

                var parentWidget = widgets.FirstOrDefault(x => x.Id == widget.RefId.Value);
                if (parentWidget == null || !parentWidget.Enabled)
                {
                    return false;
                }
            }
            else
            {
                var childWidget = widgets.FirstOrDefault(x => x.RefId == widget.Id && x.CultureCode == currentCulture);
                if (childWidget != null)
                {
                    return false;
                }
            }

            return string.IsNullOrEmpty(widget.DisplayCondition) || ruleManager.Matches(widget.DisplayCondition);
        }
    }
}
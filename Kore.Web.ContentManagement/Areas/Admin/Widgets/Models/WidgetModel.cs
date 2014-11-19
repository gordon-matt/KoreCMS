using System;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets.Models
{
    public class WidgetModel
    {
        public Guid Id { get; set; }

        [RoboHidden]
        public Guid? PageId { get; set; }

        [RoboText(IsRequired = true, MaxLength = 255, ContainerCssClass = "col-xs-6 col-md-6", ContainerRowIndex = 0)]
        public string Title { get; set; }

        [RoboChoice(RoboChoiceType.DropDownList, IsRequired = true, LabelText = "Widget Type", ContainerCssClass = "col-xs-6 col-md-6", ContainerRowIndex = 0)]
        [RoboHtmlAttribute("class", "uniform")]
        public string WidgetType { get; set; }

        [RoboChoice(RoboChoiceType.DropDownList, IsRequired = true, LabelText = "Zone", ContainerCssClass = "col-xs-6 col-md-6", ContainerRowIndex = 1)]
        [RoboHtmlAttribute("class", "uniform")]
        public Guid ZoneId { get; set; }

        [RoboNumeric(IsRequired = true, ContainerCssClass = "col-xs-6 col-md-6", ContainerRowIndex = 1)]
        public int Order { get; set; }

        [RoboChoice(RoboChoiceType.CheckBox)]
        public bool IsEnabled { get; set; }

        public string DisplayCondition { get; set; }
    }
}
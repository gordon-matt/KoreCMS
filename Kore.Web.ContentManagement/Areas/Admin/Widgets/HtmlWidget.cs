using System.ComponentModel.DataAnnotations;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets
{
    public class HtmlWidget : WidgetBase
    {
        public override string Name
        {
            get { return "Html Widget"; }
        }

        [Display(Name = "Body Content")]
        public string BodyContent { get; set; }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Widgets.Views.Shared.DisplayTemplates.HtmlWidget"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Widgets.Views.Shared.EditorTemplates.HtmlWidget"; }
        }
    }
}
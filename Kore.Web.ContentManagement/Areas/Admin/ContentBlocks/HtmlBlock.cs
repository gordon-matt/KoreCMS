using System.ComponentModel.DataAnnotations;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public class HtmlBlock : ContentBlockBase
    {
        public override string Name
        {
            get { return "Html Content Block"; }
        }

        [Display(Name = "Body Content")]
        public string BodyContent { get; set; }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.Shared.DisplayTemplates.HtmlBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.Shared.EditorTemplates.HtmlBlock"; }
        }
    }
}
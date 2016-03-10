using Kore.ComponentModel;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public class HtmlBlock : ContentBlockBase
    {
        #region IContentBlock Members

        public override string Name
        {
            get { return "Html Content Block"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.Shared.DisplayTemplates.HtmlBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.Shared.EditorTemplates.HtmlBlock"; }
        }

        #endregion IContentBlock Members

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.BodyContent)]
        [LocalizedHelpText(KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.HelpText.BodyContent)]
        public string BodyContent { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.Script)]
        [LocalizedHelpText(KoreCmsLocalizableStrings.ContentBlocks.HtmlBlock.HelpText.Script)]
        public string Script { get; set; }
    }
}
using System.Web.Mvc;
using Kore.ComponentModel;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public class FormBlock : ContentBlockBase
    {
        [AllowHtml]
        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.HtmlTemplate)]
        public string HtmlTemplate { get; set; }

        [AllowHtml]
        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.ThankYouMessage)]
        public string ThankYouMessage { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.RedirectUrl)]
        public string RedirectUrl { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.EmailAddress)]
        [LocalizedHelpText(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.HelpText.EmailAddress)]
        public string EmailAddress { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.UseAjax)]
        public bool UseAjax { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.FormUrl)]
        [LocalizedHelpText(KoreCmsLocalizableStrings.ContentBlocks.FormBlock.HelpText.FormUrl)]
        public string FormUrl { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Form Content Block"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.Shared.DisplayTemplates.FormBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Views.Shared.EditorTemplates.FormBlock"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
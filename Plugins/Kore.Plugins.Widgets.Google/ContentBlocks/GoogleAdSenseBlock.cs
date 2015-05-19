using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.Google.ContentBlocks
{
    public class GoogleAdSenseBlock : ContentBlockBase
    {
        [LocalizedDisplayName(LocalizableStrings.AdSenseBlock.AdClient)]
        public string AdClient { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AdSenseBlock.AdSlot)]
        public string AdSlot { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AdSenseBlock.Width)]
        public int Width { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AdSenseBlock.Height)]
        public int Height { get; set; }

        [LocalizedDisplayName(LocalizableStrings.AdSenseBlock.EnableLazyLoadAd)]
        public bool EnableLazyLoadAd { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Google: AdSense"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/DisplayTemplates/GoogleAdSenseBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/EditorTemplates/GoogleAdSenseBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
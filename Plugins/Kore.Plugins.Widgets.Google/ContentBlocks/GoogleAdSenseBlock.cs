using System.ComponentModel.DataAnnotations;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.Google.ContentBlocks
{
    public class GoogleAdSenseBlock : ContentBlockBase
    {
        [Display(Name = "Google Ad Client")]
        public string AdClient { get; set; }

        [Display(Name = "Google Ad Slot")]
        public string AdSlot { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        [Display(Name = "Lazy Load")]
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
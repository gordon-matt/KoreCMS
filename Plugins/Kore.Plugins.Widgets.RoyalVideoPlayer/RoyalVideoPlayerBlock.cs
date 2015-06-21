using System.ComponentModel.DataAnnotations;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer
{
    public class RoyalVideoPlayerBlock : ContentBlockBase
    {
        public RoyalVideoPlayerBlock()
        {
        }

        public Skin Skin { get; set; }

        public int PlaylistId { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Royal Video Player"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Widgets.RoyalVideoPlayer/Views/Shared/DisplayTemplates/RoyalVideoPlayerBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Widgets.RoyalVideoPlayer/Views/Shared/EditorTemplates/RoyalVideoPlayerBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }

    public enum Skin : byte
    {
        [Display(Name = "Minimal Dark")]
        MinimalDark = 0,

        [Display(Name = "Minimal White")]
        MinimalWhite = 1,

        [Display(Name = "Classic Dark")]
        ClassicDark = 2,

        [Display(Name = "Classic White")]
        ClassicWhite = 3,

        [Display(Name = "Metal Dark")]
        MetalDark = 4,

        [Display(Name = "Metal White")]
        MetalWhite = 5,

        [Display(Name = "Modern Dark")]
        ModernDark = 6,

        [Display(Name = "Modern White")]
        ModernWhite = 7
    }
}
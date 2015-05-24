using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.Google.ContentBlocks
{
    public class GoogleMapBlock : ContentBlockBase
    {
        public GoogleMapBlock()
        {
            Zoom = 8;
        }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.MapBlock.Latitude)]
        public float Latitude { get; set; }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.MapBlock.Longitude)]
        public float Longitude { get; set; }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.MapBlock.Zoom)]
        public byte Zoom { get; set; }

        [LocalizedDisplayName(LocalizableStrings.ContentBlocks.MapBlock.Height)]
        public short Height { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Google: Map"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Widgets.Google/Views/Shared/DisplayTemplates/GoogleMapBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Widgets.Google/Views/Shared/EditorTemplates/GoogleMapBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
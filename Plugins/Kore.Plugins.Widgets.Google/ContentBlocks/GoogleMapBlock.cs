using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.Google.ContentBlocks
{
    public class GoogleMapBlock : ContentBlockBase
    {
        public GoogleMapBlock()
        {
            Zoom = 8;
        }

        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public byte Zoom { get; set; }

        public short Height { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Google: Map"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/DisplayTemplates/GoogleMapBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Plugins.Widgets.Google/Views/Shared/EditorTemplates/GoogleMapBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
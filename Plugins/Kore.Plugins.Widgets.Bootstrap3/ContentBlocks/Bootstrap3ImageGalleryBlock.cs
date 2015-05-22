using System.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Widgets.Bootstrap3.ContentBlocks
{
    public class Bootstrap3ImageGalleryBlock : ContentBlockBase
    {
        public Bootstrap3ImageGalleryBlock()
        {
            ImagesPerRowXS = ImagesPerRow.Two;
            ImagesPerRowS = ImagesPerRow.Three;
            ImagesPerRowM = ImagesPerRow.Three;
            ImagesPerRowL = ImagesPerRow.Four;
        }

        [DisplayName("Media Folder")]
        public string MediaFolder { get; set; }

        [DisplayName("# Images Per Row (XS)")]
        public ImagesPerRow ImagesPerRowXS { get; set; }

        [DisplayName("# Images Per Row (S)")]
        public ImagesPerRow ImagesPerRowS { get; set; }

        [DisplayName("# Images Per Row (M)")]
        public ImagesPerRow ImagesPerRowM { get; set; }

        [DisplayName("# Images Per Row (L)")]
        public ImagesPerRow ImagesPerRowL { get; set; }

        #region ContentBlockBase Overrides

        public override string DisplayTemplatePath
        {
            get { return "/Plugins/Widgets.Bootstrap3/Views/Shared/DisplayTemplates/ImageGalleryBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "/Plugins/Widgets.Bootstrap3/Views/Shared/EditorTemplates/ImageGalleryBlock.cshtml"; }
        }

        public override string Name
        {
            get { return "Bootstrap 3: Image Gallery"; }
        }

        #endregion ContentBlockBase Overrides

        public enum ImagesPerRow
        {
            Two = 2,
            Three = 3,
            Four = 4,
            Six = 6
        }

        public static string GetThumbSizeCss(ImagesPerRow imagesPerRow, string size = "md")
        {
            switch (imagesPerRow)
            {
                case Bootstrap3ImageGalleryBlock.ImagesPerRow.Two: return string.Concat("col-", size, "-6");
                case Bootstrap3ImageGalleryBlock.ImagesPerRow.Three: return string.Concat("col-", size, "-4");
                case Bootstrap3ImageGalleryBlock.ImagesPerRow.Four: return string.Concat("col-", size, "-3");
                case Bootstrap3ImageGalleryBlock.ImagesPerRow.Six: return string.Concat("col-", size, "-2");
                default:
                    {
                        switch (size)
                        {
                            case "xs": return "col-xs-6";
                            case "lg": return "col-lg-3";
                            case "sm":
                            case "md":
                            default: return "col-md-4";
                        }
                    }
            }
        }
    }
}
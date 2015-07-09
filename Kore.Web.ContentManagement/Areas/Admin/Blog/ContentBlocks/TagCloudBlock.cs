using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.ContentBlocks
{
    public class TagCloudBlock : ContentBlockBase
    {
        public string Colors { get; set; }

        public float? FontSizeFrom { get; set; }

        public float? FontSizeTo { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Blog: Tag Cloud"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.DisplayTemplates.TagCloudBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.EditorTemplates.TagCloudBlock"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
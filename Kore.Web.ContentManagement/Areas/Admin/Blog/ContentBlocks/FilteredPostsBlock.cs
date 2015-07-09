using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.ContentBlocks
{
    public class FilteredPostsBlock : ContentBlockBase
    {
        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.AllPostsBlock.CategoryId)]
        public int? CategoryId { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.AllPostsBlock.TagId)]
        public int? TagId { get; set; }

        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.AllPostsBlock.FilterType)]
        public FilterType FilterType { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Blog: Filtered Posts"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.DisplayTemplates.FilteredPostsBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.EditorTemplates.FilteredPostsBlock"; }
        }

        #endregion ContentBlockBase Overrides
    }

    public enum FilterType : byte
    {
        None = 0,
        Category = 1,
        Tag = 2
    }
}
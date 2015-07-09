using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.ContentBlocks
{
    public class LastNPostsBlock : ContentBlockBase
    {
        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.LastNBlogEntriesBlock.NumberOfEntries)]
        public byte NumberOfEntries { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Blog: Last (N) Posts"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.DisplayTemplates.LastNPostsBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.EditorTemplates.LastNPostsBlock"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
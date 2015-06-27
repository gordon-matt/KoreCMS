using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.ContentBlocks
{
    public class LastNBlogEntriesBlock : ContentBlockBase
    {
        [LocalizedDisplayName(KoreCmsLocalizableStrings.ContentBlocks.LastNBlogEntriesBlock.NumberOfEntries)]
        public byte NumberOfEntries { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Last (N) Blog Entries Block"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.DisplayTemplates.LastNBlogEntriesBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.EditorTemplates.LastNBlogEntriesBlock"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
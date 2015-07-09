using System;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.ContentBlocks
{
    public class AllPostsBlock : ContentBlockBase
    {
        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Blog: All Posts"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.DisplayTemplates.BlogBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { throw new NotSupportedException(); }
        }

        #endregion ContentBlockBase Overrides
    }
}
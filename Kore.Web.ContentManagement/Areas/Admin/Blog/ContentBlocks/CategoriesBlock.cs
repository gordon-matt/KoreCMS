using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Web.ContentManagement.Areas.Admin.Blog.ContentBlocks
{
    public class CategoriesBlock : ContentBlockBase
    {
        public byte NumberOfCategories { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Blog: Categories"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.DisplayTemplates.CategoriesBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Blog.Views.Shared.EditorTemplates.CategoriesBlock"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
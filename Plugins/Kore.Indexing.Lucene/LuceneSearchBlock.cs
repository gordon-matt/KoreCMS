using System;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Indexing.Lucene
{
    public class LuceneSearchBlock : ContentBlockBase
    {
        public bool RenderAsBootstrapNavbarForm { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Lucene Search Form"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Indexing.Lucene.Views.Shared.DisplayTemplates.LuceneSearchBlock"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Indexing.Lucene.Views.Shared.EditorTemplates.LuceneSearchBlock"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
using System;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Indexing.Lucene
{
    public class LuceneSearchBlock : ContentBlockBase
    {
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
            get { throw new NotSupportedException(); }
        }
    }
}
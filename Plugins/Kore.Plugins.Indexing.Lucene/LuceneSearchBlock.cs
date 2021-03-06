﻿using Kore.ComponentModel;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;

namespace Kore.Plugins.Indexing.Lucene
{
    public class LuceneSearchBlock : ContentBlockBase
    {
        public enum LuceneSearchBlockStyle : byte
        {
            Default = 0,
            BootstrapNavbarForm = 1
        }

        [LocalizedDisplayName(LocalizableStrings.LuceneSearchBlock.Style)]
        public LuceneSearchBlockStyle Style { get; set; }

        #region ContentBlockBase Overrides

        public override string Name
        {
            get { return "Lucene Search Form"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "~/Plugins/Indexing.Lucene/Views/Shared/DisplayTemplates/LuceneSearchBlock.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "~/Plugins/Indexing.Lucene/Views/Shared/EditorTemplates/LuceneSearchBlock.cshtml"; }
        }

        #endregion ContentBlockBase Overrides
    }
}
using System;
using System.Collections.Generic;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Indexing;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public abstract class KorePageType : ISearchFieldProvider
    {
        public KorePageType()
        {
            LayoutPath = "~/Views/Shared/_Layout.cshtml";
        }

        public abstract string Name { get; }

        public abstract string DisplayTemplatePath { get; }

        public abstract string EditorTemplatePath { get; }

        #region Instance Properties

        public string InstanceName { get; set; }

        public Guid? InstanceParentId { get; set; }

        public string LayoutPath { get; set; }

        #endregion Instance Properties

        public abstract void InitializeInstance(Page page);

        public abstract IEnumerable<string> IndexFields { get; }

        public abstract void PopulateDocumentIndex(IDocumentIndex document, out string description);

        public abstract void ReplaceContentTokens(Func<string, string> func);
    }
}
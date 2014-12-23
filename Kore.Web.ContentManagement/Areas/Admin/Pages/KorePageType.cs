using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Indexing;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public abstract class KorePageType
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

        public string LayoutPath { get; set; }

        #endregion

        public abstract void InitializeInstance(Page page);

        public abstract void PopulateDocumentIndex(IDocumentIndex document, out string description);
    }
}
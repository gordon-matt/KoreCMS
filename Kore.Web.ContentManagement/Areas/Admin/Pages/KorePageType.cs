using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public abstract class KorePageType
    {
        private string layoutPath = "~/Views/Shared/_Layout.cshtml";

        public abstract string Name { get; }

        public string LayoutPath
        {
            get { return layoutPath; }
            set { layoutPath = value; }
        }

        public abstract string DisplayTemplatePath { get; }

        public abstract string EditorTemplatePath { get; }

        public abstract void InitializeInstance(Page page);
    }
}
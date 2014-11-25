using System;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public abstract class KorePageType
    {
        public abstract string Name { get; }

        public abstract string DisplayTemplatePath { get; }

        public abstract string EditorTemplatePath { get; }
    }
}
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.Indexing;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Domain;
using Kore.Data;
using System.Text;
using System;

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

        public abstract void ReplaceContentTokens(Func<string, string> func);
    }
}
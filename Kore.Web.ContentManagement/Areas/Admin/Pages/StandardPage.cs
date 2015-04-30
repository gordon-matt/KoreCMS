using System;
using System.Collections.Generic;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.Indexing;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class StandardPage : KorePageType
    {
        public override string Name
        {
            get { return "Standard Page"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Shared.DisplayTemplates.StandardPage"; }
        }

        public override string EditorTemplatePath
        {
            get { return "Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Shared.EditorTemplates.StandardPage"; }
        }

        public override void InitializeInstance(Page page)
        {
            if (string.IsNullOrEmpty(page.Fields))
            {
                return;
            }

            dynamic fields = JObject.Parse(page.Fields);
            MetaKeywords = fields.MetaKeywords;
            MetaDescription = fields.MetaDescription;
            BodyContent = fields.BodyContent;
        }

        public override void PopulateDocumentIndex(IDocumentIndex document, out string description)
        {
            document.Add("title", InstanceName).Analyze().Store();
            document.Add("meta_keywords", MetaKeywords).Analyze();
            document.Add("meta_description", MetaDescription).Analyze();
            document.Add("body", BodyContent).Analyze().Store();
            description = BodyContent;
        }

        public override void ReplaceContentTokens(Func<string, string> func)
        {
            BodyContent = func(BodyContent);
        }

        [Searchable]
        public string MetaKeywords { get; set; }

        [Searchable]
        public string MetaDescription { get; set; }

        [Searchable]
        public string BodyContent { get; set; }
    }
}
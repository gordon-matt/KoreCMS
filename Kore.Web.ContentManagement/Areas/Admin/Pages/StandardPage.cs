using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages
{
    public class StandardPage : KorePageType
    {
        public override string Name
        {
            get { return "Standard Page"; }
        }

        //public override string DisplayTemplatePath
        //{
        //    get { return "Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Shared.DisplayTemplates.StandardPage"; }
        //}

        //public override string EditorTemplatePath
        //{
        //    get { return "Kore.Web.ContentManagement.Areas.Admin.Pages.Views.Shared.EditorTemplates.StandardPage"; }
        //}

        public override string DisplayTemplatePath
        {
            get { return "~/Views/Shared/DisplayTemplates/Pages/StandardPage.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "~/Views/Shared/EditorTemplates/Pages/StandardPage.cshtml"; }
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

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public string BodyContent { get; set; }
    }
}
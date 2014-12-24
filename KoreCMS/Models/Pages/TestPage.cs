//using Kore.Web.ContentManagement.Areas.Admin.Pages;
//using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
//using Kore.Web.Indexing;
//using Newtonsoft.Json.Linq;

//namespace KoreCMS.Models.Pages
//{
//    public class TestPage : KorePageType
//    {
//        public override string Name
//        {
//            get { return "Test Page"; }
//        }

//        public override string DisplayTemplatePath
//        {
//            get { return "~/Views/Shared/DisplayTemplates/Pages/TestPage.cshtml"; }
//        }

//        public override string EditorTemplatePath
//        {
//            get { return "~/Views/Shared/EditorTemplates/Pages/TestPage.cshtml"; }
//        }

//        public override void InitializeInstance(Page page)
//        {
//            if (string.IsNullOrEmpty(page.Fields))
//            {
//                return;
//            }

//            dynamic fields = JObject.Parse(page.Fields);
//            TestProperty = fields.TestProperty;
//        }

//        [KorePageTypeProperty]
//        public string TestProperty { get; set; }

//        public override void PopulateDocumentIndex(IDocumentIndex document, out string description)
//        {
//            document.Add("title", InstanceName).Analyze().Store();
//            document.Add("meta_keywords", MetaKeywords).Analyze();
//            document.Add("meta_description", MetaDescription).Analyze();
//            document.Add("body", BodyContent).Analyze().Store();
//            description = BodyContent;
//        }
//    }
//}
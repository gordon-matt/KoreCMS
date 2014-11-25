using Kore.Web.ContentManagement.Areas.Admin.Pages;

namespace KoreCMS.Models.Pages
{
    public class TestPage : KorePageType
    {
        public override string Name
        {
            get { return "Test Page"; }
        }

        public override string DisplayTemplatePath
        {
            get { return "~/Views/Shared/DisplayTemplates/Pages/TestPage.cshtml"; }
        }

        public override string EditorTemplatePath
        {
            get { return "~/Views/Shared/EditorTemplates/Pages/TestPage.cshtml"; }
        }

        [KorePageTypeProperty]
        public string TestProperty { get; set; }
    }
}
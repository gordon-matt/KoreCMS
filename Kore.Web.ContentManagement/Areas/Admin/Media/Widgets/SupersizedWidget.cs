using Kore.Web.ContentManagement.Areas.Admin.Widgets;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Widgets
{
    public class SupersizedWidget : WidgetBase
    {
        public override string Name
        {
            get { return "Supersized Widget"; }
        }

        //public override void RegisterResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        //{
        //    scriptRegister.IncludeBundle("supersized");
        //    scriptRegister.IncludeBundle("supersized");
        //}

        public override string DisplayTemplatePath
        {
            get { throw new System.NotImplementedException(); }
        }

        public override string EditorTemplatePath
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}
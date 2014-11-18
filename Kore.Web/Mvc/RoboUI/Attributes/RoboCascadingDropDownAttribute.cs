using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboCascadingDropDownAttribute : RoboControlAttribute
    {
        public string ParentControl { get; set; }

        public bool AbsoluteParentControl { get; set; }

        public bool AllowMultiple { get; set; }

        public bool EnableChosen { get; set; }

        public string OnSelectedIndexChanged { get; set; }

        public string OnSuccess { get; set; }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            if (!EnableChosen) return;
            scriptRegister.IncludeBundle("jquery-chosen");
            styleRegister.IncludeBundle("jquery-chosen");
        }
    }

    public class RoboCascadingDropDownOptions
    {
        public string ParentControl { get; set; }

        public string SourceUrl { get; set; }

        public string Command { get; set; }
    }
}
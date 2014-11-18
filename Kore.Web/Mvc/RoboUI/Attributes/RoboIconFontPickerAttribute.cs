using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboIconFontPickerAttribute : RoboControlAttribute
    {
        public RoboIconFontPickerAttribute()
        {
            IconSet = "glyphicon";
            Placement = "right";
            Rows = 3;
            Columns = 6;
        }

        /// <summary>
        /// The icon set, glyphicon or fontawesome
        /// </summary>
        public string IconSet { get; set; }

        public string Placement { get; set; }

        public int Rows { get; set; }

        public int Columns { get; set; }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            scriptRegister.IncludeBundle("icon-font-picker");
            styleRegister.IncludeBundle("icon-font-picker");
        }
    }
}
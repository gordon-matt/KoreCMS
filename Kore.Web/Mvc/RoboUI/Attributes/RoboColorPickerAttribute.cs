using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboColorPickerAttribute : RoboChoiceAttribute
    {
        public RoboColorPickerAttribute()
            : base(RoboChoiceType.DropDownList)
        {
        }

        /// <summary>
        /// Show the colors inside a picker instead of inline, default: False
        /// </summary>
        public bool Picker { get; set; }

        /// <summary>
        /// Font to use for the ok/check mark, default: '', available themes: regularfont, fontawesome, glyphicons
        /// </summary>
        public string Theme { get; set; }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            scriptRegister.IncludeBundle("jquery-color-picker");
            styleRegister.IncludeBundle("jquery-color-picker");
        }
    }
}
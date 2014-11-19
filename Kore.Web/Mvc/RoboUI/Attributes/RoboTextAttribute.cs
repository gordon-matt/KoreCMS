using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboTextAttribute : RoboControlAttribute
    {
        public RoboTextAttribute()
        {
            Type = RoboTextType.TextBox;
        }

        public RoboTextAttribute(RoboTextType type)
        {
            Type = type;
        }

        public RoboTextType Type { get; set; }

        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public string EqualTo { get; set; }

        public int Cols { get; set; }

        public int Rows { get; set; }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            if (Type != RoboTextType.RichText) return;
            scriptRegister.IncludeBundle("richtext");
        }
    }

    public enum RoboTextType : byte
    {
        TextBox,
        Password,
        Email,
        Url,
        MultiText,
        RichText
    }
}
using System.Collections.Generic;
using System.Web.Mvc;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public enum RoboChoiceType
    {
        CheckBox,
        CheckBoxList,
        DropDownList,
        RadioButtonList,
    }

    public class RoboChoiceAttribute : RoboControlAttribute
    {
        private readonly RoboChoiceType type;

        public RoboChoiceAttribute(RoboChoiceType type)
        {
            this.type = type;
        }

        public bool AllowMultiple { get; set; }

        public int Columns { get; set; }

        public bool EnableChosen { get; set; }

        public bool GroupedByCategory { get; set; }

        public override bool HideLabelControl
        {
            get
            {
                return type == RoboChoiceType.CheckBox || base.HideLabelControl;
            }
        }

        public string OnSelectedIndexChanged { get; set; }

        public string OptionLabel { get; set; }

        public bool RequiredIfHaveItemsOnly { get; set; }

        public IEnumerable<SelectListItem> SelectListItems { get; set; }

        public RoboChoiceType Type
        {
            get { return type; }
        }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            if (!EnableChosen) return;
            scriptRegister.IncludeBundle("jquery-chosen");
            styleRegister.IncludeBundle("jquery-chosen");
        }
    }
}
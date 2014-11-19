using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboDatePickerAttribute : RoboControlAttribute
    {
        private readonly int? yearRangeMaxOffset;
        private readonly int? yearRangeMinOffset;

        public RoboDatePickerAttribute()
        {
            ShowOn = "both";
            ChangeMonth = false;
            ChangeYear = false;
        }

        public RoboDatePickerAttribute(int yearRangeMinOffset, int yearRangeMaxOffset)
            : this()
        {
            this.yearRangeMinOffset = yearRangeMinOffset;
            this.yearRangeMaxOffset = yearRangeMaxOffset;
        }

        ///<summary>
        /// Change Month
        /// </summary>>
        public bool ChangeMonth { get; set; }

        ///<summary>
        /// Change Year
        /// </summary>>
        public bool ChangeYear { get; set; }

        /// <summary>
        /// The format for parsed and displayed date.
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// Sort Required
        /// </summary>
        /// <returns></returns>
        public bool EnableSortRequired { get; set; }

        /// <summary>
        /// The property name of end date range
        /// </summary>
        public string EndDateRange { get; set; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public string MaximumValue { get; set; }

        /// <summary>
        /// The minimum value.
        /// </summary>
        public string MinimumValue { get; set; }

        public string ShowOn { get; set; }

        /// <summary>
        /// The property name of start date range
        /// </summary>
        public string StartDateRange { get; set; }

        //TODO: rename this.. it's very bad
        public string ToChildrenDate { get; set; }

        /// <summary>
        /// The range of years displayed in the year drop-down of date picker control.
        /// </summary>
        public string YearRangeFormat { get; set; }

        public int? YearRangeMaxOffset
        {
            get { return yearRangeMaxOffset; }
        }

        public int? YearRangeMinOffset
        {
            get { return yearRangeMinOffset; }
        }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            scriptRegister.IncludeBundle("jquery-ui");
            styleRegister.IncludeBundle("jquery-ui");
        }
    }
}
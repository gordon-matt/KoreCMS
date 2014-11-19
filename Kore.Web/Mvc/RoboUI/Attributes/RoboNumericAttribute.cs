namespace Kore.Web.Mvc.RoboUI
{
    public class RoboNumericAttribute : RoboControlAttribute
    {
        /// <summary>
        /// The minimum value.
        /// </summary>
        public string MinimumValue { get; set; }

        /// <summary>
        /// The maximum value.
        /// </summary>
        public string MaximumValue { get; set; }

        /// <summary>
        /// The maximum text length.
        /// </summary>
        public int MaxLength { get; set; }
    }
}
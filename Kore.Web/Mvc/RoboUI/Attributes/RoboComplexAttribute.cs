namespace Kore.Web.Mvc.RoboUI
{
    public class RoboComplexAttribute : RoboControlAttribute
    {
        public RoboComplexAttribute()
        {
            Column = 1;
            EnableGrid = false;
        }

        public override bool HasLabelControl
        {
            get { return false; }
        }

        public int Column { get; set; }

        public bool EnableGrid { get; set; }
    }
}
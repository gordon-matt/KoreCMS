namespace Kore.Web.Mvc.RoboUI
{
    public class RoboHiddenAttribute : RoboControlAttribute
    {
        public override bool HasLabelControl
        {
            get { return false; }
        }
    }
}
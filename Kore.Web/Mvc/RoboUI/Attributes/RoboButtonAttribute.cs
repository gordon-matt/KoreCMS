namespace Kore.Web.Mvc.RoboUI
{
    public class RoboButtonAttribute : RoboControlAttribute
    {
        public RoboButtonAttribute()
        {
            ButtonType = "button";
        }

        public bool Disabled { get; set; }

        public string OnClick { get; set; }

        public string ButtonType { get; set; }
    }
}
using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.FlexSlider.Extensions
{
    public static class StyleRegisterExtensions
    {
        public static void IncludePluginStyle(this StyleRegister register, string style, int? order = null)
        {
            string path = string.Format("/Plugins/Plugins.Widgets.FlexSlider/Content/{0}", style);
            register.IncludeExternal(path, order);
        }
    }
}
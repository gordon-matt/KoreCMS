using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.OwlCarousel.Extensions
{
    public static class StyleRegisterExtensions
    {
        public static void IncludePluginStyle(this StyleRegister register, string style, int? order = null)
        {
            string path = string.Format("/Plugins/Widgets.OwlCarousel/Content/{0}", style);
            register.IncludeExternal(path, order);
        }
    }
}
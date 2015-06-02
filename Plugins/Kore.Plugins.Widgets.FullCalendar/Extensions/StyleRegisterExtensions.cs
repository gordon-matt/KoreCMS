using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.FullCalendar.Extensions
{
    public static class StyleRegisterExtensions
    {
        public static void IncludePluginStyle(this StyleRegister register, string style, int? order = null, object htmlAttributes = null)
        {
            string path = string.Format("/Plugins/Widgets.FullCalendar/Content/{0}", style);
            register.IncludeExternal(path, order, htmlAttributes);
        }
    }
}
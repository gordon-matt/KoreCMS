using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.JQueryFormBuilder.Extensions
{
    public static class StyleRegisterExtensions
    {
        public static void IncludePluginStyle(this StyleRegister register, string style, int? order = null)
        {
            string path = string.Format("/Plugins/Widgets.JQueryFormBuilder/Content/{0}", style);
            register.IncludeExternal(path, order);
        }
    }
}
using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.Bootstrap3.FormBuilder.Extensions
{
    public static class StyleRegisterExtensions
    {
        public static void IncludePluginStyle(this StyleRegister register, string style, int? order = null)
        {
            string path = string.Format("/Plugins/Widgets.Bootstrap3.FormBuilder/Content/{0}", style);
            register.IncludeExternal(path, order);
        }
    }
}
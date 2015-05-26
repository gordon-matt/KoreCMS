using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.Bootstrap3.FormBuilder.Extensions
{
    public static class ScriptRegisterExtensions
    {
        public static void IncludePluginScript(this ScriptRegister register, string script, int? order = null)
        {
            string path = string.Format("/Plugins/Widgets.Bootstrap3.FormBuilder/Scripts/{0}", script);
            register.IncludeExternal(path, order);
        }
    }
}
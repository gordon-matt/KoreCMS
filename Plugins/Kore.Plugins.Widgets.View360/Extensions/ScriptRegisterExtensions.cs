using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.View360.Extensions
{
    public static class ScriptRegisterExtensions
    {
        public static void IncludePluginScript(this ScriptRegister register, string script, int? order = null)
        {
            string path = string.Format("/Plugins/Plugins.Widgets.View360/Scripts/{0}", script);
            register.IncludeExternal(path, order);
        }
    }
}
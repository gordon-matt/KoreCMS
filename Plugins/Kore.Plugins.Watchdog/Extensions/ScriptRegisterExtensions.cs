using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Watchdog.Extensions
{
    public static class ScriptRegisterExtensions
    {
        public static void IncludePluginScript(this ScriptRegister register, string script, int? order = null)
        {
            string path = string.Format("/Plugins/Maintenance.Watchdog/Scripts/{0}", script);
            register.IncludeExternal(path, order);
        }
    }
}
using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Ecommerce.Simple.Extensions
{
    public static class ScriptRegisterExtensions
    {
        public static void IncludePluginScript(this ScriptRegister register, string script, int? order = null)
        {
            string path = string.Format("/Plugins/Ecommerce.Simple/Scripts/{0}", script);
            register.IncludeExternal(path, order);
        }
    }
}
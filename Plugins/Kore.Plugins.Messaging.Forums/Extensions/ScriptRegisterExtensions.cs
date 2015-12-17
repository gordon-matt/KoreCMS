using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Messaging.Forums.Extensions
{
    public static class ScriptRegisterExtensions
    {
        public static void IncludePluginScript(this ScriptRegister register, string script, int? order = null, object htmlAttributes = null)
        {
            string path = string.Format("/Plugins/Messaging.Forums/Scripts/{0}", script);
            register.IncludeExternal(path, order, htmlAttributes);
        }
    }
}
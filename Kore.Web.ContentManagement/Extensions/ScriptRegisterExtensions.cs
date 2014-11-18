using Kore.Web.Mvc.Resources;

namespace Kore.Web.ContentManagement.Extensions
{
    public static class ScriptRegisterExtensions
    {
        public static void IncludePluginScript(this ScriptRegister register, string script, int? order = null)
        {
            string path = string.Format("/Plugins/Kore.Web.ContentManagement/Scripts/{0}", script);
            register.IncludeExternal(path, order);
        }

        public static void IncludePluginScript(this ScriptRegister register, string area, string script, int? order = null)
        {
            string path = string.Format("/Plugins/Kore.Web.ContentManagement/Areas/{0}/Scripts/{1}", area, script);
            register.IncludeExternal(path, order);
        }
    }
}
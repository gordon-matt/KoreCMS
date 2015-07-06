using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.RevolutionSlider.Extensions
{
    public static class ScriptRegisterExtensions
    {
        public static void IncludePluginScript(this ScriptRegister register, string script, int? order = null)
        {
            string path = string.Format("/Plugins/Widgets.RevolutionSlider/Scripts/{0}", script);
            register.IncludeExternal(path, order);
        }
    }
}
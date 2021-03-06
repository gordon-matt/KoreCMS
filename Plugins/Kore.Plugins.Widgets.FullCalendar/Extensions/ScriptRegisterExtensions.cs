﻿using Kore.Web.Mvc.Resources;

namespace Kore.Plugins.Widgets.FullCalendar.Extensions
{
    public static class ScriptRegisterExtensions
    {
        public static void IncludePluginScript(this ScriptRegister register, string script, int? order = null, object htmlAttributes = null)
        {
            string path = string.Format("/Plugins/Widgets.FullCalendar/Scripts/{0}", script);
            register.IncludeExternal(path, order, htmlAttributes);
        }
    }
}
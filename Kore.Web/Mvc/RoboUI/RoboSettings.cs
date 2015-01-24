using System.Collections.Generic;
using Kore.Infrastructure;
using Kore.Web.Configuration;
using Kore.Web.Mvc.RoboUI.Providers;

namespace Kore.Web.Mvc.RoboUI
{
    public static class RoboSettings
    {
        static RoboSettings()
        {
            var siteSettings = EngineContext.Current.Resolve<KoreSiteSettings>();

            AreaLayoutPaths = new Dictionary<string, string>();

            DefaultLayoutPath = string.IsNullOrEmpty(siteSettings.DefaultFrontendLayoutPath)
                ? "~/Views/Shared/_Layout.cshtml"
                : siteSettings.DefaultFrontendLayoutPath;

            DefaultFormProvider = new Bootstrap3RoboUIFormProvider();
            DefaultGridProvider = new JQGridRoboUIGridProvider();
        }

        public static Dictionary<string, string> AreaLayoutPaths { get; private set; }

        public static string DefaultLayoutPath { get; set; }

        public static IRoboUIFormProvider DefaultFormProvider { get; set; }

        public static IRoboUIGridProvider DefaultGridProvider { get; set; }

        public static void RegisterAreaLayoutPath(string area, string layoutPath)
        {
            if (!AreaLayoutPaths.ContainsKey(area))
            {
                AreaLayoutPaths.Add(area, layoutPath);
            }
        }
    }
}
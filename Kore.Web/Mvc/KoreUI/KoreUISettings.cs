﻿using System.Collections.Generic;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public static class KoreUISettings
    {
        private static IKoreUIProvider defaultAdminProvider;
        private static IKoreUIProvider defaultFrontendProvider;

        static KoreUISettings()
        {
            AreaUIProviders = new Dictionary<string, IKoreUIProvider>();
        }

        public static Dictionary<string, IKoreUIProvider> AreaUIProviders { get; private set; }

        public static IKoreUIProvider DefaultAdminProvider
        {
            get { return defaultAdminProvider ?? (defaultAdminProvider = new Bootstrap3UIProvider()); }
            set { defaultAdminProvider = value; }
        }

        public static IKoreUIProvider DefaultFrontendProvider
        {
            get { return defaultFrontendProvider ?? (defaultFrontendProvider = new Bootstrap3UIProvider()); }
            set { defaultFrontendProvider = value; }
        }

        public static void RegisterAreaUIProvider(string area, IKoreUIProvider provider)
        {
            if (!AreaUIProviders.ContainsKey(area))
            {
                AreaUIProviders.Add(area, provider);
            }
        }
    }
}
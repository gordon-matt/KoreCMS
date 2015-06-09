using System.Collections.Generic;
using Kore.Web.Mvc.KoreUI.Providers;

namespace Kore.Web.Mvc.KoreUI
{
    public static class KoreUISettings
    {
        private static IKoreUIProvider defaultProvider;

        static KoreUISettings()
        {
            AreaUIProviders = new Dictionary<string, IKoreUIProvider>();
        }

        public static Dictionary<string, IKoreUIProvider> AreaUIProviders { get; private set; }

        public static IKoreUIProvider DefaultProvider
        {
            get { return defaultProvider ?? (defaultProvider = new Bootstrap3UIProvider()); }
            set { defaultProvider = value; }
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
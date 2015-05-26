using System.Collections.Generic;
using Kore.Infrastructure;

namespace Kore.Localization
{
    public static class LocalizationUtilities
    {
        public static Localizer Resolve(string scope = "defaultLocalizer")
        {
            try
            {
                return EngineContext.Current.Resolve<IText>(new Dictionary<string, object> { { "scope", scope } }).Get;
                //return KoreContext.DIContainerAdapter.Resolve<IText>(new Dictionary<string, object> { { "scope", scope } }).Get;
            }
            catch
            {
                return NullLocalizer.Instance;
            }
        }
    }
}
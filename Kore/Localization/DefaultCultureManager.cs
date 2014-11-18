using System.Text.RegularExpressions;
using System.Threading;

namespace Kore.Localization
{
    public class DefaultCultureManager : ICultureManager
    {
        #region ICultureManager Members

        public string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentUICulture.Name;
        }

        public bool IsValidCulture(string cultureName)
        {
            var cultureRegex = new Regex(@"\w{2}(-\w{2,})*");
            if (cultureRegex.IsMatch(cultureName))
            {
                return true;
            }
            return false;
        }

        #endregion ICultureManager Members
    }
}
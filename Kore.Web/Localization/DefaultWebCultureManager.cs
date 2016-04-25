using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kore.Localization;
using Kore.Web.Localization.Services;

namespace Kore.Web.Localization
{
    public class DefaultWebCultureManager : DefaultCultureManager, IWebCultureManager
    {
        private readonly IEnumerable<ICultureSelector> cultureSelectors;

        public DefaultWebCultureManager(IEnumerable<ICultureSelector> cultureSelectors)
        {
            this.cultureSelectors = cultureSelectors;
        }

        #region IWebCultureManager Members

        public virtual string GetCurrentCulture(HttpContextBase requestContext)
        {
            var requestedCultures = cultureSelectors
                .Select(x => x.GetCulture(requestContext))
                .ToList()
                .Where(x => x != null)
                .OrderByDescending(x => x.Priority);

            string cultureCode = null;

            if (requestedCultures.Any())
            {
                cultureCode = requestedCultures.First().CultureCode;
            }

            if (cultureCode == string.Empty)
            {
                cultureCode = null;
            }

            return cultureCode;
        }

        #endregion IWebCultureManager Members
    }
}
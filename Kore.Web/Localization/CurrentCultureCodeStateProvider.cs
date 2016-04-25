using System;
using Kore.Caching;

namespace Kore.Web.Localization
{
    public class CurrentCultureCodeStateProvider : IWorkContextStateProvider
    {
        private readonly ICacheManager cacheManager;
        private readonly IWebCultureManager cultureManager;

        public CurrentCultureCodeStateProvider(
            ICacheManager cacheManager,
            IWebCultureManager cultureManager)
        {
            this.cacheManager = cacheManager;
            this.cultureManager = cultureManager;
        }

        #region IWorkContextStateProvider Members

        public Func<IWebWorkContext, T> Get<T>(string name)
        {
            if (name == KoreWebConstants.StateProviders.CurrentCultureCode)
            {
                return ctx => cacheManager.Get(KoreWebConstants.CacheKeys.CurrentCulture, () =>
                {
                    return (T)(object)cultureManager.GetCurrentCulture(ctx.HttpContext);
                });
            }
            return null;
        }

        #endregion IWorkContextStateProvider Members
    }
}
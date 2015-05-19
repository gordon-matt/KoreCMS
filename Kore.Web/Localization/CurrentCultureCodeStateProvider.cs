using System;

namespace Kore.Web.Localization
{
    public class CurrentCultureCodeStateProvider : IWorkContextStateProvider
    {
        private readonly IWebCultureManager cultureManager;

        public CurrentCultureCodeStateProvider(IWebCultureManager cultureManager)
        {
            this.cultureManager = cultureManager;
        }

        #region IWorkContextStateProvider Members

        public Func<IWebWorkContext, T> Get<T>(string name)
        {
            if (name == KoreWebConstants.StateProviders.CurrentCultureCode)
            {
                return ctx => (T)(object)cultureManager.GetCurrentCulture(ctx.HttpContext);
            }
            return null;
        }

        #endregion IWorkContextStateProvider Members
    }
}
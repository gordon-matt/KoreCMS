using System;
using Kore.Localization;

namespace Kore.Web.Localization
{
    public class CurrentCultureCodeStateProvider : IWorkContextStateProvider
    {
        private readonly ICultureManager cultureManager;

        public CurrentCultureCodeStateProvider(ICultureManager cultureManager)
        {
            this.cultureManager = cultureManager;
        }

        #region IWorkContextStateProvider Members

        public Func<WorkContext, T> Get<T>(string name)
        {
            if (name == KoreWebConstants.StateProviders.CurrentCultureCode)
            {
                return ctx => (T)(object)cultureManager.GetCurrentCulture();
            }
            return null;
        }

        #endregion IWorkContextStateProvider Members
    }
}
namespace Kore.Localization
{
    public class CurrentCultureStateProvider : IAppContextStateProvider
    {
        #region IAppContextStateProvider Members

        public T Get<T>(string name)
        {
            if (name == KoreConstants.StateProviderNames.CurrentCulture)
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ConvertTo<T>();
            }
            return default(T);
        }

        #endregion IAppContextStateProvider Members
    }
}
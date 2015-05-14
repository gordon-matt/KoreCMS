using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Localization.Domain;

namespace Kore.Localization
{
    public class DefaultLocalizedStringManager : ILocalizedStringManager
    {
        private readonly ICacheManager cacheManager;
        private readonly object objSync = new object();

        public DefaultLocalizedStringManager(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        #region ILocalizedStringManager Members

        public virtual string GetLocalizedString(string text, string cultureCode)
        {
            return GetResource(text, cultureCode);
        }

        #endregion ILocalizedStringManager Members

        protected virtual string GetResource(string key, string cultureCode)
        {
            lock (objSync)
            {
                var resourceCache = LoadCulture(cultureCode);

                if (resourceCache.ContainsKey(key))
                {
                    return resourceCache[key];
                }

                var invariantResourceCache = LoadCulture(null);

                if (invariantResourceCache.ContainsKey(key))
                {
                    return invariantResourceCache[key];
                }

                AddTranslation(null, key, key);

                invariantResourceCache.Add(key, key);
            }

            return key;
        }

        protected virtual IDictionary<string, string> LoadCulture(string cultureCode)
        {
            string cacheKey = string.Concat("Kore_LocalizableStrings_", cultureCode);
            return cacheManager.Get<Dictionary<string, string>>(cacheKey, () =>
            {
                return LoadTranslationsForCulture(cultureCode);
            });
        }

        protected virtual Dictionary<string, string> LoadTranslationsForCulture(string cultureCode)
        {
            var repository = EngineContext.Current.Resolve<IRepository<LocalizableString>>();

            if (string.IsNullOrEmpty(cultureCode))
            {
                return LoadTranslations(repository.Table.Where(x => x.CultureCode == null).ToList());
            }

            return LoadTranslations(repository.Table.Where(x => x.CultureCode == cultureCode).ToList());
        }

        private static Dictionary<string, string> LoadTranslations(IEnumerable<LocalizableString> items)
        {
            var dictionary = new Dictionary<string, string>();

            foreach (var item in items.Where(item => !dictionary.ContainsKey(item.TextKey)))
            {
                dictionary.Add(item.TextKey, item.TextValue);
            }

            return dictionary;
        }

        protected virtual void AddTranslation(string cultureCode, string key, string value)
        {
            var repository = EngineContext.Current.Resolve<IRepository<LocalizableString>>();
            repository.Insert(new LocalizableString
            {
                Id = Guid.NewGuid(),
                CultureCode = cultureCode,
                TextKey = key,
                TextValue = value
            });
        }
    }
}
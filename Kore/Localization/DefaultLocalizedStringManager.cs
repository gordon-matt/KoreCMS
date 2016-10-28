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
        private readonly IWorkContext workContext;
        private readonly object objSync = new object();

        public DefaultLocalizedStringManager(ICacheManager cacheManager, IWorkContext workContext)
        {
            this.cacheManager = cacheManager;
            this.workContext = workContext;
        }

        #region ILocalizedStringManager Members

        public virtual string GetLocalizedString(string key, string cultureCode)
        {
            int tenantId = workContext.CurrentTenant.Id;

            lock (objSync)
            {
                var resourceCache = LoadCulture(tenantId, cultureCode);

                if (resourceCache.ContainsKey(key))
                {
                    return resourceCache[key];
                }

                var invariantResourceCache = LoadCulture(tenantId, null);

                if (invariantResourceCache.ContainsKey(key))
                {
                    return invariantResourceCache[key];
                }

                string value = AddTranslation(tenantId, null, key);

                invariantResourceCache.Add(key, value);
            }

            return key;
        }

        #endregion ILocalizedStringManager Members

        protected virtual IDictionary<string, string> LoadCulture(int tenantId, string cultureCode)
        {
            string cacheKey = string.Concat("Kore_LocalizableStrings_", cultureCode);
            return cacheManager.Get<Dictionary<string, string>>(cacheKey, () =>
            {
                return LoadTranslationsForCulture(tenantId, cultureCode);
            });
        }

        protected virtual Dictionary<string, string> LoadTranslationsForCulture(int tenantId, string cultureCode)
        {
            var repository = EngineContext.Current.Resolve<IRepository<LocalizableString>>();

            if (string.IsNullOrEmpty(cultureCode))
            {
                return LoadTranslations(repository.Find(x => x.TenantId == tenantId && x.CultureCode == null));
            }

            return LoadTranslations(repository.Find(x => x.TenantId == tenantId && x.CultureCode == cultureCode));
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

        protected virtual string AddTranslation(int tenantId, string cultureCode, string key)
        {
            // TODO: Consider resolving this once for better performance?
            var providers = EngineContext.Current.ResolveAll<ILanguagePack>();
            var languagePacks = providers.Where(x => x.CultureCode == null);

            string value = key;

            foreach (var languagePack in languagePacks)
            {
                if (languagePack.LocalizedStrings.ContainsKey(key))
                {
                    value = languagePack.LocalizedStrings[key];
                    break;
                }
            }

            var repository = EngineContext.Current.Resolve<IRepository<LocalizableString>>();
            repository.Insert(new LocalizableString
            {
                Id = Guid.NewGuid(),
                TenantId = tenantId,
                CultureCode = cultureCode,
                TextKey = key,
                TextValue = value
            });
            return value;
        }
    }
}
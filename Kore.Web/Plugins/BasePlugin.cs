using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Localization.Domain;

namespace Kore.Web.Plugins
{
    public abstract class BasePlugin : IPlugin
    {
        protected BasePlugin()
        {
        }

        /// <summary>
        /// Gets or sets the plugin descriptor
        /// </summary>
        public virtual PluginDescriptor PluginDescriptor { get; set; }

        /// <summary>
        /// Install plugin
        /// </summary>
        public virtual void Install()
        {
            PluginManager.MarkPluginAsInstalled(this.PluginDescriptor.SystemName);
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public virtual void Uninstall()
        {
            PluginManager.MarkPluginAsUninstalled(this.PluginDescriptor.SystemName);
        }

        protected virtual void InstallLocalizableStrings<TProvider>() where TProvider : IDefaultLocalizableStringsProvider, new()
        {
            var toInsert = new HashSet<LocalizableString>();

            var translations = new TProvider().GetTranslations();
            foreach (var translation in translations)
            {
                foreach (var localizedString in translation.LocalizedStrings)
                {
                    toInsert.Add(new LocalizableString
                    {
                        Id = Guid.NewGuid(),
                        CultureCode = translation.CultureCode,
                        TextKey = localizedString.Key,
                        TextValue = localizedString.Value
                    });
                }
            }

            var localizableStringRepository = EngineContext.Current.Resolve<IRepository<LocalizableString>>();
            localizableStringRepository.Insert(toInsert);

            var cacheManager = EngineContext.Current.Resolve<ICacheManager>();
            cacheManager.Remove("LocalizableStrings_");
        }

        protected virtual void UninstallLocalizableStrings<TProvider>() where TProvider : IDefaultLocalizableStringsProvider, new()
        {
            var translations = new TProvider().GetTranslations();

            var distinctKeys = translations
                .SelectMany(x => x.LocalizedStrings.Select(y => y.Key))
                .Distinct();

            var localizableStringRepository = EngineContext.Current.Resolve<IRepository<LocalizableString>>();
            var toDelete = localizableStringRepository.Table.Where(x => distinctKeys.Contains(x.TextKey));
            localizableStringRepository.Delete(toDelete);

            var cacheManager = EngineContext.Current.Resolve<ICacheManager>();
            cacheManager.Remove("LocalizableStrings_");
        }
    }
}
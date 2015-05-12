using System;
using System.Linq;
using Kore.Caching;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Data.Services;
using Kore.Infrastructure;

namespace Kore.Web.Configuration
{
    public interface ISettingService
    {
        TSettings GetSettings<TSettings>() where TSettings : ISettings, new();

        ISettings GetSettings(Type settingsType);

        void SaveSettings(string key, string value);

        void SaveSettings<TSettings>(TSettings settings) where TSettings : ISettings;
    }

    public class DefaultSettingService : GenericDataService<Setting>, ISettingService
    {
        private readonly ICacheManager cacheManager;

        public DefaultSettingService(IRepository<Setting> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        public TSettings GetSettings<TSettings>() where TSettings : ISettings, new()
        {
            string key = string.Format(KoreWebConstants.CacheKeys.SettingsByType, typeof(TSettings).FullName);
            return cacheManager.Get<TSettings>(key, () =>
            {
                var settings = Repository.Table.Where(x => x.Type == key).FirstOrDefault();
                if (settings == null || string.IsNullOrEmpty(settings.Value))
                {
                    return new TSettings();
                }

                return settings.Value.JsonDeserialize<TSettings>();
            });
        }

        public ISettings GetSettings(Type settingsType)
        {
            string key = string.Format(KoreWebConstants.CacheKeys.SettingsByType, settingsType.FullName);
            return cacheManager.Get<ISettings>(key, () =>
            {
                var settings = Repository.Table.Where(x => x.Type == key).FirstOrDefault();
                if (settings == null || string.IsNullOrEmpty(settings.Value))
                {
                    return (ISettings)Activator.CreateInstance(settingsType);
                }

                return (ISettings)settings.Value.JsonDeserialize(settingsType);
            });
        }

        public void SaveSettings(string key, string value)
        {
            var setting = Repository.Table.Where(x => x.Type == key).FirstOrDefault();
            if (setting == null)
            {
                var iSettings = EngineContext.Current.ResolveAll<ISettings>().FirstOrDefault(x => x.GetType().FullName == key);

                if (iSettings != null)
                {
                    setting = new Setting { Name = iSettings.Name, Type = key, Value = value };
                    Insert(setting);
                }
            }
            else
            {
                setting.Value = value;
                Update(setting);
            }
        }

        public void SaveSettings<TSettings>(TSettings settings) where TSettings : ISettings
        {
            var type = settings.GetType();
            var key = type.FullName;
            var value = settings.ToJson();
            SaveSettings(key, value);
        }
    }
}
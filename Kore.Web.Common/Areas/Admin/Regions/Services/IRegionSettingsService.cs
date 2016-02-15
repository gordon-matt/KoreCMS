using Kore.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.Common.Areas.Admin.Regions.Domain;

namespace Kore.Web.Common.Areas.Admin.Regions.Services
{
    public interface IRegionSettingsService : IGenericDataService<RegionSettings>
    {
        T GetSettings<T>(int regionId) where T : IRegionSettings;

        Dictionary<int, T> GetSettings<T>(IEnumerable<int> regionIds = null) where T : IRegionSettings;
    }

    public class RegionSettingsService : GenericDataService<RegionSettings>, IRegionSettingsService
    {
        public RegionSettingsService(ICacheManager cacheManager, IRepository<RegionSettings> repository)
            : base(cacheManager, repository)
        {
        }

        public T GetSettings<T>(int regionId) where T : IRegionSettings
        {
            var instance = Activator.CreateInstance<T>();
            string settingsId = instance.Name.ToSlugUrl();

            var settings = FindOne(x =>
                x.SettingsId == settingsId &&
                x.RegionId == regionId);

            return settings.Fields.JsonDeserialize<T>();
        }

        public Dictionary<int, T> GetSettings<T>(IEnumerable<int> regionIds = null) where T : IRegionSettings
        {
            var instance = Activator.CreateInstance<T>();
            string settingsId = instance.Name.ToSlugUrl();

            IEnumerable<RegionSettings> settings;

            if (regionIds.IsNullOrEmpty())
            {
                settings = Find(x => x.SettingsId == settingsId);
            }
            else
            {
                settings = Find(x =>
                   x.SettingsId == settingsId &&
                   regionIds.Contains(x.RegionId));
            }

            return settings.ToDictionary(k => k.RegionId, v => v.Fields.JsonDeserialize<T>());
        }
    }
}
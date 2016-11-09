using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using LanguageEntity = Kore.Localization.Domain.Language;

namespace Kore.Localization.Services
{
    public interface ILanguageService : IGenericDataService<LanguageEntity>
    {
        bool CheckIfRightToLeft(int tenantId, string cultureCode);
    }

    public class LanguageService : GenericDataService<LanguageEntity>, ILanguageService
    {
        public LanguageService(ICacheManager cacheManager, IRepository<LanguageEntity> repository)
            : base(cacheManager, repository)
        {
        }

        public bool CheckIfRightToLeft(int tenantId, string cultureCode)
        {
            var rtlLanguages = CacheManager.Get("Repository_Language_RightToLeft_" + tenantId, () =>
            {
                using (var connection = OpenConnection())
                {
                    return connection.Query(x => x.TenantId == tenantId && x.IsRTL)
                        .Select(k => k.CultureCode)
                        .ToList();
                }
            });

            return rtlLanguages.Contains(cultureCode);
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            CacheManager.RemoveByPattern("Repository_Language_RightToLeft_.*");
        }
    }
}
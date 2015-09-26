using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Localization.Domain;

namespace Kore.Localization.Services
{
    public interface ILocalizablePropertyService : IGenericDataService<LocalizableProperty>
    {
    }

    public class LocalizablePropertyService : GenericDataService<LocalizableProperty>, ILocalizablePropertyService
    {
        public LocalizablePropertyService(ICacheManager cacheManager, IRepository<LocalizableProperty> repository)
            : base(cacheManager, repository)
        {
        }
    }
}
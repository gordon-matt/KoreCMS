using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Localization.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Services
{
    public interface ILanguageService : IGenericDataService<Language>
    {
        IEnumerable<Language> GetActiveLanguages();

        Language GetLanguage(string cultureCode);

        bool CheckIfRightToLeft(string cultureCode);
    }

    public class LanguageService : GenericDataService<Language>, ILanguageService
    {
        private readonly ICacheManager cacheManager;

        public LanguageService(IRepository<Language> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        public override int Delete(Language record)
        {
            int rowsAffected = base.Delete(record);
            cacheManager.Remove(Constants.CacheKeys.LanguagesAll);
            cacheManager.Remove(string.Format(Constants.CacheKeys.LanguagesForCultureCode, record.CultureCode));
            if (record.IsRTL)
            {
                cacheManager.Remove(Constants.CacheKeys.LanguagesRightToLeft);
            }
            return rowsAffected;
        }

        public IEnumerable<Language> Get()
        {
            return cacheManager.Get(Constants.CacheKeys.LanguagesAll, () =>
            {
                return Repository.Table.ToList();
            });
        }

        public IEnumerable<Language> GetActiveLanguages()
        {
            return cacheManager.Get(Constants.CacheKeys.LanguagesActive, () =>
            {
                return Repository.Table.Where(x => x.IsEnabled).ToList();
            });
        }

        public Language GetLanguage(string cultureCode)
        {
            if (string.IsNullOrEmpty(cultureCode))
            {
                return null;
            }

            return cacheManager.Get(string.Format(Constants.CacheKeys.LanguagesForCultureCode, cultureCode), () =>
            {
                var culture = Repository.Table.FirstOrDefault(x => x.CultureCode == cultureCode);
                if (culture == null)
                {
                    try
                    {
                        var parent = CultureInfo.GetCultureInfo(cultureCode);
                        var regionalLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => x.Parent.Equals(parent));
                        foreach (var language in regionalLanguages)
                        {
                            culture = Repository.Table.FirstOrDefault(x => x.CultureCode == language.Name);
                            if (culture == null) continue;
                            break;
                        }
                    }
                    catch (CultureNotFoundException)
                    {
                        culture = null;
                    }
                }

                return culture;
            });
        }

        public override int Insert(Language record)
        {
            int rowsAffected = base.Insert(record);
            cacheManager.Remove(Constants.CacheKeys.LanguagesAll);
            cacheManager.Remove(string.Format(Constants.CacheKeys.LanguagesForCultureCode, record.CultureCode));
            if (record.IsRTL)
            {
                cacheManager.Remove(Constants.CacheKeys.LanguagesRightToLeft);
            }
            return rowsAffected;
        }

        public override int Update(Language record)
        {
            int rowsAffected = base.Update(record);
            cacheManager.Remove(Constants.CacheKeys.LanguagesAll);
            cacheManager.Remove(string.Format(Constants.CacheKeys.LanguagesForCultureCode, record.CultureCode));
            if (record.IsRTL)
            {
                cacheManager.Remove(Constants.CacheKeys.LanguagesRightToLeft);
            }
            return rowsAffected;
        }

        public bool CheckIfRightToLeft(string cultureCode)
        {
            var rtlLanguages = cacheManager.Get(Constants.CacheKeys.LanguagesRightToLeft, () =>
            {
                return Repository.Table
                    .Where(x => x.IsRTL)
                    .Select(k => k.CultureCode)
                    .ToList();
            });

            return rtlLanguages.Contains(cultureCode);
        }
    }
}
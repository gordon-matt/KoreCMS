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

        // TODO: move this to cache?
        private static List<string> rightToLeftLanguages;

        public LanguageService(IRepository<Language> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        public override int Delete(Language record)
        {
            int rowsAffected = base.Delete(record);
            cacheManager.Remove("Languages_All");
            rightToLeftLanguages = null;
            return rowsAffected;
        }

        public IEnumerable<Language> Get()
        {
            return cacheManager.Get("Languages_All", () =>
            {
                return Repository.Table.ToList();
            });
        }

        public IEnumerable<Language> GetActiveLanguages()
        {
            return cacheManager.Get("Languages_ActiveLanguages", () =>
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

            return cacheManager.Get("Language_" + cultureCode, () =>
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
            cacheManager.Remove("Languages_All");
            rightToLeftLanguages = null;
            return rowsAffected;
        }

        public override int Update(Language record)
        {
            int rowsAffected = base.Update(record);
            cacheManager.Remove("Languages_All");
            rightToLeftLanguages = null;
            return rowsAffected;
        }

        public bool CheckIfRightToLeft(string cultureCode)
        {
            if (rightToLeftLanguages == null)
            {
                rightToLeftLanguages = Repository.Table
                    .Where(x => x.IsRTL)
                    .Select(k => k.CultureCode)
                    .ToList();
            }

            return rightToLeftLanguages.Contains(cultureCode);
        }
    }
}
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
        public LanguageService(ICacheManager cacheManager, IRepository<Language> repository)
            : base(cacheManager, repository)
        {
        }

        public IEnumerable<Language> GetActiveLanguages()
        {
            return Find(x => x.IsEnabled);
        }

        public Language GetLanguage(string cultureCode)
        {
            if (string.IsNullOrEmpty(cultureCode))
            {
                return null;
            }

            return CacheManager.Get(string.Format(CacheKeyFiltered, cultureCode), () =>
            {
                var language = Repository.Table.FirstOrDefault(x => x.CultureCode == cultureCode);
                if (language == null)
                {
                    try
                    {
                        var parent = CultureInfo.GetCultureInfo(cultureCode);
                        var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).Where(x => x.Parent.Equals(parent));
                        foreach (var cultureInfo in cultures)
                        {
                            language = Repository.Table.FirstOrDefault(x => x.CultureCode == cultureInfo.Name);
                            if (language != null)
                            {
                                break;
                            }
                        }
                    }
                    catch (CultureNotFoundException)
                    {
                        language = null;
                    }
                }

                return language;
            });
        }

        public bool CheckIfRightToLeft(string cultureCode)
        {
            var rtlLanguages = CacheManager.Get("Repository_Language_RightToLeft", () =>
            {
                return Repository.Table
                    .Where(x => x.IsRTL)
                    .Select(k => k.CultureCode)
                    .ToList();
            });

            return rtlLanguages.Contains(cultureCode);
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            CacheManager.Remove("Repository_Language_RightToLeft");
        }
    }
}
using System;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Services
{
    public interface IPageService : IGenericDataService<Page>
    {
        Page GetPageBySlug(string slug);

        Page GetPageBySlug(string slug, string culture);

        void ToggleEnabled(Guid refId, bool isEnabled);

        Page GetPageByLanguage(Guid id, string cultureCode);
    }

    public class PageService : GenericDataService<Page>, IPageService
    {
        private readonly ICacheManager cacheManager;

        public PageService(IRepository<Page> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        public void ToggleEnabled(Guid refId, bool isEnabled)
        {
            Repository.Update(
                x => x.Id == refId || x.RefId == refId,
                x => new Page { IsEnabled = isEnabled });
        }

        public Page GetPageByLanguage(Guid id, string cultureCode)
        {
            return Repository.Table.FirstOrDefault(x => x.RefId == id && x.CultureCode == cultureCode);
        }

        public override int Update(Page record)
        {
            base.Update(record);

            if (record.RefId == null)
            {
                //TODO: fix bug with this not working.. for now, try catch

                try
                {
                    return Update(x => x.RefId == record.Id, x => new Page
                    {
                        Slug = record.Slug
                    });
                }
                catch
                {
                    var toUpdate = Repository.Table.Where(x => x.RefId == record.Id);

                    if (toUpdate.Any())
                    {
                        foreach (var item in toUpdate)
                        {
                            item.Slug = record.Slug;
                        }

                        return Update(toUpdate);
                    }
                }
            }
            return 0;
        }

        public Page GetPageBySlug(string slug)
        {
            return Repository.Table.FirstOrDefault(x => x.Slug == slug);
        }

        public Page GetPageBySlug(string slug, string culture)
        {
            return culture == null ?
                Repository.Table.FirstOrDefault(x => x.Slug == slug && x.CultureCode == null) :
                Repository.Table.FirstOrDefault(x => x.Slug == slug && x.CultureCode == culture);
        }
    }
}
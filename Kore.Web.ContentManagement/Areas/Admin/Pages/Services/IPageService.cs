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
        Page GetPageByLanguage(Guid id, string cultureCode);

        Page GetPageBySlug(string slug);

        Page GetPageBySlug(string slug, string culture);

        void ToggleEnabled(Guid refId, bool isEnabled);
    }

    public class PageService : GenericDataService<Page>, IPageService
    {
        public PageService(ICacheManager cacheManager, IRepository<Page> repository)
            : base(cacheManager, repository)
        {
        }

        public Page GetPageByLanguage(Guid id, string cultureCode)
        {
            return FindOne(x => x.RefId == id && x.CultureCode == cultureCode);
        }

        public Page GetPageBySlug(string slug)
        {
            return FindOne(x => x.Slug == slug);
        }

        public Page GetPageBySlug(string slug, string culture)
        {
            return culture == null ?
                FindOne(x => x.Slug == slug && x.CultureCode == null) :
                FindOne(x => x.Slug == slug && x.CultureCode == culture);
        }

        public void ToggleEnabled(Guid refId, bool isEnabled)
        {
            Update(
                x => x.Id == refId || x.RefId == refId,
                x => new Page { IsEnabled = isEnabled });
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
                catch (Exception ex)
                {
                    Logger.Error(ex.Message, ex);

                    var toUpdate = Find(x => x.RefId == record.Id);

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
    }
}
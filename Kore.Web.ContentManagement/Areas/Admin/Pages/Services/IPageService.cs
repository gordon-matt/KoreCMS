using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Services
{
    public interface IPageService : IGenericDataService<Page>
    {
    }

    public class PageService : GenericDataService<Page>, IPageService
    {
        private readonly IRepository<PageVersion> pageVersionRepository;

        public PageService(
            ICacheManager cacheManager,
            IRepository<Page> repository,
            IRepository<PageVersion> pageVersionRepository)
            : base(cacheManager, repository)
        {
            this.pageVersionRepository = pageVersionRepository;
        }

        public override int Insert(Page entity)
        {
            int rowsAffected = base.Insert(entity);

            rowsAffected += pageVersionRepository.Insert(new PageVersion
            {
                Id = Guid.NewGuid(),
                PageId = entity.Id,
                CultureCode = null,
                DateCreatedUtc = DateTime.UtcNow,
                DateModifiedUtc = DateTime.UtcNow,
                Status = VersionStatus.Draft,
                Title = entity.Name,
                Slug = entity.Name.ToSlugUrl()
            });

            return rowsAffected;
        }

        public override int Insert(IEnumerable<Page> entities)
        {
            int rowsAffected = base.Insert(entities);

            var pageVersions = entities.Select(x => new PageVersion
            {
                Id = Guid.NewGuid(),
                PageId = x.Id,
                CultureCode = null,
                DateCreatedUtc = DateTime.UtcNow,
                DateModifiedUtc = DateTime.UtcNow,
                Status = VersionStatus.Draft,
                Title = x.Name,
                Slug = x.Name.ToSlugUrl()
            });
            rowsAffected += pageVersionRepository.Insert(pageVersions);

            return rowsAffected;
        }

        //public override int Update(Page record)
        //{
        //    base.Update(record);

        //    if (record.RefId == null)
        //    {
        //        //TODO: fix bug with this not working.. for now, try catch

        //        try
        //        {
        //            return Update(x => x.RefId == record.Id, x => new Page
        //            {
        //                Slug = record.Slug
        //            });
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.Error(ex.Message, ex);

        //            var toUpdate = Find(x => x.RefId == record.Id);

        //            if (toUpdate.Any())
        //            {
        //                foreach (var item in toUpdate)
        //                {
        //                    item.Slug = record.Slug;
        //                }

        //                return Update(toUpdate);
        //            }
        //        }
        //    }
        //    return 0;
        //}
    }
}
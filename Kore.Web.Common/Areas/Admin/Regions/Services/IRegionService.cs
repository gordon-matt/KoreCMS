using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Kore.Caching;
using Kore.Collections;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.Common.Areas.Admin.Regions.Domain;

namespace Kore.Web.Common.Areas.Admin.Regions.Services
{
    public interface IRegionService : IGenericDataService<Region>
    {
        Region Get(int id, bool includeChildren = false, bool includeParent = false);

        IEnumerable<Region> GetContinents(bool includeCountries = false);

        IEnumerable<Region> GetSubRegions(int regionId, RegionType? regionType = null);

        IEnumerable<Region> GetSubRegions(int regionId, int pageIndex, int pageSize, out int total, RegionType? regionType = null);

        IEnumerable<Region> GetCountries();

        IEnumerable<Region> GetStates(int countryId, bool includeCities = false);
    }

    public class RegionService : GenericDataService<Region>, IRegionService
    {
        public RegionService(ICacheManager cacheManager, IRepository<Region> repository)
            : base(cacheManager, repository)
        {
        }

        #region IRegionService Members

        public Region Get(int id, bool includeChildren = false, bool includeParent = false)
        {
            var query = Query();

            if (includeParent)
            {
                query = query.Include(x => x.Parent);
            }
            if (includeChildren)
            {
                query = query.Include(x => x.Children);
            }

            query = query.Where(x => x.Id == id);

            return query.First();
        }

        public IEnumerable<Region> GetContinents(bool includeCountries = false)
        {
            if (includeCountries)
            {
                return Query()
                    .Include(x => x.Children)
                    .Where(x => x.RegionType == RegionType.Continent)
                    .OrderBy(x => x.Order == null)
                    .ThenBy(x => x.Order)
                    .ThenBy(x => x.Name)
                    .ToList();
            }
            return Query(x => x.RegionType == RegionType.Continent)
                .OrderBy(x => x.Order == null)
                .ThenBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToList();
        }

        public IEnumerable<Region> GetSubRegions(int regionId, RegionType? regionType = null)
        {
            if (regionType.HasValue)
            {
                return Query()
                    .Include(x => x.Parent)
                    .Include(x => x.Children)
                    .Where(x => x.Parent.Id == regionId && x.RegionType == regionType)
                    .OrderBy(x => x.Order == null)
                    .ThenBy(x => x.Order)
                    .ThenBy(x => x.Name)
                    .ToHashSet();
            }

            return Query()
                .Include(x => x.Parent)
                .Include(x => x.Children)
                .Where(x => x.Parent.Id == regionId)
                .OrderBy(x => x.Order == null)
                .ThenBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToHashSet();
        }

        public IEnumerable<Region> GetSubRegions(int regionId, int pageIndex, int pageSize, out int total, RegionType? regionType = null)
        {
            var query = Query()
                .Include(x => x.Parent)
                .Include(x => x.Children);

            if (regionType.HasValue)
            {
                query = query.Where(x => x.Parent.Id == regionId && x.RegionType == regionType);
            }
            else
            {
                query = query.Where(x => x.Parent.Id == regionId);
            }

            query = query
                .OrderBy(x => x.Order == null)
                .ThenBy(x => x.Order)
                .ThenBy(x => x.Name);

            total = query.Count();

            return query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToHashSet();
        }

        public IEnumerable<Region> GetCountries()
        {
            return Query()
                .Include(x => x.Parent)
                .Include(x => x.Children)
                .Where(x => x.RegionType == RegionType.Country)
                .OrderBy(x => x.Order == null)
                .ThenBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToHashSet();
        }

        public IEnumerable<Region> GetStates(int countryId, bool includeCities = false)
        {
            if (includeCities)
            {
                return Query()
                    .Include(x => x.Children)
                    .Where(x => x.ParentId == countryId && x.RegionType == RegionType.State)
                    .OrderBy(x => x.Order == null)
                    .ThenBy(x => x.Order)
                    .ThenBy(x => x.Name)
                    .ToHashSet();
            }

            return Query(x => x.ParentId == countryId && x.RegionType == RegionType.State)
                .OrderBy(x => x.Order == null)
                .ThenBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToHashSet();
        }

        #endregion IRegionService Members
    }
}
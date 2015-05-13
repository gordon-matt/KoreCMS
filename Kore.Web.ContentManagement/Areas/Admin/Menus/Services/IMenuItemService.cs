using System;
using System.Collections.Generic;
using System.Linq;
using Kore.Caching;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Services
{
    public interface IMenuItemService : IGenericDataService<MenuItem>
    {
        MenuItem GetMenuItemByRefId(Guid refId);

        IEnumerable<MenuItem> GetMenuItems(Guid menuId, bool enabledOnly = false);
    }

    public class MenuItemService : GenericDataService<MenuItem>, IMenuItemService
    {
        public MenuItemService(ICacheManager cacheManager, IRepository<MenuItem> repository)
            : base(cacheManager, repository)
        {
        }

        public MenuItem GetMenuItemByRefId(Guid refId)
        {
            return refId == Guid.Empty
                ? null
                : FindOne(x => x.RefId == refId);
        }

        public IEnumerable<MenuItem> GetMenuItems(Guid menuId, bool enabledOnly = false)
        {
            return CacheManager.Get(string.Format("Repository_MenuItem_GetByMenuIdAndEnabled_{0}_{1}", menuId, enabledOnly), () =>
            {
                return enabledOnly
                    ? Repository.Table
                        .Where(x => x.MenuId == menuId && x.Enabled)
                        .OrderBy(x => x.Position)
                        .ThenBy(x => x.Text)
                        .ToList()
                    : Repository.Table
                        .Where(x => x.MenuId == menuId)
                        .OrderBy(x => x.Position)
                        .ThenBy(x => x.Text)
                        .ToList();
            });
        }

        protected override void ClearCache()
        {
            base.ClearCache();
            CacheManager.RemoveByPattern("Repository_MenuItem_GetByMenuIdAndEnabled_.*");
        }
    }
}
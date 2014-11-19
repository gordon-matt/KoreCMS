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
        private readonly ICacheManager cacheManager;

        public MenuItemService(IRepository<MenuItem> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        public MenuItem GetMenuItemByRefId(Guid refId)
        {
            return refId == Guid.Empty
                ? null
                : Repository.Table.FirstOrDefault(x => x.RefId == refId);
        }

        public IEnumerable<MenuItem> GetMenuItems(Guid menuId, bool enabledOnly = false)
        {
            return cacheManager.Get("MenuItems_GetMenuItems" + menuId + "_" + enabledOnly, () =>
            {
                return enabledOnly
                    ? Repository.Table.Where(x => x.MenuId == menuId && x.Enabled).OrderBy(x => x.Position).ThenBy(x => x.Text).ToList()
                    : Repository.Table.Where(x => x.MenuId == menuId).OrderBy(x => x.Position).ThenBy(x => x.Text).ToList();
            });
        }
    }
}
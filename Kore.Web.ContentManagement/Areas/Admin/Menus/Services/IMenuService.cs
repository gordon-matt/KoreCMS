using System.Linq;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Services
{
    public interface IMenuService : IGenericDataService<Menu>
    {
        Menu FindByName(string name, string urlFilter = null);
    }

    public class MenuService : GenericDataService<Menu>, IMenuService
    {
        public MenuService(IRepository<Menu> repository)
            : base(repository)
        {
        }

        #region IMenuService Members

        public Menu FindByName(string name, string urlFilter = null)
        {
            if (string.IsNullOrEmpty(urlFilter))
            {
                return Repository.Table.FirstOrDefault(x => x.Name == name && x.UrlFilter == null);
            }

            var records = Repository.Table.Where(x => x.Name == name).ToList();
            return records.FirstOrDefault(x => x.UrlFilter.Contains(urlFilter));
        }

        #endregion IMenuService Members
    }
}
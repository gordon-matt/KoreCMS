using System.Linq;
using Kore.Data;
using Kore.Data.Services;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Services
{
    public interface IMenuService : IGenericDataService<Menu>
    {
        Menu FindByName(string name);
    }

    public class MenuService : GenericDataService<Menu>, IMenuService
    {
        public MenuService(IRepository<Menu> repository)
            : base(repository)
        {
        }

        #region IMenuService Members

        public Menu FindByName(string name)
        {
            return Repository.Table.FirstOrDefault(x => x.Name == name);
        }

        #endregion IMenuService Members
    }
}
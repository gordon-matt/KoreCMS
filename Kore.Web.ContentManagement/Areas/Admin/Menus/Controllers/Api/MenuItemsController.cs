using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class MenuItemsController : GenericODataController<MenuItem, Guid>
    {
        public MenuItemsController(IRepository<MenuItem> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(MenuItem entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(MenuItem entity)
        {
            entity.Id = Guid.NewGuid();
        }
    }
}
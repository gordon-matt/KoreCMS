using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class MenuItemApiController : GenericODataController<MenuItem, Guid>
    {
        public MenuItemApiController(IRepository<MenuItem> repository)
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
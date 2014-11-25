using System;
using System.Web.Http;
using Kore.Data;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PageTypesController : GenericODataController<PageType, Guid>
    {
        private readonly IRepository<PageType> repository;

        public PageTypesController(IRepository<PageType> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(PageType entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(PageType entity)
        {
            entity.Id = Guid.NewGuid();
        }
    }
}
using System;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers.Api
{
    public class EntityTypeContentBlockApiController : GenericODataController<EntityTypeContentBlock, Guid>
    {
        public EntityTypeContentBlockApiController(IEntityTypeContentBlockService service)
            : base(service)
        {
        }

        protected override Guid GetId(EntityTypeContentBlock entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(EntityTypeContentBlock entity)
        {
            entity.Id = Guid.NewGuid();
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.ContentBlocksRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.ContentBlocksWrite; }
        }
    }
}
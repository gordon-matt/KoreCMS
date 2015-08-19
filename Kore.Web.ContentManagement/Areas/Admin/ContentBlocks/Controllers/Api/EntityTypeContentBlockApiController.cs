using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using Kore.Infrastructure;
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

        public override IHttpActionResult Post(EntityTypeContentBlock entity)
        {
            SetValues(entity);
            return base.Post(entity);
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, EntityTypeContentBlock entity)
        {
            SetValues(entity);
            return base.Put(key, entity);
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

        private static void SetValues(EntityTypeContentBlock entity)
        {
            var blockType = Type.GetType(entity.BlockType);
            var contentBlocks = EngineContext.Current.ResolveAll<IContentBlock>();
            var contentBlock = contentBlocks.First(x => x.GetType() == blockType);
            entity.BlockName = contentBlock.Name;
        }
    }
}
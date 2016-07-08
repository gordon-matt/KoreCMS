using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Kore.Data;
using Kore.Plugins.Messaging.Forums.Data.Domain;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Plugins.Messaging.Forums.Controllers.Api
{
    public class ForumApiController : GenericODataController<Forum, int>
    {
        public ForumApiController(IRepository<Forum> repository)
            : base(repository)
        {
        }

        protected override int GetId(Forum entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Forum entity)
        {
        }

        public override async Task<IHttpActionResult> Post(Forum entity)
        {
            entity.CreatedOnUtc = DateTime.UtcNow;
            entity.UpdatedOnUtc = DateTime.UtcNow;
            return await base.Post(entity);
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] int key, Forum entity)
        {
            // Client does not send all fields, so we get existing and update only the fields that are sent from the client...
            var originalEntity = await Service.FindOneAsync(key);
            originalEntity.Name = entity.Name;
            originalEntity.Description = entity.Description;
            originalEntity.DisplayOrder = entity.DisplayOrder;

            // ... and we set this too.
            originalEntity.UpdatedOnUtc = DateTime.UtcNow;
            return await base.Put(key, originalEntity);
        }

        protected override Permission ReadPermission
        {
            get { return ForumPermissions.ReadForums; }
        }

        protected override Permission WritePermission
        {
            get { return ForumPermissions.WriteForums; }
        }
    }
}
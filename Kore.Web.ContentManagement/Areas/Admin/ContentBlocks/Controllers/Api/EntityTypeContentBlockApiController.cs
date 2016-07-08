﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Kore.Infrastructure;
using Kore.Localization.Domain;
using Kore.Localization.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers.Api
{
    public class EntityTypeContentBlockApiController : GenericODataController<EntityTypeContentBlock, Guid>
    {
        private readonly Lazy<ILocalizablePropertyService> localizablePropertyService;

        public EntityTypeContentBlockApiController(
            IEntityTypeContentBlockService service,
            Lazy<ILocalizablePropertyService> localizablePropertyService)
            : base(service)
        {
            this.localizablePropertyService = localizablePropertyService;
        }

        public override async Task<IHttpActionResult> Post(EntityTypeContentBlock entity)
        {
            SetValues(entity);
            return await base.Post(entity);
        }

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, EntityTypeContentBlock entity)
        {
            SetValues(entity);
            return await base.Put(key, entity);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetLocalized([FromODataUri] Guid id, [FromODataUri] string cultureCode)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Unauthorized();
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var entity = await Service.FindOneAsync(id);

            if (entity == null)
            {
                return NotFound();
            }

            string entityType = typeof(EntityTypeContentBlock).FullName;
            string entityId = entity.Id.ToString();

            var localizedRecord = await localizablePropertyService.Value.FindOneAsync(x =>
                x.CultureCode == cultureCode &&
                x.EntityType == entityType &&
                x.EntityId == entityId &&
                x.Property == "BlockValues");

            if (localizedRecord != null)
            {
                entity.BlockValues = localizedRecord.Value;
            }

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveLocalized(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string cultureCode = (string)parameters["cultureCode"];
            var entity = (EntityTypeContentBlock)parameters["entity"];

            if (entity.Id == Guid.Empty)
            {
                return BadRequest();
            }
            string entityType = typeof(EntityTypeContentBlock).FullName;
            string entityId = entity.Id.ToString();

            var localizedRecord = await localizablePropertyService.Value.FindOneAsync(x =>
                x.CultureCode == cultureCode &&
                x.EntityType == entityType &&
                x.EntityId == entityId &&
                x.Property == "BlockValues");

            if (localizedRecord == null)
            {
                localizedRecord = new LocalizableProperty
                {
                    CultureCode = cultureCode,
                    EntityType = entityType,
                    EntityId = entityId,
                    Property = "BlockValues",
                    Value = entity.BlockValues
                };
                await localizablePropertyService.Value.InsertAsync(localizedRecord);
                return Ok();
            }
            else
            {
                localizedRecord.Value = entity.BlockValues;
                await localizablePropertyService.Value.UpdateAsync(localizedRecord);
                return Ok();
            }
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
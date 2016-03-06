using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Localization.Domain;
using Kore.Localization.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Domain;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class ContentBlockApiController : GenericODataController<ContentBlock, Guid>
    {
        private readonly Lazy<ILocalizablePropertyService> localizablePropertyService;

        public ContentBlockApiController(
            IContentBlockService service,
            Lazy<ILocalizablePropertyService> localizablePropertyService)
            : base(service)
        {
            this.localizablePropertyService = localizablePropertyService;
        }

        protected override Guid GetId(ContentBlock entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(ContentBlock entity)
        {
            entity.Id = Guid.NewGuid();
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IEnumerable<ContentBlock> Get(ODataQueryOptions<ContentBlock> options)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<ContentBlock>().AsQueryable();
            }

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            using (var connection = Service.OpenConnection())
            {
                var results = options.ApplyTo(connection.Query(x => x.PageId == null));
                return (results as IQueryable<ContentBlock>).ToHashSet();
            }
        }

        //[EnableQuery]
        [HttpPost]
        public virtual IEnumerable<ContentBlock> GetByPageId(ODataQueryOptions<ContentBlock> options, ODataActionParameters parameters)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<ContentBlock>().AsQueryable();
            }

            var pageId = (Guid)parameters["pageId"];

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            using (var connection = Service.OpenConnection())
            {
                var query = connection
                    .Query(x => x.PageId == pageId)
                    .OrderBy(x => x.ZoneId)
                    .ThenBy(x => x.Order);

                var results = options.ApplyTo(query);
                return results as IQueryable<ContentBlock>;
            }
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, ContentBlock entity)
        {
            var blockType = Type.GetType(entity.BlockType);
            var contentBlocks = EngineContext.Current.ResolveAll<IContentBlock>();
            var contentBlock = contentBlocks.First(x => x.GetType() == blockType);
            entity.BlockName = contentBlock.Name;
            return base.Put(key, entity);
        }

        public override IHttpActionResult Post(ContentBlock entity)
        {
            var blockType = Type.GetType(entity.BlockType);
            var contentBlocks = EngineContext.Current.ResolveAll<IContentBlock>();
            var contentBlock = contentBlocks.First(x => x.GetType() == blockType);
            entity.BlockName = contentBlock.Name;
            return base.Post(entity);
        }

        [HttpGet]
        public IHttpActionResult GetLocalized([FromODataUri] Guid id, [FromODataUri] string cultureCode)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Unauthorized();
            }

            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var entity = Service.FindOne(id);

            if (entity == null)
            {
                return NotFound();
            }

            string entityType = typeof(ContentBlock).FullName;
            string entityId = entity.Id.ToString();

            var localizedRecord = localizablePropertyService.Value.FindOne(x =>
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
        public IHttpActionResult SaveLocalized(ODataActionParameters parameters)
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
            var entity = (ContentBlock)parameters["entity"];

            if (entity.Id == Guid.Empty)
            {
                return BadRequest();
            }
            string entityType = typeof(ContentBlock).FullName;
            string entityId = entity.Id.ToString();

            var localizedRecord = localizablePropertyService.Value.FindOne(x =>
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
                localizablePropertyService.Value.Insert(localizedRecord);
                return Ok();
            }
            else
            {
                localizedRecord.Value = entity.BlockValues;
                localizablePropertyService.Value.Update(localizedRecord);
                return Ok();
            }
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
using System;
using System.Linq;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
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

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<ContentBlock> Get()
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<ContentBlock>().AsQueryable();
            }

            return Service.Repository.Table.Where(x => x.PageId == null);
        }

        [EnableQuery]
        [HttpPost]
        public virtual IQueryable<ContentBlock> GetByPageId(ODataActionParameters parameters)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<ContentBlock>().AsQueryable();
            }

            var pageId = (Guid)parameters["pageId"];

            return Service.Repository.Table
                .Where(x => x.PageId == pageId)
                .OrderBy(x => x.ZoneId)
                .ThenBy(x => x.Order);
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

        //private void Save(ContentBlock entity)
        //{
        //    if (entity.Id == Guid.Empty)
        //    {
        //        SetNewId(entity);
        //        var blockType = Type.GetType(entity.BlockType);
        //        var contentBlocks = EngineContext.Current.ResolveAll<IContentBlock>();
        //        var contentBlock = contentBlocks.First(x => x.GetType() == blockType);
        //        entity.BlockName = contentBlock.Name;
        //        Service.Insert(entity);
        //        return;
        //    }

        //    var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
        //    if (!string.IsNullOrEmpty(workContext.CurrentCultureCode))
        //    {
        //        // Insert or Update Localized Block Values
        //        string entityType = entity.BlockType;
        //        string entityId = entity.Id.ToString();

        //        var localizedRecord = localizablePropertyService.Value.FindOne(x =>
        //            x.CultureCode == workContext.CurrentCultureCode &&
        //            x.EntityType == entityType &&
        //            x.EntityId == entityId &&
        //            x.Property == "BlockValues");

        //        if (localizedRecord == null)
        //        {
        //            localizedRecord = new LocalizableProperty
        //            {
        //                CultureCode = workContext.CurrentCultureCode,
        //                EntityType = entityType,
        //                EntityId = entityId,
        //                Property = "BlockValues",
        //                Value = entity.BlockValues
        //            };
        //            localizablePropertyService.Value.Insert(localizedRecord);
        //        }
        //        else
        //        {
        //            localizedRecord.Value = entity.BlockValues;
        //            localizablePropertyService.Value.Update(localizedRecord);
        //        }
        //    }
        //    else
        //    {
        //        // Update Invariant Values
        //        Service.Update(entity);
        //    }
        //}

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
using System;
using System.Web.Http;
using System.Web.OData;
using Kore.Localization.Domain;
using Kore.Localization.Services;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Common.Areas.Admin.Regions.Controllers.Api
{
    public class RegionApiController : GenericODataController<Region, int>
    {
        private readonly Lazy<ILocalizablePropertyService> localizablePropertyService;

        public RegionApiController(
            IRegionService service,
            Lazy<ILocalizablePropertyService> localizablePropertyService)
            : base(service)
        {
            this.localizablePropertyService = localizablePropertyService;
        }

        protected override int GetId(Region entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Region entity)
        {
        }

        [HttpGet]
        public IHttpActionResult GetLocalized([FromODataUri] int id, [FromODataUri] string cultureCode)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Unauthorized();
            }

            if (id == 0)
            {
                return BadRequest();
            }

            var entity = Service.FindOne(id);

            if (entity == null)
            {
                return NotFound();
            }

            string entityType = typeof(Region).FullName;
            string entityId = entity.Id.ToString();

            var localizedRecord = localizablePropertyService.Value.FindOne(cultureCode, entityType, entityId, "Name");
            if (localizedRecord != null)
            {
                entity.Name = localizedRecord.Value;
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
            var entity = (Region)parameters["entity"];

            if (entity.Id == 0)
            {
                return BadRequest();
            }
            string entityType = typeof(Region).FullName;
            string entityId = entity.Id.ToString();

            var localizedRecord = localizablePropertyService.Value.FindOne(cultureCode, entityType, entityId, "Name");
            if (localizedRecord == null)
            {
                localizedRecord = new LocalizableProperty
                {
                    CultureCode = cultureCode,
                    EntityType = entityType,
                    EntityId = entityId,
                    Property = "Name",
                    Value = entity.Name
                };
                localizablePropertyService.Value.Insert(localizedRecord);
                return Ok();
            }
            else
            {
                localizedRecord.Value = entity.Name;
                localizablePropertyService.Value.Update(localizedRecord);
                return Ok();
            }
        }

        protected override Permission ReadPermission
        {
            get { return Permissions.RegionsRead; }
        }

        protected override Permission WritePermission
        {
            get { return Permissions.RegionsWrite; }
        }
    }
}
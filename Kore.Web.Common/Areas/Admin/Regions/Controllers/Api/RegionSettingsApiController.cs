using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.Http.Results;
using Kore.Infrastructure;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Common.Areas.Admin.Regions.Controllers.Api
{
    public class RegionSettingsApiController : ODataController
    {
        private readonly IEnumerable<IRegionSettings> regionSettings;
        private readonly Lazy<IRegionSettingsService> regionSettingsService;

        public RegionSettingsApiController(
            IEnumerable<IRegionSettings> regionSettings,
            Lazy<IRegionSettingsService> regionSettingsService)
        {
            this.regionSettings = regionSettings;
            this.regionSettingsService = regionSettingsService;
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<EdmRegionSettings> Get()
        {
            if (!CheckPermission(Permissions.RegionsRead))
            {
                return Enumerable.Empty<EdmRegionSettings>().AsQueryable();
            }
            return regionSettings
                .Select(x => new EdmRegionSettings
                {
                    Id = x.Name.ToSlugUrl(),
                    Name = x.Name
                })
                .AsQueryable();
        }

        [HttpPost]
        public virtual EdmRegionSettings GetSettings(ODataActionParameters parameters)
        {
            if (!CheckPermission(Permissions.RegionsRead))
            {
                return null;
            }

            string settingsId = (string)parameters["settingsId"];
            int regionId = (int)parameters["regionId"];

            var dictionary = regionSettings.ToDictionary(k => k.Name.ToSlugUrl(), v => v);

            if (!dictionary.ContainsKey(settingsId))
            {
                return null;
            }

            var settings = dictionary[settingsId];

            var dataEntity = regionSettingsService.Value.FindOne(x =>
                x.RegionId == regionId &&
                x.SettingsId == settingsId);

            if (dataEntity != null)
            {
                return new EdmRegionSettings
                {
                    Id = settingsId,
                    Name = settings.Name,
                    Fields = dataEntity.Fields
                };
            }

            return new EdmRegionSettings
            {
                Id = settingsId,
                Name = settings.Name,
                Fields = null
            };
        }

        [HttpPost]
        public virtual IHttpActionResult SaveSettings(ODataActionParameters parameters)
        {
            if (!CheckPermission(Permissions.RegionsWrite))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            string settingsId = (string)parameters["settingsId"];
            int regionId = (int)parameters["regionId"];
            string fields = (string)parameters["fields"];

            if (string.IsNullOrEmpty(settingsId))
            {
                return this.BadRequest("SettingsId has not been provided");
            }
            if (regionId == 0)
            {
                return this.BadRequest("RegionId has not been provided.");
            }

            var allSettingsIds = regionSettings.Select(x => x.Name.ToSlugUrl());

            if (!allSettingsIds.Contains(settingsId))
            {
                return this.BadRequest(string.Format("SettingsId, '{0}' is not recognized.", settingsId));
            }

            var dataEntity = regionSettingsService.Value.FindOne(x =>
                x.RegionId == regionId &&
                x.SettingsId == settingsId);

            if (dataEntity == null)
            {
                dataEntity = new RegionSettings
                {
                    SettingsId = settingsId,
                    RegionId = regionId,
                    Fields = fields
                };
                regionSettingsService.Value.Insert(dataEntity);
                return Ok();
                //TODO (currently throws error because of missing entity set on odata, basically we need to create
                //      the separate Api Controller for it. A good idea might be to make this controller do that and make the
                //      current Get() method an OData action instead... and maybe call it GetSettingsTypes())
                //return Created(dataEntity);
            }
            else
            {
                dataEntity.Fields = fields;
                regionSettingsService.Value.Update(dataEntity);
                return Ok();
                //TODO (currently throws error because of missing entity set on odata, basically we need to create
                //      the separate Api Controller for it. A good idea might be to make this controller do that and make the
                //      current Get() method an OData action instead... and maybe call it GetSettingsTypes())
                //return Updated(dataEntity);
            }
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }

    public class EdmRegionSettings
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Fields { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Kore.Caching;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Configuration.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class SettingsApiController : GenericODataController<Setting, Guid>
    {
        private readonly ICacheManager cacheManager;

        public SettingsApiController(IRepository<Setting> repository, ICacheManager cacheManager)
            : base(repository)
        {
            this.cacheManager = cacheManager;
        }

        protected override Guid GetId(Setting entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Setting entity)
        {
            entity.Id = Guid.NewGuid();
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IEnumerable<Setting> Get(ODataQueryOptions<Setting> options)
        {
            return base.Get(options);
        }

        public override IHttpActionResult Delete([FromODataUri] Guid key)
        {
            var result = base.Delete(key);

            var entity = Service.FindOne(key);
            string cacheKey = string.Format("Kore_Web_Settings_{0}", entity.Type);
            cacheManager.Remove(cacheKey);

            return result;
        }

        public override IHttpActionResult Post(Setting entity)
        {
            var result = base.Post(entity);

            string cacheKey = string.Format("Kore_Web_Settings_{0}", entity.Type);
            cacheManager.Remove(cacheKey);

            return result;
        }

        public override IHttpActionResult Put([FromODataUri] Guid key, Setting entity)
        {
            var result = base.Put(key, entity);

            string cacheKey = string.Format("Kore_Web_Settings_{0}", entity.Type);
            cacheManager.Remove(cacheKey);

            return result;
        }

        public override IHttpActionResult Patch([FromODataUri] Guid key, Delta<Setting> patch)
        {
            var result = base.Patch(key, patch);

            var entity = Service.FindOne(key);
            string cacheKey = string.Format("Kore_Web_Settings_{0}", entity.Type);
            cacheManager.Remove(cacheKey);

            return result;
        }

        protected override Permission ReadPermission
        {
            get { return ConfigurationPermissions.ReadSettings; }
        }

        protected override Permission WritePermission
        {
            get { return ConfigurationPermissions.WriteSettings; }
        }
    }
}
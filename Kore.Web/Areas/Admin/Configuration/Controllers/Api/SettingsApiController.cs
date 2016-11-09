﻿using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using Kore.Caching;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Web.Configuration;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Configuration.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class SettingsApiController : GenericTenantODataController<Setting, Guid>
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

        public override async Task<IHttpActionResult> Put([FromODataUri] Guid key, Setting entity)
        {
            var result = await base.Put(key, entity);

            string cacheKey = string.Format(KoreWebConstants.CacheKeys.SettingsKeyFormat, entity.TenantId, entity.Type);
            cacheManager.Remove(cacheKey);

            // TODO: This is an ugly hack. We need to have a way for each setting to perform some tasks after update
            if (entity.Name == new KoreSiteSettings().Name)
            {
                cacheManager.Remove(KoreWebConstants.CacheKeys.CurrentCulture);
            }

            return result;
        }

        public override async Task<IHttpActionResult> Post(Setting entity)
        {
            var result = await base.Post(entity);

            string cacheKey = string.Format(KoreWebConstants.CacheKeys.SettingsKeyFormat, entity.TenantId, entity.Type);
            cacheManager.Remove(cacheKey);

            return result;
        }

        public override async Task<IHttpActionResult> Patch([FromODataUri] Guid key, Delta<Setting> patch)
        {
            var result = await base.Patch(key, patch);

            var entity = await Service.FindOneAsync(key);
            string cacheKey = string.Format(KoreWebConstants.CacheKeys.SettingsKeyFormat, entity.TenantId, entity.Type);
            cacheManager.Remove(cacheKey);

            return result;
        }

        public override async Task<IHttpActionResult> Delete([FromODataUri] Guid key)
        {
            var result = base.Delete(key);

            var entity = await Service.FindOneAsync(key);
            string cacheKey = string.Format(KoreWebConstants.CacheKeys.SettingsKeyFormat, entity.TenantId, entity.Type);
            cacheManager.Remove(cacheKey);

            return await result;
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
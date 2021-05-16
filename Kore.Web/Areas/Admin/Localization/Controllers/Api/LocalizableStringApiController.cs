using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Kore.Caching;
using Kore.EntityFramework.Data;
using Kore.Localization.Domain;
using Kore.Localization.Models;
using Kore.Localization.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;

namespace Kore.Web.Areas.Admin.Localization.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class LocalizableStringApiController : GenericTenantODataController<LocalizableString, Guid>
    {
        private readonly ICacheManager cacheManager;

        public LocalizableStringApiController(
            ILocalizableStringService service,
            ICacheManager cacheManager)
            : base(service)
        {
            this.cacheManager = cacheManager;
        }

        protected override Guid GetId(LocalizableString entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(LocalizableString entity)
        {
            entity.Id = Guid.NewGuid();
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        public virtual async Task<IHttpActionResult> GetComparitiveTable([FromODataUri] string cultureCode, ODataQueryOptions<ComparitiveLocalizableString> options)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Unauthorized();
            }
            else
            {
                int tenantId = GetTenantId();
                var connection = GetDisposableConnection();

                // With grouping, we use .Where() and then .FirstOrDefault() instead of just the .FirstOrDefault() by itself
                //  for compatibility with MySQL.
                //  See: http://stackoverflow.com/questions/23480044/entity-framework-select-statement-with-logic
                var query = connection.Query(x => x.TenantId == tenantId && (x.CultureCode == null || x.CultureCode == cultureCode))
                        .GroupBy(x => x.TextKey)
                        .Select(grp => new ComparitiveLocalizableString
                        {
                            Key = grp.Key,
                            InvariantValue = grp.Where(x => x.CultureCode == null).FirstOrDefault().TextValue,
                            LocalizedValue = grp.Where(x => x.CultureCode == cultureCode).FirstOrDefault() == null
                                ? string.Empty
                                : grp.Where(x => x.CultureCode == cultureCode).FirstOrDefault().TextValue
                        });

                var results = await (options.ApplyTo(query) as IQueryable<ComparitiveLocalizableString>).ToHashSetAsync();
                return Ok(results, results.GetType());
            }
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> PutComparitive(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            string cultureCode = (string)parameters["cultureCode"];
            string key = (string)parameters["key"];
            var entity = (ComparitiveLocalizableString)parameters["entity"];

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!key.Equals(entity.Key))
            {
                return BadRequest();
            }

            int tenantId = GetTenantId();
            var localizedString = await Service.FindOneAsync(x => x.TenantId == tenantId && x.CultureCode == cultureCode && x.TextKey == key);

            if (localizedString == null)
            {
                localizedString = new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    TenantId = tenantId,
                    CultureCode = cultureCode,
                    TextKey = key,
                    TextValue = entity.LocalizedValue
                };
                await Service.InsertAsync(localizedString);
            }
            else
            {
                localizedString.TextValue = entity.LocalizedValue;
                await Service.UpdateAsync(localizedString);
            }

            cacheManager.Remove(string.Concat(KoreConstants.CacheKeys.LocalizableStringsFormat, tenantId, cultureCode));

            return Updated(entity);
        }

        [HttpPost]
        public virtual async Task<IHttpActionResult> DeleteComparitive(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            string cultureCode = (string)parameters["cultureCode"];
            string key = (string)parameters["key"];

            int tenantId = GetTenantId();
            var entity = await Service.FindOneAsync(x => x.TenantId == tenantId && x.CultureCode == cultureCode && x.TextKey == key);
            if (entity == null)
            {
                return NotFound();
            }

            entity.TextValue = null;
            await Service.UpdateAsync(entity);
            //Repository.Delete(entity);

            cacheManager.Remove(string.Concat(KoreConstants.CacheKeys.LocalizableStringsFormat, tenantId, cultureCode));

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Permission ReadPermission
        {
            get { return KoreWebPermissions.LocalizableStringsRead; }
        }

        protected override Permission WritePermission
        {
            get { return KoreWebPermissions.LocalizableStringsWrite; }
        }
    }
}
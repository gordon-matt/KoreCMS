using System;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.OData;
using System.Web.OData.Query;
using Kore.Caching;
using Kore.Localization.Domain;
using Kore.Localization.Models;
using Kore.Localization.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class LocalizableStringApiController : GenericODataController<LocalizableString, Guid>
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

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        public virtual IHttpActionResult GetComparitiveTable([FromODataUri] string cultureCode)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Unauthorized();
            }
            else
            {
                var query = Service.Query(x => x.CultureCode == null || x.CultureCode == cultureCode)
                        .GroupBy(x => x.TextKey)
                        .Select(grp => new ComparitiveLocalizableString
                        {
                            Key = grp.Key,
                            InvariantValue = grp.Where(x => x.CultureCode == null).FirstOrDefault().TextValue,
                            LocalizedValue = grp.Where(x => x.CultureCode == cultureCode).FirstOrDefault() == null
                                ? string.Empty
                                : grp.Where(x => x.CultureCode == cultureCode).FirstOrDefault().TextValue
                        });

                // Below doesn't work with MySQL.
                // See: http://stackoverflow.com/questions/23480044/entity-framework-select-statement-with-logic
                //var query = Service.Repository.Table
                //        .Where(x => x.CultureCode == null || x.CultureCode == cultureCode)
                //        .GroupBy(x => x.TextKey)
                //        .Select(grp => new ComparitiveLocalizableString
                //        {
                //            Key = grp.Key,
                //            InvariantValue = grp.FirstOrDefault(x => x.CultureCode == null).TextValue,
                //            LocalizedValue = grp.FirstOrDefault(x => x.CultureCode == cultureCode) == null
                //                ? string.Empty
                //                : grp.FirstOrDefault(x => x.CultureCode == cultureCode).TextValue
                //        });

                return Ok(query);
            }
        }

        [HttpPost]
        public virtual IHttpActionResult PutComparitive(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
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

            var localizedString = Service.FindOne(x => x.CultureCode == cultureCode && x.TextKey == key);

            if (localizedString == null)
            {
                localizedString = new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    CultureCode = cultureCode,
                    TextKey = key,
                    TextValue = entity.LocalizedValue
                };
                Service.Insert(localizedString);
            }
            else
            {
                localizedString.TextValue = entity.LocalizedValue;
                Service.Update(localizedString);
            }

            cacheManager.Remove(string.Concat("Kore_LocalizableStrings_", cultureCode));

            return Updated(entity);
        }

        [HttpPost]
        public virtual IHttpActionResult DeleteComparitive(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            string cultureCode = (string)parameters["cultureCode"];
            string key = (string)parameters["key"];

            var entity = Service.FindOne(x => x.CultureCode == cultureCode && x.TextKey == key);
            if (entity == null)
            {
                return NotFound();
            }

            entity.TextValue = null;
            Service.Update(entity);
            //Repository.Delete(entity);

            cacheManager.Remove(string.Concat("Kore_LocalizableStrings_", cultureCode));

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override Permission ReadPermission
        {
            get { return CmsPermissions.LocalizableStringsRead; }
        }

        protected override Permission WritePermission
        {
            get { return CmsPermissions.LocalizableStringsWrite; }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.Results;
using Kore.Caching;
using Kore.Collections;
using Kore.Data;
using Kore.Localization.Domain;
using Kore.Localization.Models;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class LocalizableStringApiController : GenericODataController<LocalizableString, Guid>
    {
        private readonly ICacheManager cacheManager;

        public LocalizableStringApiController(
            IRepository<LocalizableString> repository,
            ICacheManager cacheManager)
            : base(repository)
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

        [EnableQuery]
        [HttpPost]
        public virtual IEnumerable<ComparitiveLocalizableString> GetComparitiveTable(ODataActionParameters parameters)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<ComparitiveLocalizableString>();
            }

            string cultureCode = (string)parameters["cultureCode"];

            var query = Repository.Table
                    .Where(x => x.CultureCode == null || x.CultureCode == cultureCode)
                    .GroupBy(x => x.TextKey)
                    .Select(grp => new ComparitiveLocalizableString
                    {
                        Key = grp.Key,
                        InvariantValue = grp.FirstOrDefault(x => x.CultureCode == null).TextValue,
                        LocalizedValue = grp.FirstOrDefault(x => x.CultureCode == cultureCode) == null ? "" : grp.FirstOrDefault(x => x.CultureCode == cultureCode).TextValue
                    });

            // Since OData v3 doesn't seem to allow filter queries on action methods yet,
            //  we'll just have to make do with sending all the data to client and filtering & sorting there for this special case
            return query.ToHashSet();
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

            var localizedString = Repository.Table.FirstOrDefault(x => x.CultureCode == cultureCode && x.TextKey == key);

            if (localizedString == null)
            {
                localizedString = new LocalizableString
                {
                    Id = Guid.NewGuid(),
                    CultureCode = cultureCode,
                    TextKey = key,
                    TextValue = entity.LocalizedValue
                };
                Repository.Insert(localizedString);
            }
            else
            {
                localizedString.TextValue = entity.LocalizedValue;
                Repository.Update(localizedString);
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

            var entity = Repository.Table.FirstOrDefault(x => x.CultureCode == cultureCode && x.TextKey == key);
            if (entity == null)
            {
                return NotFound();
            }

            entity.TextValue = null;
            Repository.Update(entity);
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
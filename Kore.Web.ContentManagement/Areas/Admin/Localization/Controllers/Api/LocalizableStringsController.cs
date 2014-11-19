using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.OData;
using Kore.Collections;
using Kore.Data;
using Kore.Localization.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Localization.Models;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers.Api
{
    public class LocalizableStringsController : GenericODataController<LocalizableString, Guid>
    {
        public LocalizableStringsController(IRepository<LocalizableString> repository)
            : base(repository)
        {
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

            return Updated(entity);
        }

        [HttpPost]
        public virtual IHttpActionResult DeleteComparitive(ODataActionParameters parameters)
        {
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

            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
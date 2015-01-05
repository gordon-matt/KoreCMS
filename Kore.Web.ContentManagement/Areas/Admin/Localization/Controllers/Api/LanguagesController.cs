using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Kore.Data;
using Kore.Localization.Domain;
using Kore.Web.Http.OData;

namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Controllers.Api
{
    [Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class LanguagesController : GenericODataController<Language, Guid>
    {
        public LanguagesController(IRepository<Language> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(Language entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Language entity)
        {
            entity.Id = Guid.NewGuid();
        }
    }
}
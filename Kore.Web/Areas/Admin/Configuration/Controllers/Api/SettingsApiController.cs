using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Configuration.Domain;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Configuration;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Configuration.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class SettingsApiController : GenericODataController<Setting, Guid>
    {
        public SettingsApiController(IRepository<Setting> repository)
            : base(repository)
        {
        }

        protected override Guid GetId(Setting entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(Setting entity)
        {
            entity.Id = Guid.NewGuid();
        }

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public override IQueryable<Setting> Get()
        {
            return base.Get();
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
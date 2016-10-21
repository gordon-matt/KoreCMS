using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.OData;
using System.Web.OData.Query;
using Kore.Collections;
using Kore.Security.Membership;

namespace Kore.Web.Areas.Admin.Membership.Controllers.Api
{
    public class PublicUserApiController : ODataController
    {
        private readonly IWorkContext workContext;

        protected IMembershipService Service { get; private set; }

        public PublicUserApiController(IMembershipService service, IWorkContext workContext)
        {
            this.Service = service;
            this.workContext = workContext;
        }

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IEnumerable<PublicUserInfo> Get(ODataQueryOptions<PublicUserInfo> options)
        {
            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var query = Service.GetAllUsersAsQueryable(workContext.CurrentTenant.Id)
                .Select(x => new PublicUserInfo
                {
                    Id = x.Id,
                    UserName = x.UserName
                });

            var results = options.ApplyTo(query);
            return (results as IQueryable<PublicUserInfo>).ToHashSet();
        }

        public virtual async Task<PublicUserInfo> Get([FromODataUri] string key)
        {
            var entity = await Service.GetUserById(key);
            return new PublicUserInfo
            {
                Id = entity.Id,
                UserName = entity.UserName
            };
        }
    }

    public class PublicUserInfo
    {
        public string Id { get; set; }

        public string UserName { get; set; }
    }
}
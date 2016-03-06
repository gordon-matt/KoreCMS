using System.Collections.Generic;
using System.Linq;
using System.Web.OData;
using System.Web.OData.Query;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Web.Areas.Admin.Plugins.Models;
using Kore.Web.Plugins;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Areas.Admin.Plugins.Controllers.Api
{
    //[Authorize(Roles = KoreConstants.Roles.Administrators)]
    public class PluginApiController : ODataController
    {
        private readonly IPluginFinder pluginFinder;

        public PluginApiController(IPluginFinder pluginFinder)
        {
            this.pluginFinder = pluginFinder;
        }

        // GET: odata/kore/cms/Plugins
        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IEnumerable<EdmPluginDescriptor> Get(ODataQueryOptions<EdmPluginDescriptor> options)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<EdmPluginDescriptor>().AsQueryable();
            }

            var settings = new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            };
            options.Validate(settings);

            var query = pluginFinder.GetPluginDescriptors(false).Select(x => (EdmPluginDescriptor)x).AsQueryable();
            var results = options.ApplyTo(query);
            return (results as IQueryable<EdmPluginDescriptor>).ToHashSet();
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }
}
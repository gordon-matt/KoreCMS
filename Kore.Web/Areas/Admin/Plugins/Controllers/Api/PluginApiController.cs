using System;
using System.Linq;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
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
        private readonly Lazy<IWebHelper> webHelper;

        public PluginApiController(IPluginFinder pluginFinder, Lazy<IWebHelper> webHelper)
        {
            this.pluginFinder = pluginFinder;
            this.webHelper = webHelper;
        }

        // GET: odata/kore/cms/Plugins
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        public virtual IQueryable<EdmPluginDescriptor> Get()
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Enumerable.Empty<EdmPluginDescriptor>().AsQueryable();
            }

            return pluginFinder.GetPluginDescriptors(false).Select(x => (EdmPluginDescriptor)x).AsQueryable();
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }
}
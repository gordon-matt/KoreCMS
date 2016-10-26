using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Query;
using Castle.Core.Logging;
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
        private readonly Lazy<ILogger> logger;

        public PluginApiController(IPluginFinder pluginFinder, Lazy<ILogger> logger)
        {
            this.pluginFinder = pluginFinder;
            this.logger = logger;
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

        [EnableQuery]
        public virtual SingleResult<EdmPluginDescriptor> Get([FromODataUri] string key)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return SingleResult.Create(Enumerable.Empty<EdmPluginDescriptor>().AsQueryable());
            }

            string systemName = key.Replace('-', '.');
            var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName(systemName, false);
            EdmPluginDescriptor entity = pluginDescriptor;
            return SingleResult.Create(new[] { entity }.AsQueryable());
        }

        public virtual IHttpActionResult Put([FromODataUri] string key, EdmPluginDescriptor entity)
        {
            if (!CheckPermission(StandardPermissions.FullAccess))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string systemName = key.Replace('-', '.');
                var pluginDescriptor = pluginFinder.GetPluginDescriptorBySystemName(systemName, false);

                if (pluginDescriptor == null)
                {
                    return NotFound();
                }

                pluginDescriptor.FriendlyName = entity.FriendlyName;
                pluginDescriptor.DisplayOrder = entity.DisplayOrder;
                pluginDescriptor.LimitedToTenants.Clear();
                if (!entity.LimitedToTenants.IsNullOrEmpty())
                {
                    pluginDescriptor.LimitedToTenants = entity.LimitedToTenants.ToList();
                }
                PluginFileParser.SavePluginDescriptionFile(pluginDescriptor);
            }
            catch (Exception x)
            {
                logger.Value.Error(x.Message, x);
            }

            return Updated(entity);
        }

        protected static bool CheckPermission(Permission permission)
        {
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
        }
    }
}
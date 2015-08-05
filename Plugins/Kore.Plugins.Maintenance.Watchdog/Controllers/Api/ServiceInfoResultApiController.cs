//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Web.Http.OData;
//using System.Web.Http.OData.Query;
//using Kore.Collections;
//using Kore.Infrastructure;
//using Kore.Plugins.Maintenance.Watchdog.Models;
//using Kore.Plugins.Maintenance.Watchdog.Services;
//using Kore.Web.Security.Membership.Permissions;
//using Newtonsoft.Json;

//namespace Kore.Plugins.Maintenance.Watchdog.Controllers.Api
//{
//    public class ServiceInfoResultApiController : ODataController
//    {
//        private readonly IWatchdogInstanceService service;
//        private readonly WatchdogSettings settings;

//        public ServiceInfoResultApiController(IWatchdogInstanceService service, WatchdogSettings settings)
//        {
//            this.settings = settings;
//        }

//        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
//        public IQueryable<ServiceInfoResult> Get([FromODataUri] int watchdogInstanceId)
//        {
//            if (!CheckPermission(WatchdogPermissions.Read))
//            {
//                return Enumerable.Empty<ServiceInfoResult>().AsQueryable();
//            }

//            var instance = service.FindOne(watchdogInstanceId);

//            using (var client = new HttpClient())
//            {
//                var response = client.GetAsync(instance.Url.TrimEnd('/') + "/api/WatchdogApi/GetWatchedServices?password=" + instance.Password).Result;
//                var content = response.Content.ReadAsStringAsync().Result;
//                var watchedServices = JsonConvert.DeserializeObject<List<ServiceInfoResult>>(content);

//                if (settings.OnlyShowWatched)
//                {
//                    var items = watchedServices.Select(x => new ServiceInfoResult
//                    {
//                        WatchdogInstanceId = watchdogInstanceId,
//                        ServiceName = x.ServiceName,
//                        DisplayName = x.DisplayName,
//                        Status = x.Status,
//                        IsWatched = true
//                    });

//                    return items.AsQueryable();
//                }
//                else
//                {
//                    response = client.GetAsync(instance.Url.TrimEnd('/') + "/api/WatchdogApi/GetServices?password=" + instance.Password).Result;
//                    content = response.Content.ReadAsStringAsync().Result;
//                    var allServices = JsonConvert.DeserializeObject<List<ServiceInfoResult>>(content);

//                    var watchedServiceNames = watchedServices.Select(x => x.ServiceName).ToHashSet();

//                    var items = allServices.Select(x => new ServiceInfoResult
//                    {
//                        WatchdogInstanceId = watchdogInstanceId,
//                        ServiceName = x.ServiceName,
//                        DisplayName = x.DisplayName,
//                        Status = x.Status,
//                        IsWatched = watchedServiceNames.Contains(x.ServiceName)
//                    });

//                    return items.AsQueryable();
//                }
//            }
//        }

//        protected static bool CheckPermission(Permission permission)
//        {
//            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
//            var workContext = EngineContext.Current.Resolve<IWorkContext>();
//            return authorizationService.TryCheckAccess(permission, workContext.CurrentUser);
//        }
//    }
//}
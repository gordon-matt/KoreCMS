using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.Results;
using Kore.Collections;
using Kore.Data;
using Kore.Localization;
using Kore.Plugins.Maintenance.Watchdog.Data.Domain;
using Kore.Plugins.Maintenance.Watchdog.Models;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using Newtonsoft.Json;

namespace Kore.Plugins.Maintenance.Watchdog.Controllers.Api
{
    public class WatchdogInstanceApiController : GenericODataController<WatchdogInstance, int>
    {
        private const string urlFormat = "{0}/api/WatchdogApi/{1}?password={2}&name={3}";

        private readonly Lazy<WatchdogSettings> settings;

        public Localizer T { get; set; }

        public WatchdogInstanceApiController(
            IRepository<WatchdogInstance> repository,
            Lazy<WatchdogSettings> settings)
            : base(repository)
        {
            this.settings = settings;
            T = LocalizationUtilities.Resolve();
        }

        #region GenericODataController<WatchdogInstance, int> Members

        protected override int GetId(WatchdogInstance entity)
        {
            return entity.Id;
        }

        protected override void SetNewId(WatchdogInstance entity)
        {
            // Do nothing
        }

        protected override Permission ReadPermission
        {
            get { return WatchdogPermissions.Read; }
        }

        protected override Permission WritePermission
        {
            get { return WatchdogPermissions.Write; }
        }

        #endregion GenericODataController<WatchdogInstance, int> Members

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpPost]
        public IQueryable<ServiceInfoResult> GetServices(ODataActionParameters parameters)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Enumerable.Empty<ServiceInfoResult>().AsQueryable();
            }

            int watchdogInstanceId = (int)parameters["watchdogInstanceId"];

            var instance = Repository.Find(watchdogInstanceId);

            using (var client = new HttpClient())
            {
                var response = client.GetAsync(instance.Url.TrimEnd('/') + "/api/WatchdogApi/GetWatchedServices?password=" + instance.Password).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var watchedServices = JsonConvert.DeserializeObject<List<ServiceInfoResult>>(content);

                if (settings.Value.OnlyShowWatched)
                {
                    var items = watchedServices.Select(x => new ServiceInfoResult
                    {
                        WatchdogInstanceId = watchdogInstanceId,
                        ServiceName = x.ServiceName,
                        DisplayName = x.DisplayName,
                        Status = x.Status,
                        IsWatched = true
                    });

                    return items.AsQueryable();
                }
                else
                {
                    response = client.GetAsync(instance.Url.TrimEnd('/') + "/api/WatchdogApi/GetServices?password=" + instance.Password).Result;
                    content = response.Content.ReadAsStringAsync().Result;
                    var allServices = JsonConvert.DeserializeObject<List<ServiceInfoResult>>(content);

                    var watchedServiceNames = watchedServices.Select(x => x.ServiceName).ToHashSet();

                    var items = allServices.Select(x => new ServiceInfoResult
                    {
                        WatchdogInstanceId = watchdogInstanceId,
                        ServiceName = x.ServiceName,
                        DisplayName = x.DisplayName,
                        Status = x.Status,
                        IsWatched = watchedServiceNames.Contains(x.ServiceName)
                    });

                    return items.AsQueryable();
                }
            }
        }

        [HttpPost]
        public ChangeStatusResult StopService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WatchdogPermissions.StartStopServices))
            {
                return new ChangeStatusResult { Successful = false, Message = T(LocalizableStrings.Unauthorized) };
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = Repository.Find(watchdogInstanceId);

                var response = client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Stop",
                    watchdogInstance.Password,
                    serviceName)).Result;

                string content = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ChangeStatusResult>(content);
                return result;
            }
        }

        [HttpPost]
        public ChangeStatusResult StartService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WatchdogPermissions.StartStopServices))
            {
                return new ChangeStatusResult { Successful = false, Message = T(LocalizableStrings.Unauthorized) };
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = Repository.Find(watchdogInstanceId);

                var response = client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Start",
                    watchdogInstance.Password,
                    serviceName)).Result;

                string content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ChangeStatusResult>(content);
            }
        }

        [HttpPost]
        public ChangeStatusResult RestartService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WatchdogPermissions.StartStopServices))
            {
                return new ChangeStatusResult { Successful = false, Message = T(LocalizableStrings.Unauthorized) };
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = Repository.Find(watchdogInstanceId);

                var response = client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Restart",
                    watchdogInstance.Password,
                    serviceName)).Result;

                string content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<ChangeStatusResult>(content);
            }
        }

        [HttpPost]
        public IHttpActionResult AddService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = Repository.Find(watchdogInstanceId);

                var response = client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Add",
                    watchdogInstance.Password,
                    serviceName)).Result;

                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult RemoveService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return new UnauthorizedResult(new AuthenticationHeaderValue[0], ActionContext.Request);
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = Repository.Find(watchdogInstanceId);

                var response = client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Remove",
                    watchdogInstance.Password,
                    serviceName)).Result;

                return Ok();
            }
        }
    }
}
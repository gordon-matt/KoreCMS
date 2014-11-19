using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using Kore.Collections;
using Kore.Data;
using Kore.Web.Http.OData;
using Kore.Plugins.Watchdog.Data.Domain;
using Kore.Plugins.Watchdog.Models;
using Newtonsoft.Json;

namespace Kore.Plugins.Watchdog.Controllers.Api
{
    public class WatchdogInstancesController : GenericODataController<WatchdogInstance, int>
    {
        private const string urlFormat = "{0}/api/WatchdogApi/{1}?password={2}&name={3}";

        private readonly Lazy<WatchdogSettings> settings;

        public WatchdogInstancesController(IRepository<WatchdogInstance> repository, Lazy<WatchdogSettings> settings)
            : base(repository)
        {
            this.settings = settings;
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

        #endregion GenericODataController<WatchdogInstance, int> Members

        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpPost]
        public IQueryable<ServiceInfoResult> GetServices(ODataActionParameters parameters)
        {
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
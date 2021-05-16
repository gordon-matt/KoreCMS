using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Kore.Collections;
using Kore.Localization;
using Kore.Plugins.Maintenance.Watchdog.Data.Domain;
using Kore.Plugins.Maintenance.Watchdog.Models;
using Kore.Plugins.Maintenance.Watchdog.Services;
using Kore.Web.Http.OData;
using Kore.Web.Security.Membership.Permissions;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Query;
using Newtonsoft.Json;

namespace Kore.Plugins.Maintenance.Watchdog.Controllers.Api
{
    public class WatchdogInstanceApiController : GenericTenantODataController<WatchdogInstance, int>
    {
        private const string urlFormat = "{0}/api/WatchdogApi/{1}?password={2}&name={3}";

        private readonly Lazy<WatchdogSettings> settings;

        public Localizer T { get; set; }

        public WatchdogInstanceApiController(
            IWatchdogInstanceService service,
            Lazy<WatchdogSettings> settings)
            : base(service)
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

        //[EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All)]
        [HttpGet]
        public async Task<IHttpActionResult> GetServices([FromODataUri] int watchdogInstanceId, ODataQueryOptions<ServiceInfoResult> options)
        {
            if (!CheckPermission(ReadPermission))
            {
                return Unauthorized();
            }

            options.Validate(new ODataValidationSettings()
            {
                AllowedQueryOptions = AllowedQueryOptions.All
            });

            var instance = await Service.FindOneAsync(watchdogInstanceId);

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(instance.Url.TrimEnd('/') + "/api/WatchdogApi/GetWatchedServices?password=" + instance.Password);
                var content = await response.Content.ReadAsStringAsync();
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

                    var query = items.AsQueryable();
                    var results = options.ApplyTo(query);
                    return Ok((results as IQueryable<ServiceInfoResult>).ToHashSet());
                }
                else
                {
                    response = await client.GetAsync(instance.Url.TrimEnd('/') + "/api/WatchdogApi/GetServices?password=" + instance.Password);
                    content = await response.Content.ReadAsStringAsync();
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

                    var query = items.AsQueryable();
                    var results = options.ApplyTo(query);
                    return Ok((results as IQueryable<ServiceInfoResult>).ToHashSet());
                }
            }
        }

        [HttpPost]
        public async Task<ChangeStatusResult> StopService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WatchdogPermissions.StartStopServices))
            {
                return new ChangeStatusResult { Successful = false, Message = T(LocalizableStrings.Unauthorized).Text };
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = await Service.FindOneAsync(watchdogInstanceId);

                var response = await client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Stop",
                    watchdogInstance.Password,
                    serviceName));

                string content = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ChangeStatusResult>(content);
                return result;
            }
        }

        [HttpPost]
        public async Task<ChangeStatusResult> StartService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WatchdogPermissions.StartStopServices))
            {
                return new ChangeStatusResult { Successful = false, Message = T(LocalizableStrings.Unauthorized).Text };
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = await Service.FindOneAsync(watchdogInstanceId);

                var response = await client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Start",
                    watchdogInstance.Password,
                    serviceName));

                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ChangeStatusResult>(content);
            }
        }

        [HttpPost]
        public async Task<ChangeStatusResult> RestartService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WatchdogPermissions.StartStopServices))
            {
                return new ChangeStatusResult { Successful = false, Message = T(LocalizableStrings.Unauthorized).Text };
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = await Service.FindOneAsync(watchdogInstanceId);

                var response = await client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Restart",
                    watchdogInstance.Password,
                    serviceName));

                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ChangeStatusResult>(content);
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> AddService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = await Service.FindOneAsync(watchdogInstanceId);

                var response = await client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Add",
                    watchdogInstance.Password,
                    serviceName));

                return Ok();
            }
        }

        [HttpPost]
        public async Task<IHttpActionResult> RemoveService(ODataActionParameters parameters)
        {
            if (!CheckPermission(WritePermission))
            {
                return Unauthorized();
            }

            string serviceName = (string)parameters["name"];
            int watchdogInstanceId = (int)parameters["instanceId"];

            using (var client = new HttpClient())
            {
                var watchdogInstance = await Service.FindOneAsync(watchdogInstanceId);

                var response = await client.GetAsync(string.Format(
                    urlFormat,
                    watchdogInstance.Url.TrimEnd('/'),
                    "Remove",
                    watchdogInstance.Password,
                    serviceName));

                return Ok();
            }
        }
    }
}
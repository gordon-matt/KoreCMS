using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceProcess;
using System.Web.Http;
using Kore.Collections;

namespace Kore.WatchdogService
{
    public class WatchdogApiController : ApiController
    {
        public IEnumerable<ServiceInfoResult> GetServices()
        {
            return ServiceController.GetServices()
                .OrderBy(x => x.DisplayName)
                .Select(x => new ServiceInfoResult
                {
                    ServiceName = x.ServiceName,
                    DisplayName = x.DisplayName,
                    Status = x.Status.ToString()
                }).ToHashSet();
        }

        public IEnumerable<ServiceInfoResult> GetWatchedServices()
        {
            return ServiceController.GetServices()
                .Where(x => Global.Settings.Services.Contains(x.ServiceName))
                .OrderBy(x => x.DisplayName)
                .Select(x => new ServiceInfoResult
                {
                    ServiceName = x.ServiceName,
                    DisplayName = x.DisplayName,
                    Status = x.Status.ToString()
                }).ToHashSet();
        }

        [HttpGet]
        public void Add(string name)
        {
            if (ServiceController.GetServices().Any(x => x.ServiceName == name))
            {
                Global.Settings.Services.Add(name);
                Global.Settings.Save(Global.SettingsFilePath);
            }
        }

        [HttpGet]
        public void Remove(string name)
        {
            if (Global.Settings.Services.Contains(name))
            {
                Global.Settings.Services.Remove(name);
                Global.Settings.Save(Global.SettingsFilePath);
            }
        }

        [HttpGet]
        public ChangeStatusResult Start(string name)
        {
            var sc = new ServiceController(name);

            if (sc != null && sc.Status != ServiceControllerStatus.Running)
            {
                try
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                    return new ChangeStatusResult { Successful = true, Message = string.Format("Successfully started service, '{0}'.", name) };
                }
                catch (InvalidOperationException x)
                {
                    return new ChangeStatusResult { Successful = false, Message = string.Format("Error starting service: {0}", x.GetBaseException().Message) };
                }
            }
            return new ChangeStatusResult { Successful = false, Message = string.Format("Could not find service: {0}", name) };
        }

        [HttpGet]
        public ChangeStatusResult Stop(string name)
        {
            var sc = new ServiceController(name);

            if (sc != null && sc.Status != ServiceControllerStatus.Stopped)
            {
                if (!sc.CanStop)
                {
                    return new ChangeStatusResult { Successful = false, Message = string.Format("Service, '{0}' cannot be stopped", name) };
                }

                try
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
                    return new ChangeStatusResult { Successful = true, Message = string.Format("Successfully stopped service, '{0}'.", name) };
                }
                catch (InvalidOperationException x)
                {
                    return new ChangeStatusResult { Successful = false, Message = string.Format("Error stopping service: {0}", x.GetBaseException().Message) };
                }
            }
            return new ChangeStatusResult { Successful = false, Message = string.Format("Could not find service: {0}", name) };
        }

        [HttpGet]
        public ChangeStatusResult Restart(string name)
        {
            var sc = new ServiceController(name);

            if (sc != null)
            {
                if (!sc.CanStop)
                {
                    return new ChangeStatusResult { Successful = false, Message = string.Format("Service, '{0}' cannot be stopped", name) };
                }

                try
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                    return new ChangeStatusResult { Successful = true, Message = string.Format("Successfully restarted service, '{0}'.", name) };
                }
                catch (InvalidOperationException x)
                {
                    return new ChangeStatusResult { Successful = false, Message = string.Format("Error restarting service: {0}", x.GetBaseException().Message) };
                }
            }
            return new ChangeStatusResult { Successful = false, Message = string.Format("Could not find service: {0}", name) };
        }
    }

    [DataContract]
    public struct ServiceInfoResult
    {
        [DataMember]
        public string ServiceName { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string Status { get; set; }
    }

    [DataContract]
    public struct ChangeStatusResult
    {
        [DataMember]
        public bool Successful { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}
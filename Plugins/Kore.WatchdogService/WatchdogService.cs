using System;
using System.ServiceProcess;
using System.Timers;
using Microsoft.Owin.Hosting;
using NLog;

namespace Kore.WatchdogService
{
    public partial class WatchdogService : ServiceBase
    {
        #region Private Variables

        private static readonly Logger logger = LogManager.GetLogger("Kore.WatchdogService.WatchdogService");
        private Timer timer;

        private static IDisposable selfHostedWebApiService;

        #endregion Private Variables

        public WatchdogService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Watchdog Service Started");

            selfHostedWebApiService = WebApp.Start<Startup>(url: Global.APIBaseAddress);

            timer = new Timer(Global.TimerInterval)
            {
                AutoReset = true
            };

            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            foreach (string service in Global.Settings.Services)
            {
                var sc = new ServiceController(service);
                EnsureServiceRunning(sc);
            }
        }

        private static void EnsureServiceRunning(ServiceController sc)
        {
            switch (sc.Status)
            {
                case ServiceControllerStatus.ContinuePending:
                    {
                        sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                    }
                    break;

                case ServiceControllerStatus.Paused:
                    {
                        sc.Continue();
                        sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                    }
                    break;

                case ServiceControllerStatus.PausePending:
                    {
                        sc.WaitForStatus(ServiceControllerStatus.Paused);
                        sc.Continue();
                        sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                    }
                    break;

                case ServiceControllerStatus.StartPending:
                    {
                        sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                    }
                    break;

                case ServiceControllerStatus.Stopped:
                    {
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                    }
                    break;

                case ServiceControllerStatus.StopPending:
                    {
                        sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
                        sc.Start();
                        sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 1, 0));
                    }
                    break;

                case ServiceControllerStatus.Running:
                default: break;
            }
        }

        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
            selfHostedWebApiService.Dispose();
            logger.Info("Watchdog Service Stopped");
        }
    }
}
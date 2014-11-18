using System.ServiceProcess;

namespace Kore.WatchdogService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WatchdogService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
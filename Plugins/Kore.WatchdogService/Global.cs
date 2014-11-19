using System.Configuration;
using System.IO;
using System.Reflection;

namespace Kore.WatchdogService
{
    public static class Global
    {
        public static readonly string SettingsFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase.Substring(8)), "Settings.xml");

        private static Settings settings;
        private static int timerInterval;
        private static string apiBaseAddress;
        private static string apiPassword;

        public static Settings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = Settings.Load(SettingsFilePath);
                }
                return settings;
            }
        }

        public static int TimerInterval
        {
            get
            {
                if (timerInterval == 0)
                {
                    timerInterval = int.Parse(ConfigurationManager.AppSettings["TimerInterval"]);
                }
                return timerInterval;
            }
        }

        public static string APIBaseAddress
        {
            get
            {
                if (string.IsNullOrEmpty(apiBaseAddress))
                {
                    apiBaseAddress = ConfigurationManager.AppSettings["APIBaseAddress"];
                }
                return apiBaseAddress;
            }
        }

        public static string APIPassword
        {
            get
            {
                if (string.IsNullOrEmpty(apiPassword))
                {
                    apiPassword = ConfigurationManager.AppSettings["APIPassword"];
                }
                return apiPassword;
            }
        }
    }
}
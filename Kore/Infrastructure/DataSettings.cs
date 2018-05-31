using System;
using System.IO;
using System.Web.Hosting;

namespace Kore.Infrastructure
{
    public class DataSettings
    {
        public string ProviderName { get; set; }

        public string ConnectionString { get; set; }

        public string AdminEmail { get; set; }

        public string AdminPassword { get; set; }

        public bool CreateSampleData { get; set; }

        public string DefaultLanguage { get; set; }

        public string Theme { get; set; }

        public bool IsValid()
        {
            return !string.IsNullOrEmpty(this.ProviderName) && !string.IsNullOrEmpty(this.ConnectionString);
        }
    }

    public static class DataSettingsManager
    {
        private const string virtualPath = "~/App_Data/KoreSettings.xml";

        /// <summary>
        /// Maps a virtual path to a physical disk path.
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        private static string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                return HostingEnvironment.MapPath(path);
            }
            else
            {
                // Not hosted. For example, run in unit tests
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                return Path.Combine(baseDirectory, path);
            }
        }

        /// <summary>
        /// Load settings
        /// </summary>
        /// <param name="filePath">File path; pass null to use default settings file path</param>
        /// <returns></returns>
        public static DataSettings LoadSettings()
        {
            string filePath = MapPath(virtualPath);

            if (File.Exists(filePath))
            {
                string text = File.ReadAllText(filePath);
                return text.XmlDeserialize<DataSettings>();
            }
            else
            {
                return new DataSettings();
            }
        }

        public static void SaveSettings(DataSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            //use webHelper.MapPath instead of HostingEnvironment.MapPath which is not available in unit tests
            string filePath = MapPath(virtualPath);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                    //we use 'using' to close the file after it's created
                }
            }

            string xml = settings.XmlSerialize();
            File.WriteAllText(filePath, xml);
        }
    }

    public static class DataSettingsHelper
    {
        private static bool? isDatabaseInstalled;

        public static bool IsDatabaseInstalled
        {
            get
            {
                if (!isDatabaseInstalled.HasValue)
                {
                    var settings = DataSettingsManager.LoadSettings();
                    isDatabaseInstalled = settings != null && !string.IsNullOrEmpty(settings.ConnectionString);
                }
                return isDatabaseInstalled.Value;
            }
        }

        public static void ResetCache()
        {
            isDatabaseInstalled = null;
        }
    }
}
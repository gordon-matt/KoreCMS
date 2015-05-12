using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Kore.Caching;
using Microsoft.Win32;

namespace Kore.Web.ContentManagement.FileSystems.Media
{
    /// <summary>
    /// Returns the mime-type by looking into IIS configuration and the Registry
    /// </summary>
    public class ConfigurationMimeTypeProvider : IMimeTypeProvider
    {
        private readonly ICacheManager cacheManager;
        private const string UnknownMimeType = "application/unknown";

        public ConfigurationMimeTypeProvider(ICacheManager cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        /// <summary>
        /// Returns the mime-type of the specified file path
        /// </summary>
        public string GetMimeType(string path)
        {
            string extension = Path.GetExtension(path);
            if (string.IsNullOrWhiteSpace(extension))
            {
                return UnknownMimeType;
            }

            string cacheKey = string.Format(Constants.CacheKeys.MediaMimeType, extension);
            return cacheManager.Get(cacheKey, () =>
            {
                try
                {
                    try
                    {
                        string applicationHost = System.Environment.ExpandEnvironmentVariables(@"%windir%\system32\inetsrv\config\applicationHost.config");
                        string webConfig = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").FilePath;

                        // search for custom mime types in web.config and applicationhost.config
                        foreach (var configFile in new[] { webConfig, applicationHost })
                        {
                            if (File.Exists(configFile))
                            {
                                var xDocument = XDocument.Load(configFile);
                                var mimeMap = xDocument.XPathSelectElements("//staticContent/mimeMap[@fileExtension='" + extension + "']").FirstOrDefault();
                                if (mimeMap != null)
                                {
                                    var mimeType = mimeMap.Attribute("mimeType");
                                    if (mimeType != null)
                                    {
                                        return mimeType.Value;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // ignore issues with web.config to fall back to registry
                    }

                    // search into the registry
                    var regKey = Registry.ClassesRoot.OpenSubKey(extension.ToLower());
                    if (regKey != null)
                    {
                        var contentType = regKey.GetValue("Content Type");
                        if (contentType != null)
                        {
                            return contentType.ToString();
                        }
                    }
                }
                catch
                {
                    // if an exception occured return application/unknown
                    return UnknownMimeType;
                }

                return UnknownMimeType;
            });
        }
    }
}
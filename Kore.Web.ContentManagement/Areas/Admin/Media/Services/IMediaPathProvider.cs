using System;
using System.Configuration;
using System.IO;
using System.Web.Hosting;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Services
{
    public interface IMediaPathProvider
    {
        /// <summary>
        /// Storage Path, ex: C:\\wwwroot\CMS\Media
        /// </summary>
        string StoragePath { get; }

        /// <summary>
        /// Public Path, ex: /Media
        /// </summary>
        string PublicPath { get; }
    }

    public class MediaPathProvider : IMediaPathProvider
    {
        public MediaPathProvider()
        {
            var configMediaFolder = ConfigurationManager.AppSettings["Kore.MediaFolder"];

            // TODO: think about tenancy in future:
            //var configMediaFolder = System.Configuration.ConfigurationManager.AppSettings[settings.Name + "MediaFolder"];
            if (!string.IsNullOrEmpty(configMediaFolder))
            {
                VirtualPath = configMediaFolder;
                StoragePath = HostingEnvironment.MapPath(VirtualPath);
                PublicPath = configMediaFolder.Trim('~');
            }
            else
            {
                var mediaPath = HostingEnvironment.IsHosted
                    ? HostingEnvironment.MapPath("~/Media/") ?? ""
                    : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");

                StoragePath = mediaPath;
                VirtualPath = "~/Media/";

                // TODO: think about tenancy in future:
                //StoragePath = Path.Combine(mediaPath, settings.Name);
                //VirtualPath = "~/Media/" + settings.Name + "/";

                var appPath = "";
                if (HostingEnvironment.IsHosted)
                {
                    appPath = HostingEnvironment.ApplicationVirtualPath ?? "";
                }

                if (!appPath.EndsWith("/"))
                {
                    appPath = appPath + '/';
                }

                if (!appPath.StartsWith("/"))
                {
                    appPath = '/' + appPath;
                }

                PublicPath = appPath + "Media/";

                // TODO: think about tenancy in future:
                //PublicPath = appPath + "Media/" + settings.Name + "/";
            }
        }

        public string StoragePath { get; private set; }

        public string VirtualPath { get; private set; }

        public string PublicPath { get; private set; }
    }
}
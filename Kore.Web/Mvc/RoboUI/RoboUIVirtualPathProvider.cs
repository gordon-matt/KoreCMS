using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using Kore.Web.Hosting;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUIVirtualPathProvider : VirtualPathProvider, IKoreVirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            if (virtualPath.EndsWith("RoboFormResult_.cshtml"))
            {
                return true;
            }
            return base.FileExists(virtualPath);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (virtualPath.EndsWith("RoboFormResult_.cshtml"))
            {
                return new RoboFormVirtualFile(virtualPath);
            }

            return base.GetFile(virtualPath);
        }

        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (virtualPath.EndsWith("RoboFormResult_.cshtml"))
            {
                return null;
            }
            return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        #region Nested type: RoboFormVirtualFile

        private class RoboFormVirtualFile : VirtualFile
        {
            public RoboFormVirtualFile(string virtualPath)
                : base(virtualPath)
            {
            }

            public override Stream Open()
            {
                string layoutPath = null;
                string currentArea = (string)HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];

                if (RoboSettings.AreaLayoutPaths.ContainsKey(currentArea))
                {
                    layoutPath = RoboSettings.AreaLayoutPaths[currentArea];
                }
                else
                {
                    layoutPath = RoboSettings.DefaultLayoutPath;
                }

                string content = string.Format(
@"@inherits Kore.Web.Mvc.WebViewPage<dynamic>
@{{
    Layout = ""{0}"";
}}
[ROBO_UI_PLACEHOLDER]", layoutPath);

                return new MemoryStream(Encoding.UTF8.GetBytes(content));
            }
        }

        #endregion Nested type: RoboFormVirtualFile

        public VirtualPathProvider Instance
        {
            get { return this; }
        }
    }
}
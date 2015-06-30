using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Kore.Web.Mvc.EmbeddedResources
{
    public class EmbeddedViewVirtualPathProvider : VirtualPathProvider
    {
        private readonly EmbeddedResourceTable embeddedViews;

        public EmbeddedViewVirtualPathProvider(EmbeddedResourceTable embeddedViews)
        {
            if (embeddedViews == null)
            {
                throw new ArgumentNullException("embeddedViews");
            }
            this.embeddedViews = embeddedViews;
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsEmbeddedView(virtualPath) || Previous.FileExists(virtualPath));
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            return IsEmbeddedView(virtualPath)
                ? null
                : Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedView(virtualPath))
            {
                string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);
                var fullyQualifiedName = virtualPathAppRelative.Substring(virtualPathAppRelative.LastIndexOf("/") + 1, virtualPathAppRelative.Length - 1 - virtualPathAppRelative.LastIndexOf("/"));

                var metadata = embeddedViews.FindEmbeddedResource(fullyQualifiedName);
                return new EmbeddedResourceVirtualFile(metadata, virtualPath);
            }

            return Previous.GetFile(virtualPath);
        }

        private bool IsEmbeddedView(string virtualPath)
        {
            /*old validation
            it can cause issue if we have several views with the same full path:
            for example: both Kore.Plugin.Plugin1 and Kore.Plugin.Plugin2 can have Views\Config\Configure.cshtml files

            That's why we specify FQN for views into plugin controllers
             */
            //string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);

            //return virtualPathAppRelative.StartsWith("~/Views/", StringComparison.InvariantCultureIgnoreCase)
            //       && _embeddedViews.ContainsEmbeddedView(virtualPathAppRelative);
            if (string.IsNullOrEmpty(virtualPath))
            {
                return false;
            }

            string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);
            if (!virtualPathAppRelative.StartsWith("~/Views/", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var fullyQualifiedName = virtualPathAppRelative.Substring(
                virtualPathAppRelative.LastIndexOf("/") + 1,
                virtualPathAppRelative.Length - 1 - virtualPathAppRelative.LastIndexOf("/"));

            bool isEmbedded = embeddedViews.ContainsEmbeddedResource(fullyQualifiedName);
            return isEmbedded;
        }
    }
}
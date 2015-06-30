using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Kore.Web.Mvc.EmbeddedResources
{
    public class EmbeddedStyleVirtualPathProvider : VirtualPathProvider
    {
        private readonly EmbeddedResourceTable embeddedStyles;

        public EmbeddedStyleVirtualPathProvider(EmbeddedResourceTable embeddedStyles)
        {
            if (embeddedStyles == null)
            {
                throw new ArgumentNullException("embeddedStyles");
            }
            this.embeddedStyles = embeddedStyles;
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsEmbeddedStyle(virtualPath) || Previous.FileExists(virtualPath));
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            return IsEmbeddedStyle(virtualPath)
                ? null
                : Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedStyle(virtualPath))
            {
                string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);
                var fullyQualifiedName = virtualPathAppRelative.Substring(virtualPathAppRelative.LastIndexOf("/") + 1, virtualPathAppRelative.Length - 1 - virtualPathAppRelative.LastIndexOf("/"));

                var metadata = embeddedStyles.FindEmbeddedResource(fullyQualifiedName);
                return new EmbeddedResourceVirtualFile(metadata, virtualPath);
            }

            return Previous.GetFile(virtualPath);
        }

        private bool IsEmbeddedStyle(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                return false;
            }

            string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);
            if (!virtualPathAppRelative.StartsWith("~/Content/", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var fullyQualifiedName = virtualPathAppRelative.Substring(
                virtualPathAppRelative.LastIndexOf("/") + 1,
                virtualPathAppRelative.Length - 1 - virtualPathAppRelative.LastIndexOf("/"));

            bool isEmbedded = embeddedStyles.ContainsEmbeddedResource(fullyQualifiedName);
            return isEmbedded;
        }
    }
}
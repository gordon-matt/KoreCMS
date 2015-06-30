using System;
using System.Collections;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;

namespace Kore.Web.Mvc.EmbeddedResources
{
    public class EmbeddedScriptVirtualPathProvider : VirtualPathProvider
    {
        private readonly EmbeddedResourceTable embeddedScripts;

        public EmbeddedScriptVirtualPathProvider(EmbeddedResourceTable embeddedScripts)
        {
            if (embeddedScripts == null)
            {
                throw new ArgumentNullException("embeddedScripts");
            }
            this.embeddedScripts = embeddedScripts;
        }

        public override bool FileExists(string virtualPath)
        {
            return (IsEmbeddedScript(virtualPath) || Previous.FileExists(virtualPath));
        }

        public override CacheDependency GetCacheDependency(
            string virtualPath,
            IEnumerable virtualPathDependencies,
            DateTime utcStart)
        {
            return IsEmbeddedScript(virtualPath)
                ? null
                : Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
        }

        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedScript(virtualPath))
            {
                string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);
                var fullyQualifiedName = virtualPathAppRelative.Substring(virtualPathAppRelative.LastIndexOf("/") + 1, virtualPathAppRelative.Length - 1 - virtualPathAppRelative.LastIndexOf("/"));

                var metadata = embeddedScripts.FindEmbeddedResource(fullyQualifiedName);
                return new EmbeddedResourceVirtualFile(metadata, virtualPath);
            }

            return Previous.GetFile(virtualPath);
        }

        private bool IsEmbeddedScript(string virtualPath)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                return false;
            }

            string virtualPathAppRelative = VirtualPathUtility.ToAppRelative(virtualPath);
            if (!virtualPathAppRelative.StartsWith("~/Scripts/", StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var fullyQualifiedName = virtualPathAppRelative.Substring(
                virtualPathAppRelative.LastIndexOf("/") + 1,
                virtualPathAppRelative.Length - 1 - virtualPathAppRelative.LastIndexOf("/"));

            bool isEmbedded = embeddedScripts.ContainsEmbeddedResource(fullyQualifiedName);
            return isEmbedded;
        }
    }
}
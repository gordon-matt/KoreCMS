using System;
using System.Collections.Generic;
using System.Linq;

namespace Kore.Web.Mvc.EmbeddedResources
{
    [Serializable]
    public class EmbeddedResourceTable
    {
        private static readonly object _lock = new object();
        private readonly Dictionary<string, EmbeddedResourceMetadata> resourceCache;

        public EmbeddedResourceTable()
        {
            resourceCache = new Dictionary<string, EmbeddedResourceMetadata>(StringComparer.InvariantCultureIgnoreCase);
        }

        public void AddResource(string resourceName, string assemblyName)
        {
            lock (_lock)
            {
                resourceCache[resourceName] = new EmbeddedResourceMetadata
                {
                    Name = resourceName,
                    AssemblyFullName = assemblyName
                };
            }
        }

        public IEnumerable<EmbeddedResourceMetadata> Resources
        {
            get { return resourceCache.Values; }
        }

        public bool ContainsEmbeddedResource(string fullyQualifiedName)
        {
            var foundView = FindEmbeddedResource(fullyQualifiedName);
            return (foundView != null);
        }

        public EmbeddedResourceMetadata FindEmbeddedResource(string fullyQualifiedName)
        {
            if (string.IsNullOrEmpty(fullyQualifiedName))
            {
                return null;
            }
            return Resources.SingleOrDefault(x => x.Name.ToLowerInvariant().Equals(fullyQualifiedName.ToLowerInvariant()));
        }
    }
}
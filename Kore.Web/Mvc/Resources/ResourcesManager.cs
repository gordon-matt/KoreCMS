using System;
using System.Collections.Generic;
using System.Linq;

namespace Kore.Web.Mvc.Resources
{
    public class ResourcesManager : IResourcesManager
    {
        private readonly Dictionary<string, MetaEntry> metas = new Dictionary<string, MetaEntry>();

        private readonly Dictionary<string, ResourceEntry> resources =
            new Dictionary<string, ResourceEntry>(StringComparer.InvariantCultureIgnoreCase);

        private readonly Dictionary<string, string> inlineResources = new Dictionary<string, string>();

        #region IResourceManager Members

        public void RegisterResource(ResourceEntry resourceEntry)
        {
            if (!resources.ContainsKey(resourceEntry.Path))
            {
                resources.Add(resourceEntry.Path, resourceEntry);
            }
        }

        public void RegisterInlineResource(string type, string code, bool ignoreExists = false)
        {
            if (ignoreExists)
            {
                if (!inlineResources.ContainsKey(code))
                {
                    inlineResources.Add(code, type);
                }
            }
            else
            {
                inlineResources.Add(code, type);
            }
        }

        public IEnumerable<ResourceEntry> GetResources(string type)
        {
            return resources
                .Where(x => x.Value.Type == type)
                .OrderBy(x => x.Value.Order)
                .Select(x => x.Value);
        }

        public IEnumerable<string> GetInlineResources(string type)
        {
            return inlineResources
                .Where(x => x.Value == type)
                .Select(x => x.Key);
        }

        public void SetMeta(MetaEntry meta)
        {
            metas[meta.Name ?? Guid.NewGuid().ToString()] = meta;
        }

        public void AppendMeta(MetaEntry meta, string contentSeparator)
        {
            if (meta == null || String.IsNullOrEmpty(meta.Name))
            {
                return;
            }

            MetaEntry existingMeta;
            if (metas.TryGetValue(meta.Name, out existingMeta))
            {
                meta = MetaEntry.Combine(existingMeta, meta, contentSeparator);
            }
            metas[meta.Name] = meta;
        }

        public virtual IEnumerable<MetaEntry> GetRegisteredMetas()
        {
            return metas.Values.ToList().AsReadOnly();
        }

        #endregion IResourceManager Members
    }
}
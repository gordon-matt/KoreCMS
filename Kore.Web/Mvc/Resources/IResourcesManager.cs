using System.Collections.Generic;

namespace Kore.Web.Mvc.Resources
{
    public interface IResourcesManager
    {
        void RegisterResource(ResourceEntry resourceEntry);

        void RegisterInlineResource(string type, string code, bool ignoreExists = false);

        IEnumerable<ResourceEntry> GetResources(string type);

        IEnumerable<string> GetInlineResources(string type);

        void SetMeta(MetaEntry meta);

        void AppendMeta(MetaEntry meta, string contentSeparator);

        IEnumerable<MetaEntry> GetRegisteredMetas();
    }
}
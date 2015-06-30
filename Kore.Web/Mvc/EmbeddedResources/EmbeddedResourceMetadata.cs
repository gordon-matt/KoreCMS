using System;

namespace Kore.Web.Mvc.EmbeddedResources
{
    [Serializable]
    public class EmbeddedResourceMetadata
    {
        public string Name { get; set; }

        public string AssemblyFullName { get; set; }
    }
}
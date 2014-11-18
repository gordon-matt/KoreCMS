using System;

namespace Kore.Web.Mvc.EmbeddedViews
{
    [Serializable]
    public class EmbeddedResourceMetadata
    {
        public string Name { get; set; }

        public string AssemblyFullName { get; set; }
    }
}
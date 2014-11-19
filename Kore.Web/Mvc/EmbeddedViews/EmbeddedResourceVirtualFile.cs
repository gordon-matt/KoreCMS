using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;

namespace Kore.Web.Mvc.EmbeddedViews
{
    public class EmbeddedResourceVirtualFile : VirtualFile
    {
        private readonly EmbeddedResourceMetadata metadata;

        public EmbeddedResourceVirtualFile(EmbeddedResourceMetadata metadata, string virtualPath)
            : base(virtualPath)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("embeddedViewMetadata");
            }
            this.metadata = metadata;
        }

        public override Stream Open()
        {
            Assembly assembly = GetResourceAssembly();
            return assembly == null ? null : assembly.GetManifestResourceStream(metadata.Name);
        }

        protected virtual Assembly GetResourceAssembly()
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(assembly =>
                string.Equals(assembly.FullName, metadata.AssemblyFullName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
using System.Reflection;
using Kore.Collections;
using Kore.Infrastructure;

namespace Kore.Web.Mvc.EmbeddedViews
{
    public class EmbeddedResourceResolver : IEmbeddedResourceResolver
    {
        private ITypeFinder typeFinder;

        private static EmbeddedResourceTable scripts;
        private static EmbeddedResourceTable views;

        public EmbeddedResourceResolver(ITypeFinder typeFinder)
        {
            this.typeFinder = typeFinder;
        }

        private void GetEmbeddedResources()
        {
            scripts = new EmbeddedResourceTable();
            views = new EmbeddedResourceTable();

            var assemblies = typeFinder.GetAssemblies();

            if (assemblies.IsNullOrEmpty())
            {
                return;
            }

            foreach (var assembly in assemblies)
            {
                var names = GetNamesOfAssemblyResources(assembly);

                if (names.IsNullOrEmpty())
                {
                    continue;
                }

                foreach (var name in names)
                {
                    var key = name.ToLowerInvariant();

                    if (key.Contains(".views."))
                    {
                        views.AddResource(name, assembly.FullName);
                    }
                    else if (key.Contains(".scripts."))
                    {
                        scripts.AddResource(name, assembly.FullName);
                    }
                }
            }
        }

        private static string[] GetNamesOfAssemblyResources(Assembly assembly)
        {
            //GetManifestResourceNames will throw a NotSupportedException when run on a dynamic assembly
            try
            {
                if (!assembly.IsDynamic)
                {
                    return assembly.GetManifestResourceNames();
                }
            }
            catch
            {
                // Any exception we fall back to returning an empty array.
            }

            return new string[] { };
        }

        #region IEmbeddedResourceResolver Members

        public EmbeddedResourceTable Scripts
        {
            get
            {
                if (scripts == null)
                {
                    GetEmbeddedResources();
                }
                return scripts;
            }
        }

        public EmbeddedResourceTable Views
        {
            get
            {
                if (views == null)
                {
                    GetEmbeddedResources();
                }
                return views;
            }
        }

        #endregion IEmbeddedResourceResolver Members
    }
}
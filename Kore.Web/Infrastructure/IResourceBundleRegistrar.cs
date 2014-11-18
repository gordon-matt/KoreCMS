using System.Web.Optimization;

namespace Kore.Web.Infrastructure
{
    public interface IResourceBundleRegistrar
    {
        void Register(BundleCollection bundles);
    }
}
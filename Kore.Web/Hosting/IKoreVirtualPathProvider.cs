using System.Web.Hosting;

namespace Kore.Web.Hosting
{
    public interface IKoreVirtualPathProvider
    {
        VirtualPathProvider Instance { get; }
    }
}
using Kore.Caching;

namespace Kore.Web.IO.FileSystems.VirtualPath
{
    /// <summary>
    /// Enable monitoring changes over virtual path
    /// </summary>
    public interface IVirtualPathMonitor
    {
        IVolatileToken WhenPathChanges(string virtualPath);
    }
}
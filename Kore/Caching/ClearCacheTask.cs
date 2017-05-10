using Kore.Infrastructure;
using Kore.Tasks;

namespace Kore.Caching
{
    /// <summary>
    /// Clear cache schedueled task implementation
    /// </summary>
    public class ClearCacheTask : ITask
    {
        public string Name
        {
            get { return "Clear Cache Task"; }
        }

        public int DefaultInterval
        {
            get { return 600; }
        }

        public void Execute()
        {
            var staticCacheManager = EngineContext.Current.ResolveNamed<ICacheManager>("Kore_Cache_Static");
            staticCacheManager.Clear();

            var perRequestCacheManager = EngineContext.Current.ResolveNamed<ICacheManager>("Kore_Cache_Per_Request");
            perRequestCacheManager.Clear();
        }
    }
}
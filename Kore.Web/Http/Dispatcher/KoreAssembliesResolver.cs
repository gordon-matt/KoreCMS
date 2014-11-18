using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http.Dispatcher;
using Kore.Infrastructure;

namespace Kore.Web.Http.Dispatcher
{
    public class KoreAssembliesResolver : DefaultAssembliesResolver
    {
        public override ICollection<Assembly> GetAssemblies()
        {
            return EngineContext.Current.Resolve<ITypeFinder>().GetAssemblies().ToArray();
        }
    }
}
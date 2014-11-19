using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public class IndexerToken : IMemberAccessToken
    {
        private readonly ReadOnlyCollection<object> arguments;

        public IndexerToken(IEnumerable<object> arguments)
        {
            this.arguments = new ReadOnlyCollection<object>(arguments.ToArray());
        }

        public IndexerToken(params object[] arguments)
            : this((IEnumerable<object>)arguments)
        {
        }

        public ReadOnlyCollection<object> Arguments
        {
            get { return arguments; }
        }
    }
}
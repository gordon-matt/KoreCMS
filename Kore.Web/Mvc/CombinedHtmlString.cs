using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kore.Web.Mvc
{
    public class CombinedHtmlString : IHtmlString
    {
        private readonly string htmlString;

        public CombinedHtmlString(params object[] fragments)
        {
            htmlString = fragments.Where(x => x != null).Aggregate("", (a, b) => a + b);
        }

        public CombinedHtmlString(params IHtmlString[] fragments)
        {
            htmlString = fragments.Where(x => x != null).Aggregate("", (a, b) => a + b);
        }

        public CombinedHtmlString(IEnumerable<IHtmlString> fragments)
        {
            htmlString = fragments.Where(x => x != null).Aggregate("", (a, b) => a + b);
        }

        public string ToHtmlString()
        {
            return htmlString;
        }

        public override string ToString()
        {
            return htmlString;
        }
    }
}
using System.Web.Mvc;
using Kore.Web.Mvc.Bootstrap.Version2;
using Kore.Web.Mvc.Bootstrap.Version3;

namespace Kore.Web.Mvc.Bootstrap
{
    public static class HtmlHelperExtensions
    {
        public static Bootstrap2<TModel> Bootstrap2<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Bootstrap2<TModel>(htmlHelper);
        }

        public static Bootstrap3<TModel> Bootstrap3<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new Bootstrap3<TModel>(htmlHelper);
        }
    }
}
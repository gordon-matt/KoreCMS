using System.Web.Mvc;

namespace Kore.Web.Mvc.JQGrid
{
    public static class HtmlHelperExtensions
    {
        public static JQGrid<TModel> JQGrid<TModel>(this HtmlHelper htmlHelper, string id) where TModel : class
        {
            return new JQGrid<TModel>(id);
        }
    }
}
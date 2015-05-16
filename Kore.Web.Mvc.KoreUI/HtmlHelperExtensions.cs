using System.Web.Mvc;
using Kore.Web.Mvc.KoreUI;

namespace Kore.Web.Mvc.KoreUI
{
    public static class HtmlHelperExtensions
    {
        public static KoreUI<TModel> KoreUI<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            return new KoreUI<TModel>(htmlHelper);
        }
    }
}
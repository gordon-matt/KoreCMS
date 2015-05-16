using System.Web.Mvc;

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
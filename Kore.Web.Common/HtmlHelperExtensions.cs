using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kore.Infrastructure;
using Kore.Web.Collections;
using Kore.Web.Common.Areas.Admin.Regions.Services;

namespace Kore.Web.Common
{
    public static class HtmlHelperExtensions
    {
        public static KoreCommon<TModel> KoreCommon<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new KoreCommon<TModel>(html);
        }
    }

    public class KoreCommon<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal KoreCommon(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString CountryDropDownList(string name, string selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetCountries(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString CountryDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetCountries(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetCountries(string selectedValue = null, string emptyText = null)
        {
            var service = EngineContext.Current.Resolve<IRegionService>();

            var records = service.GetCountries().OrderBy(x => x.Name);

            return records.ToSelectList(
                value => value.Id,
                text => text.Name,
                selectedValue,
                emptyText);
        }
    }
}
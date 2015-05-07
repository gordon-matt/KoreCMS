using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kore.Collections;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Plugins.Ecommerce.Simple.Data.Domain;
using Kore.Web.Collections;

namespace Kore.Plugins.Ecommerce.Simple
{
    public static class HtmlHelperExtensions
    {
        public static SimpleCommerce<TModel> SimpleCommerce<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new SimpleCommerce<TModel>(html);
        }
    }

    public class SimpleCommerce<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal SimpleCommerce(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString CategoriesDropDownList(string name, string selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetCategoriesSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString CategoriesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetCategoriesSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetCategoriesSelectList(string selectedValue = null, string emptyText = null)
        {
            var repository = EngineContext.Current.Resolve<IRepository<Category>>();

            return repository.Table
                .OrderBy(x => x.Name)
                .ToHashSet()
                .ToSelectList(
                    value => value.Id,
                    text => text.Name,
                    selectedValue,
                    emptyText);
        }
    }
}
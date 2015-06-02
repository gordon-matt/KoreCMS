using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kore.Collections;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Plugins.Widgets.FullCalendar.Data.Domain;
using Kore.Web.Collections;

namespace Kore.Plugins.Widgets.FullCalendar
{
    public static class HtmlHelperExtensions
    {
        public static FullCalendar<TModel> FullCalendar<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new FullCalendar<TModel>(html);
        }
    }

    public class FullCalendar<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal FullCalendar(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString CalendarDropDownList(string name, int? selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetCalendarSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString CalendarDropDownListFor(Expression<Func<TModel, int>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetCalendarSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetCalendarSelectList(int? selectedValue = null, string emptyText = null)
        {
            var repository = EngineContext.Current.Resolve<IRepository<Calendar>>();

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
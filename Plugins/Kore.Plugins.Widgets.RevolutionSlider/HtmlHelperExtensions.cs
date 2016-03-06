using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kore.Collections;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Collections;
using Slider = Kore.Plugins.Widgets.RevolutionSlider.Data.Domain.RevolutionSlider;

namespace Kore.Plugins.Widgets.RevolutionSlider
{
    public static class HtmlHelperExtensions
    {
        public static RevolutionSlider<TModel> RevolutionSlider<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new RevolutionSlider<TModel>(html);
        }
    }

    public class RevolutionSlider<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal RevolutionSlider(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString SliderDropDownList(string name, int? selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetSliderSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString SliderDropDownListFor(Expression<Func<TModel, int>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetSliderSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetSliderSelectList(int? selectedValue = null, string emptyText = null)
        {
            var repository = EngineContext.Current.Resolve<IRepository<Slider>>();

            using (var connection = repository.OpenConnection())
            {
                return connection.Query()
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
}
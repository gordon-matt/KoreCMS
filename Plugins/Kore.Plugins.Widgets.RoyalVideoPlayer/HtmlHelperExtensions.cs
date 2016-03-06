using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kore.Collections;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Plugins.Widgets.RoyalVideoPlayer.Data.Domain;
using Kore.Web.Collections;
using Kore.Web.Mvc;

namespace Kore.Plugins.Widgets.RoyalVideoPlayer
{
    public static class HtmlHelperExtensions
    {
        public static RoyalVideoPlayer<TModel> RoyalVideoPlayer<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new RoyalVideoPlayer<TModel>(html);
        }
    }

    public class RoyalVideoPlayer<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal RoyalVideoPlayer(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString PlaylistDropDownList(string name, int? selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetPlaylistSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString PlaylistDropDownListFor(Expression<Func<TModel, int>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetPlaylistSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public MvcHtmlString PlaylistCheckBoxList(
            string name,
            IEnumerable<string> selectedIds,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
        {
            var selectList = GetPlaylistSelectList();

            return html.CheckBoxList(
                name,
                selectList,
                selectedIds,
                labelHtmlAttributes: labelHtmlAttributes,
                checkboxHtmlAttributes: checkboxHtmlAttributes);
        }

        public MvcHtmlString PlaylistCheckBoxListFor(
            Expression<Func<TModel, IEnumerable<string>>> expression,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
        {
            var selectList = GetPlaylistSelectList();

            return html.CheckBoxListFor(
                expression,
                selectList,
                labelHtmlAttributes: labelHtmlAttributes,
                checkboxHtmlAttributes: checkboxHtmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetPlaylistSelectList(int? selectedValue = null, string emptyText = null)
        {
            var repository = EngineContext.Current.Resolve<IRepository<Playlist>>();

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
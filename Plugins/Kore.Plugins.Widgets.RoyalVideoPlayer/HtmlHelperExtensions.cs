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

        private static IEnumerable<SelectListItem> GetPlaylistSelectList(int? selectedValue = null, string emptyText = null)
        {
            var repository = EngineContext.Current.Resolve<IRepository<Playlist>>();

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
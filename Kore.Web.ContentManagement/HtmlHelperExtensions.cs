using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kore.Infrastructure;
using Kore.Web.Collections;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Services;
using Kore.Web.ContentManagement.Areas.Admin.Widgets;
using Kore.Web.ContentManagement.Areas.Admin.Widgets.Services;

namespace Kore.Web.ContentManagement
{
    public static class HtmlHelperExtensions
    {
        public static KoreCMS<TModel> KoreCMS<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new KoreCMS<TModel>(html);
        }

        public static MvcHtmlString Menu(this HtmlHelper html, string menuName, string templateViewName)
        {
            return html.Action("Menu", "Frontend", new { area = "", name = menuName, templateViewName = templateViewName });
        }

        public static MvcHtmlString WidgetZone(this HtmlHelper html, string zoneName)
        {
            return html.Action("WidgetsByZone", "Frontend", new { area = "", zoneName = zoneName });
        }
    }

    public class KoreCMS<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal KoreCMS(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString WidgetTypesDropDownList(string name, string selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetWidgetTypesSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString WidgetTypesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetWidgetTypesSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public MvcHtmlString ZonesDropDownList(string name, string selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetZonesSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString ZonesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetZonesSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        private static string GetTypeFullName(Type type)
        {
            return string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);
        }

        private static IEnumerable<SelectListItem> GetWidgetTypesSelectList(string selectedValue = null, string emptyText = null)
        {
            var widgets = EngineContext.Current.ResolveAll<IWidget>();

            var widgetTypes = widgets
                .Select(x => new
                {
                    x.Name,
                    Type = GetTypeFullName(x.GetType())
                })
                .OrderBy(x => x.Name)
                .ToDictionary(k => k.Name, v => v.Type);

            return widgetTypes.ToSelectList(
                value => value.Value,
                text => text.Key,
                selectedValue,
                emptyText);
        }

        private static IEnumerable<SelectListItem> GetZonesSelectList(string selectedValue = null, string emptyText = null)
        {
            var zoneService = EngineContext.Current.Resolve<IZoneService>();

            return zoneService.Repository.Table
                .ToList()
                .ToSelectList(
                    value => value.Id,
                    text => text.Name,
                    selectedValue,
                    emptyText);
        }
    }
}
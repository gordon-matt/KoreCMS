using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Collections;
using Kore.Web.ContentManagement.Areas.Admin.Blog.Services;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Services;

namespace Kore.Web.ContentManagement
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString AutoBreadcrumbs(this HtmlHelper html, string templateViewName)
        {
            return html.Action("AutoBreadcrumbs", "Frontend", new
            {
                area = string.Empty,
                templateViewName = templateViewName
            });
        }

        public static MvcHtmlString AutoMenu(this HtmlHelper html, string templateViewName, bool includeHomePageLink = true)
        {
            return html.Action("AutoMenu", "Frontend", new
            {
                area = string.Empty,
                templateViewName = templateViewName,
                includeHomePageLink = includeHomePageLink
            });
        }

        public static MvcHtmlString AutoSubMenu(this HtmlHelper html, string templateViewName)
        {
            return html.Action("AutoSubMenu", "Frontend", new
            {
                area = string.Empty,
                templateViewName = templateViewName
            });
        }

        public static MvcHtmlString ContentZone(this HtmlHelper html, string zoneName, bool renderAsWidgets = false, WidgetColumns widgetColumns = WidgetColumns.Default)
        {
            return html.Action("ContentBlocksByZone", "Frontend", new
            {
                area = string.Empty,
                zoneName = zoneName,
                renderAsWidgets = renderAsWidgets,
                widgetColumns = widgetColumns
            });
        }

        public static MvcHtmlString EntityTypeContentZone(this HtmlHelper html, string zoneName, string entityType, object entityId, bool renderAsWidgets = false, WidgetColumns widgetColumns = WidgetColumns.Default)
        {
            return html.Action("EntityTypeContentBlocksByZone", "Frontend", new
            {
                area = string.Empty,
                entityType = entityType,
                entityId = entityId,
                zoneName = zoneName,
                renderAsWidgets = renderAsWidgets,
                widgetColumns = widgetColumns
            });
        }

        public static KoreCMS<TModel> KoreCMS<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new KoreCMS<TModel>(html);
        }

        public static MvcHtmlString Menu(this HtmlHelper html, string menuName, string templateViewName, bool filterByUrl = false)
        {
            return html.Action("Menu", "Frontend", new
            {
                area = string.Empty,
                name = menuName,
                templateViewName = templateViewName,
                filterByUrl = filterByUrl
            });
        }
    }

    public enum WidgetColumns : byte
    {
        Default = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4
    }

    public class KoreCMS<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal KoreCMS(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString BlogCategoryDropDownList(string name, int? selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetBlogCategorySelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString BlogCategoryDropDownListFor(Expression<Func<TModel, int>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetBlogCategorySelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public MvcHtmlString BlogTagDropDownList(string name, int? selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetBlogTagSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString BlogTagDropDownListFor(Expression<Func<TModel, int>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetBlogTagSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public MvcHtmlString ContentBlockTypesDropDownList(string name, string selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetContentBlockTypesSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString ContentBlockTypesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetContentBlockTypesSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public MvcHtmlString PageTypesDropDownList(string name, string selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetPageTypesSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString PageTypesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetPageTypesSelectList(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public MvcHtmlString TopLevelPagesDropDownList(string name, string selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetTopLevelPagesSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString TopLevelPagesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetTopLevelPagesSelectList(selectedValue, emptyText);
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

        private static IEnumerable<SelectListItem> GetBlogCategorySelectList(int? selectedValue = null, string emptyText = null)
        {
            var categoryService = EngineContext.Current.Resolve<IBlogCategoryService>();

            return categoryService.Find()
                .ToSelectList(
                    value => value.Id,
                    text => text.Name,
                    selectedValue,
                    emptyText);
        }

        private static IEnumerable<SelectListItem> GetBlogTagSelectList(int? selectedValue = null, string emptyText = null)
        {
            var tagService = EngineContext.Current.Resolve<IBlogTagService>();

            return tagService.Find()
                .ToSelectList(
                    value => value.Id,
                    text => text.Name,
                    selectedValue,
                    emptyText);
        }

        private static IEnumerable<SelectListItem> GetContentBlockTypesSelectList(string selectedValue = null, string emptyText = null)
        {
            var contentBlocks = EngineContext.Current.ResolveAll<IContentBlock>();

            var blockTypes = contentBlocks
                .Select(x => new
                {
                    x.Name,
                    Type = GetTypeFullName(x.GetType())
                })
                .OrderBy(x => x.Name)
                .ToDictionary(k => k.Name, v => v.Type);

            return blockTypes.ToSelectList(
                value => value.Value,
                text => text.Key,
                selectedValue,
                emptyText);
        }

        private static IEnumerable<SelectListItem> GetPageTypesSelectList(string selectedValue = null, string emptyText = null)
        {
            var repository = EngineContext.Current.Resolve<IRepository<PageType>>();

            using (var connection = repository.OpenConnection())
            {
                return connection.Query()
                    .OrderBy(x => x.Name)
                    .ToList()
                    .ToSelectList(
                        value => value.Id,
                        text => text.Name,
                        selectedValue,
                        emptyText);
            }
        }

        private static IEnumerable<SelectListItem> GetTopLevelPagesSelectList(string selectedValue = null, string emptyText = null)
        {
            var pageService = EngineContext.Current.Resolve<IPageService>();

            return pageService.GetTopLevelPages()
                .ToSelectList(
                    value => value.Id,
                    text => text.Name,
                    selectedValue,
                    emptyText);
        }

        private static string GetTypeFullName(Type type)
        {
            return string.Concat(type.FullName, ", ", type.Assembly.GetName().Name);
        }

        private static IEnumerable<SelectListItem> GetZonesSelectList(string selectedValue = null, string emptyText = null)
        {
            var zoneService = EngineContext.Current.Resolve<IZoneService>();

            using (var connection = zoneService.OpenConnection())
            {
                return connection.Query()
                    .OrderBy(x => x.Name)
                    .ToList()
                    .ToSelectList(
                        value => value.Id,
                        text => text.Name,
                        selectedValue,
                        emptyText);
            }
        }
    }
}
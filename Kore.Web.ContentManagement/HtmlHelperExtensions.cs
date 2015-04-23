using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Collections;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks;
using Kore.Web.ContentManagement.Areas.Admin.ContentBlocks.Services;
using Kore.Web.ContentManagement.Areas.Admin.Media.Services;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

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

        public static MvcHtmlString ContentZone(this HtmlHelper html, string zoneName)
        {
            return html.Action("ContentBlocksByZone", "Frontend", new { area = "", zoneName = zoneName });
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

    public class KoreCMS<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal KoreCMS(HtmlHelper<TModel> html)
        {
            this.html = html;
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

        public MvcHtmlString MediaFoldersDropDownList(string name, string selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetMediaFoldersSelectList(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString MediaFoldersDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetMediaFoldersSelectList(selectedValue, emptyText);
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

        private static void GetMediaFolders(IMediaService service, List<Tuple<string, string>> folders, string path, byte level = 0)
        {
            var mediaFolders = service.GetMediaFolders(path);

            foreach (var folder in mediaFolders)
            {
                if (level != 0)
                {
                    string folderName = string.Concat("---".Repeat(level), " ", folder.Name);
                    folders.Add(new Tuple<string, string>(folderName, folder.MediaPath));
                }
                else
                {
                    folders.Add(new Tuple<string, string>(folder.Name, folder.MediaPath));
                }
                GetMediaFolders(service, folders, folder.MediaPath, (byte)(level + 1));
            }
        }

        private static IEnumerable<SelectListItem> GetMediaFoldersSelectList(string selectedValue = null, string emptyText = null)
        {
            var service = EngineContext.Current.Resolve<IMediaService>();

            var folders = new List<Tuple<string, string>>();
            GetMediaFolders(service, folders, null);

            return folders
                .ToSelectList(
                    value => value.Item2,
                    text => text.Item1,
                    selectedValue,
                    emptyText);
        }

        private static IEnumerable<SelectListItem> GetPageTypesSelectList(string selectedValue = null, string emptyText = null)
        {
            var repository = EngineContext.Current.Resolve<IRepository<PageType>>();

            return repository.Table
                .OrderBy(x => x.Name)
                .ToList()
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

            return zoneService.Repository.Table
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
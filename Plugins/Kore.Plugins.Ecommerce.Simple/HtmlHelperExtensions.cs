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
using Kore.Web;
using Kore.Web.Collections;
using Kore.Web.Common.Areas.Admin.Regions.Domain;
using Kore.Web.Common.Areas.Admin.Regions.Services;
using Newtonsoft.Json.Linq;

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

        public MvcHtmlString CountryDropDownList(string name, int? selectedValue = null, string emptyText = null, object htmlAttributes = null)
        {
            var selectList = GetCountries(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString CountryDropDownListFor(Expression<Func<TModel, int>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetCountries(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetCategoriesSelectList(string selectedValue = null, string emptyText = null)
        {
            var repository = EngineContext.Current.Resolve<IRepository<SimpleCommerceCategory>>();

            return repository.Table
                .OrderBy(x => x.Name)
                .ToHashSet()
                .ToSelectList(
                    value => value.Id,
                    text => text.Name,
                    selectedValue,
                    emptyText);
        }

        private static IEnumerable<SelectListItem> GetCountries(int? selectedValue = null, string emptyText = null)
        {
            var regionService = EngineContext.Current.Resolve<IRegionService>();
            var regionSettingsService = EngineContext.Current.Resolve<IRegionSettingsService>();

            var countries = regionService.GetCountries().ToDictionary(k => k.Id, v => v);

            string settingsId = StoreRegionSettings.SettingsName.ToSlugUrl();

            var settings = regionSettingsService.Find(x =>
                x.SettingsId == settingsId &&
                countries.Keys.Contains(x.RegionId));

            // If no settings have been made for this plugin,
            if (!settings.Any())
            {
                // ... then we assume ALL countries are OK.
                return countries.Values
                    .OrderBy(x => x.Order == null)
                    .ThenBy(x => x.Order)
                    .ThenBy(x => x.Name)
                    .ToSelectList(
                        value => value.Id,
                        text => text.Name,
                        selectedValue,
                        emptyText);
            }
            else
            {
                // Else only get the countries specified
                var records = new HashSet<Region>();
                foreach (var setting in settings)
                {
                    dynamic fields = JObject.Parse(setting.Fields);
                    bool isEnabled = fields.IsEnabled;

                    if (isEnabled)
                    {
                        records.Add(countries[setting.RegionId]);
                    }
                }

                return records
                    .OrderBy(x => x.Order == null)
                    .ThenBy(x => x.Order)
                    .ThenBy(x => x.Name)
                    .ToSelectList(
                        value => value.Id,
                        text => text.Name,
                        selectedValue,
                        emptyText);
            }
        }
    }
}
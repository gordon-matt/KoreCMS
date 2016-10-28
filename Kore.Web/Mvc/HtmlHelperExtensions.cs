using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Kore.Collections;
using Kore.ComponentModel;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Security.Membership;
using Kore.Tenants.Services;
using Kore.Threading;
using Kore.Web.Collections;
using Kore.Web.Mvc.Controls;
using Kore.Web.Mvc.KoreUI;
using Kore.Web.Mvc.KoreUI.Providers;
using Kore.Web.Mvc.Themes;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Mvc
{
    public enum PageTarget : byte
    {
        Default = 0,
        Blank = 1,
        Parent = 2,
        Self = 3,
        Top = 4
    }

    public static class HtmlHelperExtensions
    {
        #region Images

        public static MvcHtmlString EmbeddedImage(this HtmlHelper helper, Assembly assembly, string resourceName, string alt, object htmlAttributes = null)
        {
            string base64 = string.Empty;
            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            using (var memoryStream = new MemoryStream())
            {
                resourceStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                base64 = Convert.ToBase64String(memoryStream.ToArray());
            }

            return Image(helper, string.Concat("data:image/jpg;base64,", base64), alt, htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string src, string alt, object htmlAttributes = null)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", urlHelper.Content(src));

            if (!string.IsNullOrEmpty(alt))
            {
                builder.MergeAttribute("alt", alt);
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ImageLink(this HtmlHelper helper, string src, string alt, string href, object aHtmlAttributes = null, object imgHtmlAttributes = null, PageTarget target = PageTarget.Default)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", href);

            switch (target)
            {
                case PageTarget.Blank: builder.MergeAttribute("target", "_blank"); break;
                case PageTarget.Parent: builder.MergeAttribute("target", "_parent"); break;
                case PageTarget.Self: builder.MergeAttribute("target", "_self"); break;
                case PageTarget.Top: builder.MergeAttribute("target", "_top"); break;
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(aHtmlAttributes));

            var img = helper.Image(src, alt, imgHtmlAttributes);

            builder.InnerHtml = img.ToString();

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Map(this HtmlHelper helper, string name, ImageMapHotSpot[] hotSpots)
        {
            return helper.Map(name, name, hotSpots);
        }

        public static MvcHtmlString Map(this HtmlHelper helper, string name, string id, ImageMapHotSpot[] hotSpots)
        {
            var map = new ImageMap
            {
                ID = id,
                Name = name,
                HotSpots = hotSpots
            };

            return new MvcHtmlString(map.ToString());
        }

        #endregion Images

        #region Html Link

        public static MvcHtmlString EmailLink(this HtmlHelper helper, string emailAddress)
        {
            return helper.Link(string.Concat("mailto:", emailAddress));
        }

        public static MvcHtmlString Link(this HtmlHelper helper, string href, PageTarget target = PageTarget.Default)
        {
            return helper.Link(href, href, target);
        }

        public static MvcHtmlString Link(this HtmlHelper helper, string linkText, string href, PageTarget target = PageTarget.Default)
        {
            return helper.Link(linkText, href, null, target);
        }

        public static MvcHtmlString Link(this HtmlHelper helper, string linkText, string href, object htmlAttributes, PageTarget target = PageTarget.Default)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", href);
            builder.InnerHtml = linkText;

            switch (target)
            {
                case PageTarget.Blank: builder.MergeAttribute("target", "_blank"); break;
                case PageTarget.Parent: builder.MergeAttribute("target", "_parent"); break;
                case PageTarget.Self: builder.MergeAttribute("target", "_self"); break;
                case PageTarget.Top: builder.MergeAttribute("target", "_top"); break;
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString Link(this HtmlHelper helper, string linkText, string href, RouteValueDictionary htmlAttributes, PageTarget target = PageTarget.Default)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", href);
            builder.InnerHtml = linkText;

            switch (target)
            {
                case PageTarget.Blank: builder.MergeAttribute("target", "_blank"); break;
                case PageTarget.Parent: builder.MergeAttribute("target", "_parent"); break;
                case PageTarget.Self: builder.MergeAttribute("target", "_self"); break;
                case PageTarget.Top: builder.MergeAttribute("target", "_top"); break;
            }

            builder.MergeAttributes(htmlAttributes);

            return MvcHtmlString.Create(builder.ToString());
        }

        #endregion Html Link

        #region NumbersDropDown

        public static MvcHtmlString NumbersDropDown(this HtmlHelper html, string name, int min, int max, int? selected = null, string emptyText = null, object htmlAttributes = null)
        {
            var items = new List<SelectListItem>();

            for (int i = min; i <= max; i++)
            {
                var item = new SelectListItem
                {
                    Text = i.ToString(CultureInfo.InvariantCulture),
                    Value = i.ToString(CultureInfo.InvariantCulture),
                    Selected = i == selected
                };
                items.Add(item);
            }

            if (emptyText != null) // we don't check for empty, because empty string can be valid for emptyText value.
            {
                items.Insert(0, new SelectListItem { Value = string.Empty, Text = emptyText });
            }

            var selectList = new SelectList(items, "Value", "Text");

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public static MvcHtmlString NumbersDropDownFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int min, int max, string emptyText = null, object htmlAttributes = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var items = new List<SelectListItem>();

            for (int i = min; i <= max; i++)
            {
                var item = new SelectListItem
                {
                    Text = i.ToString(CultureInfo.InvariantCulture),
                    Value = i.ToString(CultureInfo.InvariantCulture),
                    Selected = i.Equals(selectedValue)
                };
                items.Add(item);
            }

            if (emptyText != null) // we don't check for empty, because empty string can be valid for emptyText value.
            {
                items.Insert(0, new SelectListItem { Value = string.Empty, Text = emptyText });
            }

            var selectList = new SelectList(items, "Value", "Text");

            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        #endregion NumbersDropDown

        #region Other

        public static MvcHtmlString FileUpload(this HtmlHelper html, string name, object htmlAttributes = null)
        {
            var builder = new TagBuilder("input");
            builder.MergeAttribute("type", "file");
            builder.GenerateId(name);

            if (!string.IsNullOrEmpty(name))
            {
                builder.MergeAttribute("name", name);
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString JsonObject<TEntity>(this HtmlHelper html, string name, TEntity item)
        {
            return new MvcHtmlString(string.Format("var {0} = {1};", name, item.ToJson()));
        }

        #endregion Other

        public static MvcHtmlString CheckBoxList(
            this HtmlHelper html,
            string name,
            IEnumerable<SelectListItem> selectList,
            IEnumerable<string> selectedValues,
            byte numberOfColumns = 1,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
        {
            string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            string fullHtmlFieldId = html.ViewData.TemplateInfo.GetFullHtmlFieldId(name);

            var values = new List<string>();
            if (selectedValues != null)
            {
                values.AddRange(selectedValues);
            }

            if (selectList == null)
            {
                throw new ArgumentNullException("selectList");
            }

            int index = 0;

            var sb = new StringBuilder();

            bool groupByCategory = (selectList.First() is ExtendedSelectListItem);

            if (groupByCategory)
            {
                var items = selectList.Cast<ExtendedSelectListItem>().ToList();
                var groups = items.GroupBy(x => x.Category);

                foreach (var @group in groups)
                {
                    sb.AppendFormat(@"<label class=""checkbox-list-group-label"">{0}</label>", group.Key);

                    foreach (var item in group)
                    {
                        var isChecked = values.Contains(item.Value);

                        var tagBuilder = new FluentTagBuilder("label")
                            .MergeAttribute("for", fullHtmlFieldName)
                            .MergeAttributes(labelHtmlAttributes)
                            .StartTag("input", TagRenderMode.SelfClosing)
                                .MergeAttribute("type", "checkbox")
                                .MergeAttribute("name", fullHtmlFieldName)
                                .MergeAttribute("id", fullHtmlFieldId + "_" + index)
                                .MergeAttribute("value", item.Value);

                        if (isChecked)
                        {
                            tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
                        }

                        if (checkboxHtmlAttributes != null)
                        {
                            tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
                        }

                        tagBuilder = tagBuilder.EndTag(); //end checkbox
                        tagBuilder = tagBuilder
                            .StartTag("span")
                                .SetInnerText(item.Text)
                            .EndTag();

                        sb.Append(tagBuilder.ToString());
                        index++;
                    }
                }
            }
            else
            {
                var rows = (int)Math.Ceiling((selectList.Count() * 1d) / numberOfColumns);
                var columnWidth = (int)Math.Ceiling(12d / numberOfColumns);

                for (var i = 0; i < numberOfColumns; i++)
                {
                    var items = selectList.Skip(i * rows).Take(rows);
                    sb.AppendFormat("<div class=\"col-md-{0}\">", columnWidth);

                    foreach (var item in items)
                    {
                        var isChecked = values.Contains(item.Value);

                        var tagBuilder = new FluentTagBuilder("label")
                            .MergeAttribute("for", fullHtmlFieldName)
                            .MergeAttributes(labelHtmlAttributes)
                            .StartTag("input", TagRenderMode.SelfClosing)
                                .MergeAttribute("type", "checkbox")
                                .MergeAttribute("name", fullHtmlFieldName)
                                .MergeAttribute("id", fullHtmlFieldId + "_" + index)
                                .MergeAttribute("value", item.Value);

                        if (isChecked)
                        {
                            tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
                        }

                        if (checkboxHtmlAttributes != null)
                        {
                            tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
                        }

                        tagBuilder = tagBuilder.EndTag(); //end checkbox
                        tagBuilder = tagBuilder
                            .StartTag("span")
                                .SetInnerText(item.Text)
                            .EndTag();

                        sb.Append(tagBuilder.ToString());
                        index++;
                    }

                    sb.Append("</div>");
                }
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TProperty>>> expression,
            IEnumerable<SelectListItem> selectList,
            byte numberOfColumns = 1,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null) where TModel : class
        {
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            string fullHtmlFieldName = html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName);
            string fullHtmlFieldId = html.ViewData.TemplateInfo.GetFullHtmlFieldId(htmlFieldName);

            var func = expression.Compile();
            var selectedValues = func(html.ViewData.Model);

            var values = new List<string>();
            if (selectedValues != null)
            {
                values.AddRange(selectedValues.Select(x => Convert.ToString(x)));
            }

            if (selectList == null)
            {
                throw new ArgumentNullException("selectList");
            }

            int index = 0;

            var sb = new StringBuilder();

            bool groupByCategory = (selectList.First() is ExtendedSelectListItem);

            if (groupByCategory)
            {
                var items = selectList.Cast<ExtendedSelectListItem>().ToList();
                var groups = items.GroupBy(x => x.Category);

                foreach (var @group in groups)
                {
                    sb.AppendFormat("<strong>{0}</strong>", group.Key);

                    foreach (var item in group)
                    {
                        var isChecked = values.Contains(item.Value);

                        var tagBuilder = new FluentTagBuilder("label")
                            .MergeAttribute("for", fullHtmlFieldName)
                            .MergeAttributes(labelHtmlAttributes)
                            .StartTag("input", TagRenderMode.SelfClosing)
                                .MergeAttribute("type", "checkbox")
                                .MergeAttribute("name", fullHtmlFieldName)
                                .MergeAttribute("id", fullHtmlFieldId + "_" + index)
                                .MergeAttribute("value", item.Value);

                        if (isChecked)
                        {
                            tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
                        }

                        if (checkboxHtmlAttributes != null)
                        {
                            tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
                        }

                        tagBuilder = tagBuilder.EndTag(); //end checkbox
                        tagBuilder = tagBuilder
                            .StartTag("span")
                                .SetInnerText(item.Text)
                            .EndTag();

                        sb.Append(tagBuilder.ToString());
                        index++;
                    }
                }
            }
            else
            {
                var rows = (int)Math.Ceiling((selectList.Count() * 1d) / numberOfColumns);
                var columnWidth = (int)Math.Ceiling(12d / numberOfColumns);

                for (var i = 0; i < numberOfColumns; i++)
                {
                    var items = selectList.Skip(i * rows).Take(rows);
                    sb.AppendFormat("<div class=\"col-md-{0}\">", columnWidth);

                    foreach (var item in items)
                    {
                        var isChecked = values.Contains(item.Value);

                        var tagBuilder = new FluentTagBuilder("label")
                            .MergeAttribute("for", fullHtmlFieldName)
                            .MergeAttributes(labelHtmlAttributes)
                            .StartTag("input", TagRenderMode.SelfClosing)
                                .MergeAttribute("type", "checkbox")
                                .MergeAttribute("name", fullHtmlFieldName)
                                .MergeAttribute("id", fullHtmlFieldId + "_" + index)
                                .MergeAttribute("value", item.Value);

                        if (isChecked)
                        {
                            tagBuilder = tagBuilder.MergeAttribute("checked", "checked");
                        }

                        if (checkboxHtmlAttributes != null)
                        {
                            tagBuilder = tagBuilder.MergeAttributes(checkboxHtmlAttributes);
                        }

                        tagBuilder = tagBuilder.EndTag(); //end checkbox
                        tagBuilder = tagBuilder
                            .StartTag("span")
                                .SetInnerText(item.Text)
                            .EndTag();

                        sb.Append(tagBuilder.ToString());
                        index++;
                    }

                    sb.Append("</div>");
                }
            }

            return new MvcHtmlString(sb.ToString());
        }

        public static MvcHtmlString CulturesDropDownList<TModel>(this HtmlHelper<TModel> html, string name, string selectedValue = null, object htmlAttributes = null, string emptyText = null)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var selectList = cultures
                .OrderBy(x => x.DisplayName)
                .ToSelectList(
                    value => value.Name,
                    text => text.DisplayName,
                    selectedValue,
                    emptyText);

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public static MvcHtmlString CulturesDropDownListFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = cultures
                .OrderBy(x => x.DisplayName)
                .ToSelectList(
                    value => value.Name,
                    text => text.DisplayName,
                    selectedValue,
                    emptyText);

            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        //public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, string emptyText = null, object htmlAttributes = null) where TEnum : struct
        //{
        //    var func = expression.Compile();
        //    var selectedValue = func(html.ViewData.Model);

        //    var selectList = EnumExtensions.ToSelectList<TEnum>(selectedValue, emptyText);
        //    return html.DropDownListFor(expression, selectList, htmlAttributes);
        //}

        public static MvcHtmlString EnumMultiDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, IEnumerable<TEnum>>> expression, string emptyText = null, object htmlAttributes = null) where TEnum : struct
        {
            var func = expression.Compile();
            var selectedValues = func(html.ViewData.Model);

            var parsedSelectedValues = Enumerable.Empty<long>();

            if (selectedValues != null)
            {
                parsedSelectedValues = selectedValues.Select(x => Convert.ToInt64(x));
            }

            var multiSelectList = EnumExtensions.ToMultiSelectList<TEnum>(parsedSelectedValues, emptyText);

            return html.ListBoxFor(expression, multiSelectList, htmlAttributes);
        }

        public static MvcHtmlString HelpText(this HtmlHelper html, string helpText, object htmlAttributes = null)
        {
            var tagBuilder = new FluentTagBuilder("p")
                .AddCssClass("help-block")
                .MergeAttributes(htmlAttributes)
                .SetInnerHtml(helpText);

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString HelpTextFor<TModel, TProperty>(this HtmlHelper<TModel> html, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var memberExpression = expression.Body as MemberExpression;
            var propertyInfo = (memberExpression.Member as PropertyInfo);
            var attribute = propertyInfo.GetCustomAttributes().OfType<LocalizedHelpTextAttribute>().FirstOrDefault();

            if (attribute == null)
            {
                return MvcHtmlString.Empty;
            }

            var tagBuilder = new FluentTagBuilder("p")
                .AddCssClass("help-block")
                .MergeAttributes(htmlAttributes)
                .SetInnerHtml(attribute.HelpText);

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static Kore<TModel> Kore<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new Kore<TModel>(html);
        }

        public static KoreUI<TModel> KoreUI<TModel>(this HtmlHelper<TModel> htmlHelper, IKoreUIProvider provider = null)
        {
            if (provider != null)
            {
                return new KoreUI<TModel>(htmlHelper, provider);
            }

            string areaName = (string)htmlHelper.ViewContext.RouteData.DataTokens["area"];
            if (!string.IsNullOrEmpty(areaName) && KoreUISettings.AreaUIProviders.ContainsKey(areaName))
            {
                return new KoreUI<TModel>(htmlHelper, KoreUISettings.AreaUIProviders[areaName]);
            }
            return new KoreUI<TModel>(htmlHelper);
        }

        //TODO: Test
        public static MvcHtmlString Table<T>(this HtmlHelper html, IEnumerable<T> items, object htmlAttributes = null)
        {
            var builder = new FluentTagBuilder("table")
                .MergeAttributes(htmlAttributes)
                .StartTag("thead")
                    .StartTag("tr");

            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                builder = builder.StartTag("th").SetInnerText(property.Name).EndTag();
            }

            builder
                .EndTag() // </tr>
                .EndTag() // </thead>
                .StartTag("tbody");

            foreach (var item in items)
            {
                builder = builder.StartTag("tr");
                foreach (var property in properties)
                {
                    string value = property.GetValue(item).ToString();
                    builder = builder.StartTag("td").SetInnerText(value).EndTag();
                }
                builder = builder.EndTag(); // </tr>
            }
            builder = builder.EndTag(); // </tbody>

            return MvcHtmlString.Create(builder.ToString());
        }

        /// <summary>
        /// Create an HTML tree from a recursive collection of items
        /// </summary>
        public static TreeView<T> TreeView<T>(this HtmlHelper html, IEnumerable<T> items)
        {
            return new TreeView<T>(html, items);
        }
    }

    public class Kore<TModel>
        where TModel : class
    {
        private readonly HtmlHelper<TModel> html;

        internal Kore(HtmlHelper<TModel> html)
        {
            this.html = html;
        }

        public MvcHtmlString LanguagesDropDownList(string name, string selectedValue = null, object htmlAttributes = null, string emptyText = null, bool includeInvariant = false, string invariantText = null)
        {
            var selectList = GetLanguages(selectedValue, emptyText);
            return html.DropDownList(name, selectList, htmlAttributes);
        }

        /// <summary>
        /// Returns an HTML select element populated with the languages currently specified in the admin area as active
        /// </summary>
        /// <param name="expression"> An expression that identifies the property to use. This property should contain a culture code value</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns></returns>
        public MvcHtmlString LanguagesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null, bool includeInvariant = false, string invariantText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = GetLanguages(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        public MvcHtmlString PermissionsCheckBoxList(
            string name,
            IEnumerable<string> selectedPermissionIds,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            var permissionProviders = EngineContext.Current.ResolveAll<IPermissionProvider>();
            var permissions = permissionProviders.SelectMany(x => x.GetPermissions()).ToList();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var allPermissions = AsyncHelper.RunSync(() => membershipService.GetAllPermissions(workContext.CurrentTenant.Id)).ToHashSet();
            var T = LocalizationUtilities.Resolve();

            #region First check if all permissions are in the DB

            foreach (var permission in permissions)
            {
                if (!allPermissions.Any(x => x.Name == permission.Name))
                {
                    var newPermission = new KorePermission
                    {
                        Name = permission.Name,
                        Category = string.IsNullOrEmpty(permission.Category) ? T(KoreWebLocalizableStrings.General.Miscellaneous) : permission.Category,
                        Description = permission.Description
                    };
                    membershipService.InsertPermission(workContext.CurrentTenant.Id, newPermission);
                    allPermissions.Add(newPermission);
                }
            }

            #endregion First check if all permissions are in the DB

            var selectList = new List<ExtendedSelectListItem>();
            foreach (var categoryGroup in allPermissions.OrderBy(x => x.Category, new PermissionComparer(StringComparer.InvariantCultureIgnoreCase)).GroupBy(x => x.Category))
            {
                selectList.AddRange(categoryGroup.OrderBy(x => x.Description)
                    .Select(permission => new ExtendedSelectListItem
                    {
                        Category = permission.Category,
                        Text = permission.Description,
                        Value = permission.Id
                    }));
            }

            return html.CheckBoxList(
                name,
                selectList,
                selectedPermissionIds,
                labelHtmlAttributes: labelHtmlAttributes,
                checkboxHtmlAttributes: checkboxHtmlAttributes);
        }

        public MvcHtmlString RolesCheckBoxList(
            string name,
            IEnumerable<string> selectedRoleIds,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var selectList = AsyncHelper.RunSync(() => membershipService.GetAllRoles(workContext.CurrentTenant.Id))
                .ToSelectList(
                    value => value.Id,
                    text => text.Name);

            return html.CheckBoxList(name, selectList, selectedRoleIds, labelHtmlAttributes: labelHtmlAttributes, checkboxHtmlAttributes: checkboxHtmlAttributes);
        }

        public MvcHtmlString RolesDropDownList(string name, string selectedValue = null, object htmlAttributes = null, string emptyText = null)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var selectList = AsyncHelper.RunSync(() => membershipService.GetAllRoles(workContext.CurrentTenant.Id))
                .ToSelectList(
                    value => value.Id,
                    text => text.Name,
                    selectedValue,
                    emptyText);

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public MvcHtmlString RolesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var membershipService = EngineContext.Current.Resolve<IMembershipService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var selectList = AsyncHelper.RunSync(() => membershipService.GetAllRoles(workContext.CurrentTenant.Id))
                .ToSelectList(
                    value => value.Id,
                    text => text.Name,
                    selectedValue,
                    emptyText);

            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        /// <summary>
        /// Returns an HTML select element populated with the themes currently available
        /// </summary>
        /// <param name="expression"> An expression that identifies the property to use. This property should contain a theme name.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns></returns>
        public MvcHtmlString ThemesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var themeProvider = EngineContext.Current.Resolve<IThemeProvider>();
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = themeProvider.GetThemeConfigurations()
                .ToSelectList(
                    value => value.ThemeName,
                    text => text.ThemeName,
                    selectedValue,
                    emptyText);

            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }




        public MvcHtmlString TenantsCheckBoxList(
            string name,
            IEnumerable<string> selectedTenantIds,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
        {
            var service = EngineContext.Current.Resolve<ITenantService>();

            var selectList = GetTenantsMultiSelectList(selectedTenantIds);

            return html.CheckBoxList(name, selectList, selectedTenantIds, labelHtmlAttributes: labelHtmlAttributes, checkboxHtmlAttributes: checkboxHtmlAttributes);
        }

        private static IEnumerable<SelectListItem> GetTenantsMultiSelectList(IEnumerable<string> selectedValues = null, string emptyText = null)
        {
            var service = EngineContext.Current.Resolve<ITenantService>();

            using (var connection = service.OpenConnection())
            {
                return connection.Query()
                    .OrderBy(x => x.Name)
                    .ToList()
                    .ToMultiSelectList(
                        value => value.Id,
                        text => text.Name,
                        selectedValues,
                        emptyText);
            }
        }


        private static IEnumerable<SelectListItem> GetLanguages(string selectedValue = null, string emptyText = null, bool includeInvariant = false, string invariantText = null)
        {
            var languageManager = EngineContext.Current.Resolve<ILanguageManager>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var languages = languageManager.GetActiveLanguages(workContext.CurrentTenant.Id).ToList();

            if (includeInvariant)
            {
                if (string.IsNullOrEmpty(invariantText))
                {
                    languages.Insert(0, new Language { CultureCode = null, Name = "[ Invariant ]" });
                }
                else
                {
                    languages.Insert(0, new Language { CultureCode = null, Name = invariantText });
                }
            }

            return languages
                .ToSelectList(
                    value => value.CultureCode,
                    text => text.Name,
                    selectedValue,
                    emptyText);
        }

        private class PermissionComparer : IComparer<string>
        {
            private readonly IComparer<string> baseComparer;

            public PermissionComparer(IComparer<string> baseComparer)
            {
                this.baseComparer = baseComparer;
            }

            public int Compare(string x, string y)
            {
                var value = String.Compare(x, y, StringComparison.Ordinal);

                if (value == 0)
                {
                    return 0;
                }

                if (baseComparer.Compare(x, "System") == 0)
                {
                    return -1;
                }

                if (baseComparer.Compare(y, "System") == 0)
                {
                    return 1;
                }

                return value;
            }
        }
    }
}
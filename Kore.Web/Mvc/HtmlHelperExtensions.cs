using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Security.Membership;
using Kore.Web.Collections;
using Kore.Web.Mvc.Controls;
using Kore.Web.Mvc.RoboUI;
using Kore.Web.Mvc.Themes;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Mvc
{
    public enum PageTarget
    {
        Default = 0,
        Blank,
        Parent,
        Self,
        Top
    }

    public static class HtmlHelperExtensions
    {
        #region Images

        public static MvcHtmlString Image(this HtmlHelper helper, string src, string alt)
        {
            return Image(helper, null, src, alt, null);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string name, string src, string alt)
        {
            return Image(helper, name, src, alt, null);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string name, string src, string alt, object htmlAttributes)
        {
            return helper.Image(name, src, alt, null, htmlAttributes);
        }

        public static MvcHtmlString Image(this HtmlHelper helper, string name, string src, string alt, string usemap, object htmlAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
            var builder = new TagBuilder("img");
            builder.GenerateId(name);
            builder.MergeAttribute("src", urlHelper.Content(src));

            if (!string.IsNullOrEmpty(alt))
            {
                builder.MergeAttribute("alt", alt);
            }
            if (!string.IsNullOrEmpty(name))
            {
                builder.MergeAttribute("name", name);
            }
            if (!string.IsNullOrEmpty(usemap))
            {
                builder.MergeAttribute("usemap", usemap);
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));

            return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        public static MvcHtmlString ImageLink(this HtmlHelper helper, string src, string alt, string href, PageTarget target = PageTarget.Default)
        {
            return helper.ImageLink(null, src, alt, href, null, null, target);
        }

        public static MvcHtmlString ImageLink(this HtmlHelper helper, string name, string src, string alt, string href, PageTarget target = PageTarget.Default)
        {
            return helper.ImageLink(name, src, alt, href, null, null, target);
        }

        public static MvcHtmlString ImageLink(this HtmlHelper helper, string name, string src, string alt, string href, object aHtmlAttributes, object imgHtmlAttributes, PageTarget target = PageTarget.Default)
        {
            var builder = new TagBuilder("a");
            builder.MergeAttribute("href", href);
            builder.GenerateId(name);

            if (!string.IsNullOrEmpty(name))
            {
                builder.MergeAttribute("name", name);
            }

            switch (target)
            {
                case PageTarget.Blank: builder.MergeAttribute("target", "_blank"); break;
                case PageTarget.Parent: builder.MergeAttribute("target", "_parent"); break;
                case PageTarget.Self: builder.MergeAttribute("target", "_self"); break;
                case PageTarget.Top: builder.MergeAttribute("target", "_top"); break;
            }

            builder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(aHtmlAttributes));

            var img = helper.Image(name + "Image", src, alt, imgHtmlAttributes);

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

        //TODO: Review if need this. I think HTML5 has some number or range input type now to replace this.

        public static MvcHtmlString NumbersDropDown(this HtmlHelper html, string name, int min, int max)
        {
            return html.NumbersDropDown(name, min, max, min, null);
        }

        public static MvcHtmlString NumbersDropDown(this HtmlHelper html, string name, int min, int max, int selected)
        {
            return html.NumbersDropDown(name, min, max, selected, null);
        }

        public static MvcHtmlString NumbersDropDown(this HtmlHelper html, string name, int min, int max, int selected, object htmlAttributes)
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
            var selectList = new SelectList(items, "Value", "Text");

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        public static MvcHtmlString NumbersDropDownFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int min, int max)
        {
            return html.NumbersDropDownFor(expression, min, max, null);
        }

        public static MvcHtmlString NumbersDropDownFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, int min, int max, object htmlAttributes)
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
            var selectList = new SelectList(items, "Value", "Text");

            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

        #endregion NumbersDropDown

        #region Other

        public static MvcHtmlString FileUpload(this HtmlHelper html, string name, object htmlAttributes)
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
            bool groupByCategory = false,
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

        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, IEnumerable<TProperty>>> expression,
            IEnumerable<SelectListItem> selectList,
            bool groupByCategory = false,
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

        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> html, Expression<Func<TModel, TEnum>> expression, string emptyText = null, object htmlAttributes = null) where TEnum : struct
        {
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = EnumExtensions.ToSelectList<TEnum>(selectedValue, emptyText);
            return html.DropDownListFor(expression, selectList, htmlAttributes);
        }

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

        public static Kore<TModel> Kore<TModel>(this HtmlHelper<TModel> html) where TModel : class
        {
            return new Kore<TModel>(html);
        }

        public static RoboUIFormResult<TModel> RoboForm<TModel>(this HtmlHelper htmlHelper, TModel model) where TModel : class
        {
            return new RoboUIFormResult<TModel>(model, htmlHelper.ViewContext);
        }

        public static RoboUIGridResult<TModel> RoboGrid<TModel>(this HtmlHelper htmlHelper, TModel model) where TModel : class
        {
            return new RoboUIGridResult<TModel>(htmlHelper.ViewContext);
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

        public MvcHtmlString LanguagesDropDownList(string name, string selectedValue = null, object htmlAttributes = null, string emptyText = null)
        {
            var languageManager = EngineContext.Current.Resolve<ILanguageManager>();

            var selectList = languageManager.GetActiveLanguages()
                .ToSelectList(
                    value => value.CultureCode,
                    text => text.Name,
                    selectedValue,
                    emptyText);

            return html.DropDownList(name, selectList, htmlAttributes);
        }

        /// <summary>
        /// Returns an HTML select element populated with the languages currently specified in the admin area as active
        /// </summary>
        /// <param name="expression"> An expression that identifies the property to use. This property should contain a culture code value</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns></returns>
        public MvcHtmlString LanguagesDropDownListFor(Expression<Func<TModel, string>> expression, object htmlAttributes = null, string emptyText = null)
        {
            var languageManager = EngineContext.Current.Resolve<ILanguageManager>();
            var func = expression.Compile();
            var selectedValue = func(html.ViewData.Model);

            var selectList = languageManager.GetActiveLanguages()
                .ToSelectList(
                    value => value.CultureCode,
                    text => text.Name,
                    selectedValue,
                    emptyText);

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
            var allPermissions = membershipService.GetAllPermissions().ToList();
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
                    membershipService.InsertPermission(newPermission);
                    allPermissions.Add(newPermission);
                }
            }

            #endregion

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
                groupByCategory: true,
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

            var selectList = membershipService.GetAllRoles()
                .ToSelectList(
                    value => value.Id,
                    text => text.Name);

            return html.CheckBoxList(name, selectList, selectedRoleIds, labelHtmlAttributes: labelHtmlAttributes, checkboxHtmlAttributes: checkboxHtmlAttributes);
        }

        public MvcHtmlString RolesDropDownList(string name, string selectedValue = null, object htmlAttributes = null, string emptyText = null)
        {
            var membershipService = EngineContext.Current.Resolve<IMembershipService>();

            var selectList = membershipService.GetAllRoles()
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

            var selectList = membershipService.GetAllRoles()
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
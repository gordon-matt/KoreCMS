using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Web.Mvc.Controls;
using Kore.Web.Mvc.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Mvc.RoboUI.Providers
{
    public class Bootstrap3RoboUIFormProvider : BaseRoboUIFormProvider
    {
        private const string ControlCssClass = "form-control";
        private const string ControlsCssClass = "col-lg-9 col-sm-9 col-md-9 col-xs-9";
        private const string FormGroupCssClass = "form-group";
        private const string LabelCssClass = "col-lg-3 col-sm-3 col-md-3 col-xs-3 control-label";

        public Localizer localizer;

        private Localizer T
        {
            get
            {
                if (localizer == null)
                {
                    localizer = LocalizationUtilities.Resolve();
                }
                return localizer;
            }
        }

        #region IRoboUIFormProvider Members

        public override string GetButtonSizeCssClass(ButtonSize buttonSize)
        {
            switch (buttonSize)
            {
                case ButtonSize.Default: return "btn";
                case ButtonSize.Large: return "btn btn-lg";
                case ButtonSize.Small: return "btn btn-sm";
                case ButtonSize.ExtraSmall: return "btn btn-xs";
                default:
                    throw new ArgumentOutOfRangeException("buttonSize");
            }
        }

        public override string GetButtonStyleCssClass(ButtonStyle buttonStyle)
        {
            switch (buttonStyle)
            {
                case ButtonStyle.Default: return "btn-default";
                case ButtonStyle.Primary: return "btn-primary";
                case ButtonStyle.Info: return "btn-info";
                case ButtonStyle.Success: return "btn-success";
                case ButtonStyle.Warning: return "btn-warning";
                case ButtonStyle.Danger: return "btn-danger";
                case ButtonStyle.Inverse: return "btn-inverse";
                case ButtonStyle.Link: return "btn-link";
                default:
                    throw new ArgumentOutOfRangeException("buttonStyle");
            }
        }

        public override string RenderAction(HtmlHelper htmlHelper, RoboUIFormAction action)
        {
            if (action.HtmlBuilder != null)
            {
                return action.HtmlBuilder();
            }

            if (action.MenuItems.Count > 0)
            {
                var sb = new StringBuilder();

                sb.AppendFormat("<button data-toggle=\"dropdown\" class=\"{0} dropdown-toggle\">",
                    string.IsNullOrEmpty(action.CssClass) ? "btn btn-default" : action.CssClass.Trim());

                sb.Append(action.Text);
                sb.Append("&nbsp;<span class=\"caret\"></span>");
                sb.AppendFormat("</button>");

                sb.Append("<ul class=\"dropdown-menu\">");
                foreach (var childAction in action.MenuItems)
                {
                    sb.Append(childAction);
                }
                sb.Append("</ul>");

                return sb.ToString();
            }

            if (action.IsSubmitButton)
            {
                var attributes = new RouteValueDictionary();

                if (!action.HtmlAttributes.IsNullOrEmpty())
                {
                    foreach (var attribute in action.HtmlAttributes)
                    {
                        attributes.Add(attribute.Key, attribute.Value);
                    }
                }

                var cssClass = (GetButtonSizeCssClass(action.ButtonSize) + " " + GetButtonStyleCssClass(action.ButtonStyle) + " " + action.CssClass + (!action.IsValidationSupported ? " cancel" : "")).Trim();

                if (!string.IsNullOrEmpty(cssClass))
                {
                    attributes.Add("class", cssClass);
                }

                if (!string.IsNullOrEmpty(action.ClientId))
                {
                    attributes.Add("id", action.ClientId);
                }

                if (!string.IsNullOrEmpty(action.ConfirmMessage))
                {
                    attributes.Add("onclick", string.Format("return confirm('{0}');", action.ConfirmMessage));
                }

                if (!string.IsNullOrEmpty(action.ClientClickCode))
                {
                    attributes["onclick"] = action.ClientClickCode;
                }

                var tagBuilder = new TagBuilder("button") { InnerHtml = action.Text };
                tagBuilder.MergeAttribute("type", "submit");
                tagBuilder.MergeAttribute("value", action.Value);
                tagBuilder.MergeAttribute("name", action.Name);
                tagBuilder.MergeAttribute("id", "btn" + action.Name);
                tagBuilder.MergeAttribute("title", action.Description ?? action.Text);
                tagBuilder.MergeAttributes(attributes);

                if (!string.IsNullOrEmpty(action.IconCssClass))
                {
                    var icon = new TagBuilder("i");
                    icon.AddCssClass(action.IconCssClass);

                    tagBuilder.InnerHtml = string.Concat(icon.ToString(), " ", action.Text);
                }

                return tagBuilder.ToString(TagRenderMode.Normal);
            }
            else
            {
                var attributes = new RouteValueDictionary();

                if (!action.HtmlAttributes.IsNullOrEmpty())
                {
                    foreach (var attribute in action.HtmlAttributes)
                    {
                        attributes.Add(attribute.Key, attribute.Value);
                    }
                }

                var cssClass = (GetButtonSizeCssClass(action.ButtonSize) + " " + GetButtonStyleCssClass(action.ButtonStyle) + " " + action.CssClass + (!action.IsValidationSupported ? " cancel" : "")).Trim();
                if (!string.IsNullOrEmpty(cssClass))
                {
                    attributes.Add("class", cssClass);
                }

                if (!string.IsNullOrEmpty(action.ClientId))
                {
                    attributes.Add("id", action.ClientId);
                }

                if (!string.IsNullOrEmpty(action.ConfirmMessage))
                {
                    attributes.Add("onclick", string.Format("return confirm('{0}');", action.ConfirmMessage));
                }

                if (!string.IsNullOrEmpty(action.ClientClickCode))
                {
                    attributes["onclick"] = action.ClientClickCode;
                }

                attributes["href"] = action.Url;

                if (action.IsShowModalDialog)
                {
                    attributes.Add("data-toggle", "fancybox");
                    attributes.Add("data-fancybox-type", "iframe");
                    attributes.Add("data-fancybox-width", action.ModalDialogWidth);
                }

                var tagBuilder = new TagBuilder("a") { InnerHtml = action.Text };
                tagBuilder.MergeAttributes(attributes);

                if (!string.IsNullOrEmpty(action.IconCssClass))
                {
                    var icon = new TagBuilder("i");
                    icon.AddCssClass(action.IconCssClass);

                    tagBuilder.InnerHtml = string.Concat(icon.ToString(), " ", action.Text);
                }

                return tagBuilder.ToString(TagRenderMode.Normal);
            }
        }

        public override string RenderActions(HtmlHelper htmlHelper, IEnumerable<RoboUIFormAction> actions)
        {
            if (!actions.Any())
            {
                return string.Empty;
            }

            var sb = new StringBuilder(256);

            sb.Append("<div class=\"form-group\"><div class=\"btn-toolbar\">");

            foreach (var action in actions)
            {
                sb.Append("<div class=\"btn-group\">");
                sb.Append(RenderAction(htmlHelper, action));
                sb.Append("</div>");
            }

            sb.Append("</div></div>");

            return sb.ToString();
        }

        public override string RenderForm<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm)
        {
            var sb = new StringBuilder(2048);

            var formId = string.IsNullOrWhiteSpace(roboForm.FormId)
                ? "robo-form-" + Guid.NewGuid().ToString("N").ToLowerInvariant()
                : roboForm.FormId;

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var scriptRegister = new ScriptRegister(workContext);

            Write(sb, string.IsNullOrEmpty(roboForm.CssClass)
                    ? "<div class=\"robo-form-container\">"
                    : string.Format("<div class=\"robo-form-container {0}\">", roboForm.CssClass));

            if (!roboForm.DisableGenerateForm)
            {
                string form = BeginForm(htmlHelper, roboForm, formId);
                Write(sb, form);
            }

            if (!string.IsNullOrEmpty(roboForm.FormWrapperStartHtml))
            {
                Write(sb, roboForm.FormWrapperStartHtml);
            }

            #region Buttons

            var htmlActions = new List<string>();

            if (roboForm.ShowSubmitButton && !roboForm.ReadOnly)
            {
                var tagBuilder = new TagBuilder("button")
                {
                    InnerHtml = roboForm.SubmitButtonText
                };

                tagBuilder.MergeAttribute("type", "submit");
                tagBuilder.MergeAttribute("name", "Save");
                tagBuilder.AddCssClass(GetButtonSizeCssClass(ButtonSize.Default));
                tagBuilder.AddCssClass(GetButtonStyleCssClass(ButtonStyle.Primary));
                tagBuilder.MergeAttributes(roboForm.SubmitButtonHtmlAttributes);

                htmlActions.Add(tagBuilder.ToString(TagRenderMode.Normal));
            }

            htmlActions.AddRange(roboForm.Actions.Select(x => RenderAction(htmlHelper, x)));

            if (roboForm.ShowCancelButton)
            {
                if (!string.IsNullOrEmpty(roboForm.CancelButtonUrl))
                {
                    var tagBuilder = new TagBuilder("a")
                    {
                        InnerHtml = roboForm.CancelButtonText
                    };

                    tagBuilder.MergeAttribute("href", roboForm.CancelButtonUrl);
                    tagBuilder.AddCssClass(GetButtonSizeCssClass(ButtonSize.Default));
                    tagBuilder.AddCssClass(GetButtonStyleCssClass(ButtonStyle.Default));
                    tagBuilder.MergeAttributes(roboForm.CancelButtonHtmlAttributes);
                    htmlActions.Add(tagBuilder.ToString(TagRenderMode.Normal));
                }
                else
                {
                    var tagBuilder = new TagBuilder("button")
                    {
                        InnerHtml = roboForm.CancelButtonText
                    };

                    tagBuilder.MergeAttribute("type", "button");
                    tagBuilder.MergeAttribute("name", "Cancel");
                    tagBuilder.MergeAttribute("onclick", "if(self != top){ parent.jQuery.fancybox.close(); }else{ history.back(); }");
                    tagBuilder.AddCssClass(GetButtonSizeCssClass(ButtonSize.Default));
                    tagBuilder.AddCssClass(GetButtonStyleCssClass(ButtonStyle.Default));
                    tagBuilder.MergeAttributes(roboForm.CancelButtonHtmlAttributes);
                    htmlActions.Add(tagBuilder.ToString(TagRenderMode.Normal));
                }
            }

            #endregion Buttons

            var properties = roboForm.GetProperties();

            if (!properties.Any())
            {
                return sb.ToString();
            }

            foreach (var property in properties)
            {
                property.IsReadOnly = property.IsReadOnly || roboForm.ReadOnlyProperties.Contains(property.Name);
            }

            switch (roboForm.Layout)
            {
                #region RoboFormLayout.Grid

                case RoboUIFormLayout.Grid:
                    {
                        var gridLayoutColumns = Math.Max(
                            roboForm.GridLayouts.Max(x => x.Value.ColumnSpan),
                            roboForm.GridLayouts.Max(x => x.Value.Column) + 1);

                        var gridLayoutRows = roboForm.GridLayouts.Max(x => x.Value.Row) + 1;

                        Write(sb, "<div class=\"box\">");

                        if (roboForm.ShowBoxHeader)
                        {
                            Write(sb, string.Format(
                            "<div class=\"box-header\">{1}<span class=\"title\">{0}</span></div>",
                            roboForm.Title,
                            roboForm.ShowAsModalDialog && roboForm.ShowCloseButton
                                ? "<i class=\"kore-icon kore-icon-close\" onclick=\"window.parent.fancyboxResult = null; parent.jQuery.fancybox.close();\" style=\"cursor:pointer\"></i>"
                                : string.Empty));
                        }

                        Write(sb, "<div class=\"box-content\">");

                        if (roboForm.ShowValidationSummary)
                        {
                            Write(sb, string.Format(
                                "<div data-valmsg-summary=\"true\" class=\"validation-summary\"><span>{0}</span><ul></ul></div>",
                                roboForm.ValidationSummary));
                        }

                        Write(sb, "<table style=\"width: 100%;\">");

                        var columnWidth = 100 / gridLayoutColumns;
                        Write(sb, "<colgroup>");
                        for (int i = 0; i < gridLayoutColumns; i++)
                        {
                            Write(sb, string.Format("<col style=\"width: {0}%\">", columnWidth));
                        }
                        Write(sb, "</colgroup>");

                        for (var r = 0; r < gridLayoutRows; r++)
                        {
                            var controlsInRow = roboForm.GridLayouts.Where(x => x.Value.Row == r).ToList();
                            if (controlsInRow.Count == 0)
                            {
                                continue;
                            }

                            Write(sb, "<tr>");
                            var maxColSpan = 1;

                            for (var c = 0; c < gridLayoutColumns; c++)
                            {
                                if (maxColSpan > 1)
                                {
                                    if (c + 1 <= maxColSpan)
                                    {
                                        continue;
                                    }
                                }

                                // Calc row span
                                var cells = roboForm.GridLayouts.Values.Where(x => DetectSpanCells(x, r, c)).ToList();
                                if (cells.Count > 0)
                                {
                                    continue;
                                }

                                var col = c;
                                var controlsInCol = controlsInRow.Where(x => x.Value.Column == col).ToList();
                                if (controlsInCol.Count == 0)
                                {
                                    Write(sb, "<td></td>");
                                    continue;
                                }

                                maxColSpan = controlsInCol.Max(x => x.Value.ColumnSpan);

                                Write(sb, "<td style=\"vertical-align: top;\"");

                                if (maxColSpan > 1)
                                {
                                    Write(sb, string.Format(" colspan=\"{0}\"", maxColSpan));
                                }

                                var rowSpan = controlsInCol.Max(x => x.Value.RowSpan);
                                if (rowSpan > 1)
                                {
                                    Write(sb, string.Format(" rowspan=\"{0}\"", rowSpan));
                                }

                                Write(sb, ">");

                                foreach (var control in controlsInCol)
                                {
                                    var property = properties.FirstOrDefault(x => x.Name == control.Key);
                                    if (property == null)
                                    {
                                        continue;
                                    }

                                    if (property is RoboHiddenAttribute)
                                    {
                                        Write(sb, RenderControl(htmlHelper, roboForm, property));
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(property.ControlSpan))
                                        {
                                            continue;
                                        }

                                        var spanControls = properties.Where(x => x.ControlSpan == property.Name).Select(x => RenderControl(htmlHelper, roboForm, x)).ToList();
                                        spanControls.Insert(0, RenderControl(htmlHelper, roboForm, property));

                                        Write(sb, property, spanControls.ToArray());
                                    }
                                }

                                Write(sb, "</td>");
                            }

                            Write(sb, "</tr>");
                        }

                        Write(sb, "</table>");

                        Write(null, htmlActions.ToArray());

                        Write(sb, "</div></div>");
                    }
                    break;

                #endregion RoboFormLayout.Grid

                #region RoboFormLayout.Grouped

                case RoboUIFormLayout.Grouped:
                    {
                        foreach (var groupedLayout in roboForm.GroupedLayouts)
                        {
                            if (!string.IsNullOrEmpty(groupedLayout.FormGroupWrapperStartHtml))
                            {
                                Write(sb, groupedLayout.FormGroupWrapperStartHtml);
                            }

                            Write(sb, string.Format("<div class=\"box {0}\">", groupedLayout.CssClass));

                            if (roboForm.ShowBoxHeader)
                            {
                                Write(sb, string.Format("<div class=\"box-header\"><span class=\"title\">{0}</span></div>", groupedLayout.Title));
                            }

                            Write(sb, groupedLayout.EnableScrollbar
                                ? "<div class=\"box-content\" style=\"overflow: auto;\">"
                                : "<div class=\"box-content\">");

                            var groupedLayoutColumns = groupedLayout.Column;

                            if (groupedLayoutColumns == 0)
                            {
                                groupedLayoutColumns = 1;
                            }

                            var groupedLayoutRows = (int)Math.Ceiling((double)groupedLayout.Properties.Count / groupedLayoutColumns);

                            // Render hidden fields
                            foreach (var property in properties.Where(x => !roboForm.ExcludedProperties.Contains(x.Name) && x is RoboHiddenAttribute))
                            {
                                Write(sb, RenderControl(htmlHelper, roboForm, property));
                            }

                            if (groupedLayout.EnableGrid)
                            {
                                Write(sb, "<table style=\"width: 100%;\">");

                                var columnWith = 100 / groupedLayoutColumns;

                                Write(sb, "<colgroup>");

                                for (int i = 0; i < groupedLayoutColumns; i++)
                                {
                                    Write(sb, string.Format("<col style=\"width: {0}%\">", columnWith));
                                }

                                Write(sb, "</colgroup>");

                                var index = 0;

                                for (var i = 0; i < groupedLayoutRows; i++)
                                {
                                    Write(sb, "<tr>");

                                    for (var j = 0; j < groupedLayoutColumns; j++)
                                    {
                                        if (index == groupedLayout.Properties.Count)
                                        {
                                            continue;
                                        }
                                        var propertyName = groupedLayout.Properties.ElementAt(index);

                                        if (roboForm.ExcludedProperties.Contains(propertyName))
                                        {
                                            Write(sb, "<td></td>");
                                            continue;
                                        }

                                        var property = properties.First(x => x.Name == propertyName);

                                        if (!string.IsNullOrEmpty(property.ControlSpan))
                                        {
                                            continue;
                                        }

                                        var spanControls = properties.Where(x => x.ControlSpan == propertyName).Select(x => RenderControl(htmlHelper, roboForm, x)).ToList();
                                        spanControls.Insert(0, RenderControl(htmlHelper, roboForm, property));

                                        if (property is RoboHiddenAttribute)
                                        {
                                            Write(sb, property, spanControls.ToArray());
                                            index++;
                                            j--;
                                            continue;
                                        }

                                        Write(sb, "<td>");

                                        Write(sb, property, spanControls.ToArray());

                                        Write(sb, "</td>");
                                        index++;
                                    }

                                    Write(sb, "</tr>");
                                }

                                Write(sb, "</table>");
                            }
                            else
                            {
                                var groupedLayoutProperties = groupedLayout.Properties.Select(x => properties.First(y => y.Name == x)).ToList();
                                var max = groupedLayoutProperties.Max(x => x.ContainerRowIndex);

                                if (max != -100)
                                {
                                    for (var i = 0; i <= max; i++)
                                    {
                                        var propertiesInRow =
                                            groupedLayoutProperties.Where(
                                                x => x.ContainerRowIndex == i && !roboForm.ExcludedProperties.Contains(x.Name))
                                                .ToList();
                                        if (!propertiesInRow.Any())
                                        {
                                            continue;
                                        }

                                        Write(sb, "<div class=\"row\">");

                                        foreach (var property in propertiesInRow)
                                        {
                                            Write(sb, roboForm.Localizer, property, RenderControl(htmlHelper, roboForm, property));
                                        }

                                        Write(sb, "</div>");
                                    }
                                }
                                else
                                {
                                    foreach (var property in groupedLayoutProperties.Where(x => !roboForm.ExcludedProperties.Contains(x.Name) && !(x is RoboHiddenAttribute)))
                                    {
                                        Write(sb, "<div class=\"row\">");

                                        Write(sb, roboForm.Localizer, property, RenderControl(htmlHelper, roboForm, property));

                                        Write(sb, "</div>");
                                    }
                                }
                            }

                            Write(sb, "</div></div>");

                            if (!string.IsNullOrEmpty(groupedLayout.FormGroupWrapperEndHtml))
                            {
                                Write(sb, groupedLayout.FormGroupWrapperEndHtml);
                            }
                        }

                        Write(sb, htmlActions.ToArray());
                    }
                    break;

                #endregion RoboFormLayout.Grouped

                #region RoboFormLayout.Tab

                case RoboUIFormLayout.Tab:
                    {
                        Write(sb, "<div class=\"box\">");

                        Write(sb, string.Format("<div class=\"box-header\">"));
                        Write(sb, string.Format("<ul class=\"nav nav-tabs\">"));

                        var tabIndex = 0;
                        foreach (var tabbedLayout in roboForm.TabbedLayouts)
                        {
                            Write(sb, tabIndex == 0
                                ? string.Format("<li class=\"active\"><a data-toggle=\"tab\" href=\"#{1}_Tab{2}\">{0}</a></li>",
                                    tabbedLayout.Title,
                                    formId,
                                    tabIndex)
                                : string.Format("<li><a data-toggle=\"tab\" href=\"#{1}_Tab{2}\">{0}</a></li>",
                                    tabbedLayout.Title,
                                    formId,
                                    tabIndex));

                            tabIndex++;
                        }

                        Write(sb, "</ul>");
                        Write(sb, "</div>");

                        Write(sb, "<div class=\"box-content\">");
                        Write(sb, "<div class=\"tab-content\">");

                        tabIndex = 0;
                        foreach (var tabbedLayout in roboForm.TabbedLayouts)
                        {
                            Write(sb, tabIndex == 0
                                ? string.Format("<div id=\"{0}_Tab{1}\" class=\"tab-pane active\">", formId, tabIndex)
                                : string.Format("<div id=\"{0}_Tab{1}\" class=\"tab-pane\">", formId, tabIndex));
                            tabIndex++;

                            foreach (var item in tabbedLayout.Groups)
                            {
                                var propertiesInGroup = properties.Where(x => item.Properties.Contains(x.Name)).ToList();
                                if (propertiesInGroup.Count == 0)
                                {
                                    continue;
                                }

                                if (propertiesInGroup.All(x => x.ContainerRowIndex == -100))
                                {
                                    var index = 0;
                                    foreach (var attribute in propertiesInGroup.Where(attribute => !(attribute is RoboHiddenAttribute)))
                                    {
                                        attribute.ContainerRowIndex = index;
                                        index++;
                                    }
                                }

                                if (item.Title != null)
                                {
                                    Write(sb, "<div class=\"box\">");

                                    if (roboForm.ShowBoxHeader)
                                    {
                                        Write(sb, string.Format("<div class=\"box-header\"><span class=\"title\">{0}</span></div>", item.Title));
                                    }

                                    Write(sb, "<div class=\"box-content\">");
                                }

                                // Render hidden fields
                                foreach (var attribute in propertiesInGroup.Where(attribute => (attribute is RoboHiddenAttribute)))
                                {
                                    Write(sb, RenderControl(htmlHelper, roboForm, attribute));
                                }

                                var max = propertiesInGroup.Max(x => x.ContainerRowIndex);
                                var min = propertiesInGroup.Min(x => x.ContainerRowIndex);
                                for (var i = min; i <= max; i++)
                                {
                                    var propertiesInRow = propertiesInGroup.Where(x => x.ContainerRowIndex == i && !(x is RoboHiddenAttribute)).ToList();
                                    if (propertiesInRow.Count == 0)
                                    {
                                        continue;
                                    }

                                    Write(sb, "<div class=\"row\">");

                                    foreach (var property in propertiesInRow)
                                    {
                                        if (roboForm.ExcludedProperties.Contains(property.Name))
                                        {
                                            continue;
                                        }
                                        Write(sb, roboForm.Localizer, property, RenderControl(htmlHelper, roboForm, property));
                                    }

                                    Write(sb, "</div>");
                                }

                                if (item.Title != null)
                                {
                                    Write(sb, "</div></div>");
                                }
                            }

                            Write(sb, "</div>");
                        }

                        Write(sb, "<div class=\"row\">");
                        WriteActions(sb, roboForm.FormActionsContainerCssClass, roboForm.FormActionsCssClass, htmlActions.ToArray());
                        Write(sb, "</div>");

                        Write(sb, "</div>");
                        Write(sb, "</div>");
                        Write(sb, "</div>");
                    }

                    scriptRegister.IncludeInline(string.Format("$('#{0} a[data-toggle=\"tab\"]').on('shown.bs.tab', function (e) {{ var $id = $(e.target).attr('href');  $($id).find('select').change();  }});", formId));
                    break;

                #endregion RoboFormLayout.Tab

                #region RoboFormLayout.Wizard

                case RoboUIFormLayout.Wizard:
                    {
                        roboForm.Actions.Clear();
                        var currentStep = roboForm.GetCurrentWizardStep();

                        if (currentStep > 0)
                        {
                            roboForm.Actions.Add(new RoboUIFormAction(true, false).HasText("Back").HasName("__CurrentStep").HasValue(Convert.ToString(currentStep - 1)).HasButtonStyle(ButtonStyle.Default));
                        }

                        if (currentStep < roboForm.GroupedLayouts.Count - 1)
                        {
                            roboForm.Actions.Add(new RoboUIFormAction(true, true).HasText("Next").HasName("__CurrentStep").HasValue(Convert.ToString(currentStep + 1)).HasButtonStyle(ButtonStyle.Primary));
                        }

                        if (currentStep == roboForm.GroupedLayouts.Count - 1)
                        {
                            // Finish
                            roboForm.Actions.Add(new RoboUIFormAction(true, true).HasText(roboForm.SubmitButtonText).HasName("__CurrentStep").HasValue(Convert.ToString(currentStep + 1)).HasButtonStyle(ButtonStyle.Primary));
                        }

                        //htmlActions.AddRange(Actions.Select(action => action.Create(formProvider)));

                        var index = 0;
                        foreach (var groupedLayout in roboForm.GroupedLayouts)
                        {
                            if (currentStep != index)
                            {
                                foreach (var propertyName in groupedLayout.Properties)
                                {
                                    var value = roboForm.GetPropertyValue(roboForm.FormModel, propertyName);
                                    if (value != null)
                                    {
                                        Write(sb, string.Format(
                                            "<input type=\"hidden\" name=\"RoboWizard_{0}\" value=\"{1}\" />",
                                            propertyName,
                                            HttpUtility.HtmlEncode(value.SharpSerialize())));
                                    }
                                }

                                index++;
                                continue;
                            }

                            Write(sb, "<div class=\"box\">");

                            if (roboForm.ShowBoxHeader)
                            {
                                Write(sb, string.Format("<div class=\"box-header\"><span class=\"title\">{0}</span></div>", groupedLayout.Title));
                            }

                            Write(sb, "<div class=\"box-content\">");

                            // Auto assign grid row index
                            if (properties.Where(pair => !roboForm.ExcludedProperties.Contains(pair.Name)).All(x => x.ContainerRowIndex == -1))
                            {
                                var rowIndex = 0;
                                foreach (var property in properties)
                                {
                                    if (property is RoboHiddenAttribute)
                                    {
                                        continue;
                                    }
                                    property.ContainerRowIndex = rowIndex;
                                    rowIndex++;
                                }
                            }

                            var includedProperties = groupedLayout.Properties.Where(x => !roboForm.ExcludedProperties.Contains(x));

                            // Render hidden fields
                            foreach (var property in properties.Where(x => includedProperties.Contains(x.Name) && x is RoboHiddenAttribute))
                            {
                                Write(sb, RenderControl(htmlHelper, roboForm, property));
                            }

                            var max = properties.Max(x => x.ContainerRowIndex);
                            for (var i = 0; i <= max; i++)
                            {
                                var propertiesInRow = properties
                                    .Where(x =>
                                        x.ContainerRowIndex == i &&
                                        includedProperties.Contains(x.Name))
                                    .ToList();

                                if (!propertiesInRow.Any())
                                {
                                    continue;
                                }

                                Write(sb, "<div class=\"row\">");

                                foreach (var property in propertiesInRow)
                                {
                                    Write(sb, roboForm.Localizer, property, RenderControl(htmlHelper, roboForm, property));
                                }

                                Write(sb, "</div>");
                            }

                            Write(sb, "</div></div>");

                            index++;
                        }

                        // In wizard, we overwrite other actions
                        htmlActions = roboForm.Actions.Select(x => RenderAction(htmlHelper, x)).ToList();

                        Write(sb, "<div class=\"row\">");
                        WriteActions(sb, roboForm.FormActionsContainerCssClass, roboForm.FormActionsCssClass, htmlActions.ToArray());
                        Write(sb, "</div>");
                    }
                    break;

                #endregion RoboFormLayout.Wizard

                #region RoboFormLayout.Table

                case RoboUIFormLayout.Table:
                    {
                        Write(sb, "<div class=\"box\">");

                        if (roboForm.ShowBoxHeader)
                        {
                            Write(sb, string.Format("<div class=\"box-header\"><span class=\"title\">{0}</span></div>", roboForm.Title));
                        }

                        Write(sb, "<div class=\"box-content\">");

                        // Hidden fields
                        foreach (var property in properties.Where(pair => !roboForm.ExcludedProperties.Contains(pair.Name) && pair is RoboHiddenAttribute))
                        {
                            Write(sb, RenderControl(htmlHelper, roboForm, property));
                        }

                        Write(sb, "<table class=\"table table-bordered\">");

                        Write(sb, "<tbody>");

                        foreach (var property in properties.Where(pair => !roboForm.ExcludedProperties.Contains(pair.Name) && !(pair is RoboHiddenAttribute)))
                        {
                            if (!string.IsNullOrEmpty(property.ControlSpan))
                            {
                                continue;
                            }

                            var spanControls = properties.Where(x => x.ControlSpan == property.Name).Select(x => RenderControl(htmlHelper, roboForm, x)).ToList();
                            spanControls.Insert(0, RenderControl(htmlHelper, roboForm, property));

                            Write(sb, "<tr>");

                            Write(sb, string.Format("<td>{0}</td>", property.LabelText));

                            Write(sb, "<td>");
                            Write(sb, spanControls.ToArray());
                            Write(sb, "</td>");

                            Write(sb, "</tr>");
                        }

                        Write(sb, "</tbody>");

                        Write(sb, "</table>");

                        Write(null, htmlActions.ToArray());

                        Write(sb, "</div></div>");
                        break;
                    }

                #endregion RoboFormLayout.Table

                #region RoboFormLayout.Flat

                case RoboUIFormLayout.Flat:
                    {
                        Write(sb, "<div class=\"box\">");

                        if (roboForm.ShowBoxHeader)
                        {
                            Write(sb, string.Format(
                               "<div class=\"box-header\"><span class=\"title\">{0}</span><div class=\"pull-right box-header-controls\">{1}</div></div>",
                               roboForm.Title,
                               roboForm.ShowAsModalDialog && roboForm.ShowCloseButton
                                   ? "<i class=\"kore-icon kore-icon-close\" onclick=\"window.parent.fancyboxResult = null; parent.jQuery.fancybox.close();\" style=\"cursor:pointer\"></i>"
                                   : string.Empty));
                        }

                        Write(sb, "<div class=\"box-content\">");

                        if (!string.IsNullOrEmpty(roboForm.Description))
                        {
                            Write(sb, "<div class=\"lead\">" + roboForm.Description + "</div>");
                        }

                        if (roboForm.ShowValidationSummary)
                        {
                            if (string.IsNullOrEmpty(roboForm.ValidationSummary))
                            {
                                Write(sb, "<div data-valmsg-summary=\"true\" class=\"validation-summary\"><ul></ul></div>");
                            }
                            else
                            {
                                Write(sb, string.Format("<div data-valmsg-summary=\"true\" class=\"validation-summary\"><span>{0}</span><ul></ul></div>", roboForm.ValidationSummary));
                            }
                        }

                        // Auto assign grid row index
                        if (properties.Where(pair => !roboForm.ExcludedProperties.Contains(pair.Name)).All(x => x.ContainerRowIndex == -1))
                        {
                            var rowIndex = 0;
                            foreach (var property in properties)
                            {
                                if (property is RoboHiddenAttribute)
                                {
                                    continue;
                                }
                                property.ContainerRowIndex = rowIndex;
                                rowIndex++;
                            }
                        }

                        // Render hidden fields
                        foreach (var property in properties.Where(x => !roboForm.ExcludedProperties.Contains(x.Name) && x is RoboHiddenAttribute))
                        {
                            Write(sb, RenderControl(htmlHelper, roboForm, property));
                        }

                        var max = properties.Max(x => x.ContainerRowIndex);

                        if (max != -100)
                        {
                            for (var i = 0; i <= max; i++)
                            {
                                var propertiesInRow =
                                    properties.Where(
                                        x => x.ContainerRowIndex == i && !roboForm.ExcludedProperties.Contains(x.Name))
                                        .ToList();
                                if (!propertiesInRow.Any())
                                {
                                    continue;
                                }

                                Write(sb, "<div class=\"row\">");

                                foreach (var property in propertiesInRow)
                                {
                                    Write(sb, roboForm.Localizer, property, RenderControl(htmlHelper, roboForm, property));
                                }

                                Write(sb, "</div>");
                            }
                        }
                        else
                        {
                            foreach (var property in properties.Where(x => !roboForm.ExcludedProperties.Contains(x.Name) && !(x is RoboHiddenAttribute)))
                            {
                                Write(sb, "<div class=\"row\">");

                                Write(sb, roboForm.Localizer, property, RenderControl(htmlHelper, roboForm, property));

                                Write(sb, "</div>");
                            }
                        }

                        Write(sb, "<div class=\"row row-actions\">");
                        WriteActions(sb, roboForm.FormActionsContainerCssClass, roboForm.FormActionsCssClass, htmlActions.ToArray());
                        Write(sb, "</div>");

                        Write(sb, "</div></div>");
                    }
                    break;

                #endregion RoboFormLayout.Flat

                default:
                    throw new ArgumentOutOfRangeException();
            }

            foreach (var hiddenValue in roboForm.HiddenValues)
            {
                Write(sb, string.Format("<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\"/>", hiddenValue.Key, HttpUtility.HtmlEncode(hiddenValue.Value)));
            }

            if (!string.IsNullOrEmpty(roboForm.FormWrapperEndHtml))
            {
                Write(sb, roboForm.FormWrapperEndHtml);
            }

            if (!roboForm.DisableGenerateForm)
            {
                Write(sb, "</form>");
            }

            if (!roboForm.DisableBlockUI)
            {
                // Block UI
                Write(sb, "<div class=\"blockUI\" style=\"display:none; z-index: 100; border: none; margin: 0; padding: 0; width: 100%; height: 100%; top: 0; left: 0; background-color: #000000; opacity: 0.05; filter: alpha(opacity = 5); cursor: wait; position: absolute;\"></div>");

                // Block Msg
                Write(sb, "<div class=\"blockUIMsg\" style=\"display:none;\">Processing...</div>");

                if (roboForm.AjaxEnabled)
                {
                    scriptRegister.IncludeInline("$(document).bind(\"ajaxSend\", function(){ $(\".blockUI, .blockUIMsg\").show(); }).bind(\"ajaxComplete\", function(){ $(\".blockUI, .blockUIMsg\").hide(); });");
                }
                else
                {
                    scriptRegister.IncludeInline(string.Format("$('#{0}').on(\"submit\", function(){{ var isValid = $('#{0}').valid(); if(isValid){{ $(\".blockUI, .blockUIMsg\").show(); }} }});", formId));
                }
            }

            // End div container
            Write(sb, "</div>");

            if (roboForm.EnableKnockoutJs)
            {
                scriptRegister.IncludeInline(string.Format("var data = $('#{0}').serializeObject();", formId));
                scriptRegister.IncludeInline("var viewModel = ko.mapping.fromJS(data);");
                scriptRegister.IncludeInline(string.Format("ko.applyBindings(viewModel, document.getElementById('{0}'));", formId));
            }

            if (roboForm.Layout == RoboUIFormLayout.Tab)
            {
                scriptRegister.IncludeInline(string.Format("function {1}_ValidateTabs(){{ var validationInfo = $('#{1}').data('unobtrusiveValidation'); for(var i = 0; i < {0}; i++){{ $('a[href=#{1}_Tab' + i + ']').tab('show'); var isValid = validationInfo.validate(); if(!isValid){{ return false; }} }} }}", roboForm.GroupedLayouts.Count, formId.Replace("-", "_")));
            }

            return sb.ToString();
        }

        public override string RenderAutoCompleteAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboAutoCompleteAttribute roboAttribute)
        {
            if (roboForm.ReadOnly)
            {
                return string.Empty;
            }

            var options = roboAttribute.Options ?? roboForm.GetAutoCompleteDataSource(roboAttribute.Name);

            var attributes = roboAttribute.HtmlAttributes;

            if (roboAttribute.IsReadOnly)
            {
                MergeHtmlAttribute(attributes, "readonly", "readonly");
                return htmlHelper.TextBox(roboAttribute.Name, null, attributes).ToHtmlString();
            }

            MergeHtmlAttribute(attributes, "class", ControlCssClass);
            MergeHtmlAttribute(attributes, "data-val", "true");

            if (roboAttribute.IsRequired)
            {
                MergeHtmlAttribute(attributes, "data-val-required", T(KoreWebLocalizableStrings.Validation.Required));
            }

            MergeHtmlAttribute(attributes, "data-jqui-type", "autocomplete");
            MergeHtmlAttribute(attributes, "data-jqui-acomp-source", options.SourceUrl);
            MergeHtmlAttribute(attributes, "autocomplete", "off");

            if (roboAttribute.MinLength > 0)
            {
                MergeHtmlAttribute(attributes, "data-jqui-acomp-minlength", roboAttribute.MinLength.ToString(CultureInfo.InvariantCulture));
            }

            string onChangeFunc = null;

            if (roboAttribute.MustMatch)
            {
                var onChangeFuncName = "onAutoCompleteChange_" + Guid.NewGuid().ToString("N");
                onChangeFunc = string.Format("<script type=\"text/javascript\">function {0}(event, ui) {{ if(!ui.item){{ $(this).val(''); }} }}</script>", onChangeFuncName);
                MergeHtmlAttribute(attributes, "data-jqui-acomp-change", onChangeFuncName);
            }

            if (options.HasTextSelector == false)
            {
                var valMsg = string.Format("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", roboAttribute.Name);
                return string.Join("", new[] { onChangeFunc, htmlHelper.TextBox(roboAttribute.Name, null, attributes).ToHtmlString(), valMsg });
            }
            else
            {
                var valMsg = string.Format("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", roboAttribute.Name + "_Text");
                var textValue = options.GetText(roboForm.FormModel);
                MergeHtmlAttribute(attributes, "data-jqui-acomp-hiddenvalue", roboAttribute.Name);
                var autoCompleteTextControl = htmlHelper.TextBox(roboAttribute.Name + "_Text", textValue, attributes);
                var autoCompleteValueControl = htmlHelper.Hidden(roboAttribute.Name);
                return string.Join("", new[] { onChangeFunc, autoCompleteTextControl.ToHtmlString(), valMsg, autoCompleteValueControl.ToHtmlString() });
            }
        }

        public override string RenderButtonAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboButtonAttribute roboAttribute)
        {
            var tagBuilder = new FluentTagBuilder("button")
                .MergeAttribute("type", roboAttribute.ButtonType)
                .MergeAttribute("id", htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(roboAttribute.Name))
                .MergeAttribute("name", roboForm.ViewData.TemplateInfo.GetFullHtmlFieldName(roboAttribute.Name))
                .SetInnerText(roboAttribute.LabelText);

            if (roboAttribute.Disabled)
            {
                tagBuilder.MergeAttribute("disabled", "disabled");
            }

            if (!string.IsNullOrEmpty(roboAttribute.OnClick))
            {
                tagBuilder.MergeAttribute("onclick", roboAttribute.OnClick);
            }

            tagBuilder.MergeAttributes(roboAttribute.HtmlAttributes);

            return tagBuilder.ToString();
        }

        public override string RenderCaptchaAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCaptchaAttribute roboAttribute)
        {
            throw new NotImplementedException();
            //if (!(roboAttribute.Value is CaptchaSettings))
            //{
            //    return string.Empty;
            //}

            //var settings = roboAttribute.Value as CaptchaSettings;

            //var sb = new StringBuilder();
            //sb.Append("<script type=\"text/javascript\">var RecaptchaOptions = { theme: 'clean' };</script>");
            //sb.AppendFormat("<script type=\"text/javascript\" src=\"http://www.google.com/recaptcha/api/challenge?k={0}\"></script>", settings.PublicKey);
            //sb.Append("<noscript>");
            //sb.AppendFormat("<iframe src=\"http://www.google.com/recaptcha/api/noscript?k={0}\" height=\"300\" width=\"500\" frameborder=\"0\"></iframe><br>", settings.PublicKey);
            //sb.Append("<textarea name=\"recaptcha_challenge_field\" rows=\"3\" cols=\"40\"></textarea><input type=\"hidden\" name=\"recaptcha_response_field\" value=\"manual_challenge\"></noscript>");

            //return sb.ToString();
        }

        public override string RenderCascadingCheckBoxListAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCascadingCheckBoxListAttribute roboAttribute)
        {
            var clientId = "divcbl_" + Guid.NewGuid().ToString("N").ToLowerInvariant();
            var sourceUrl = roboForm.GetCascadingCheckBoxDataSource(roboAttribute.Name);

            if (string.IsNullOrEmpty(roboAttribute.ParentControl))
            {
                throw new ArgumentException("The ParentControl must be not null or empty.");
            }

            if (!typeof(IEnumerable).IsAssignableFrom(roboAttribute.PropertyType))
            {
                throw new NotSupportedException("Cannot apply robo choice for non enumerable property as checkbox list.");
            }

            string cssClass = ControlCssClass + " ";

            if (roboAttribute.HtmlAttributes.ContainsKey("class"))
            {
                cssClass += (roboAttribute.HtmlAttributes["class"] ?? "checkbox");
            }
            else
            {
                cssClass += "checkbox";
            }

            var value = roboAttribute.Value as IEnumerable;

            var values = new List<string>();
            string selectedItems = "";
            if (value != null)
            {
                values.AddRange(from object item in value select Convert.ToString(item));
                selectedItems = string.Join(",", values.ToArray());
            }

            var sb = new StringBuilder();
            sb.AppendFormat("$('#{0}').change(function(){{", htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(roboAttribute.ParentControl));
            if (roboAttribute.IsReadOnly)
            {
                sb.AppendFormat("$.ajax({{url: '{0}', data: 'sender={2}&' + $(this.form).serialize(), type: 'POST', dataType: 'json', success: function(result){{ var control = $('#{1}'); control.empty(); if(!result || !result.length){{ return; }} var items = '{5}'; $.each(result, function(index, item){{ if(items.indexOf(item.Value) != -1){{control.append('<label class = \"{3}\"><input type=\"checkbox\" name=\"{4}\" value=\"'+ item.Value +'\" checked=\"checked\" disabled=\"disabled\">' + item.Text + '</label>');}} else{{control.append('<label class = \"{3}\"><input type=\"checkbox\" name=\"{4}\" value=\"'+ item.Value +'\" disabled=\"disabled\">' + item.Text + '</label>');}} }}); }} }});", sourceUrl, clientId, roboAttribute.ParentControl, cssClass, roboAttribute.Name, selectedItems);
            }
            else
            {
                sb.AppendFormat("$.ajax({{url: '{0}', data: 'sender={2}&' + $(this.form).serialize(), type: 'POST', dataType: 'json', success: function(result){{ var control = $('#{1}'); control.empty(); if(!result || !result.length){{ return; }} var items = '{5}'; $.each(result, function(index, item){{ if(items.indexOf(item.Value) != -1){{control.append('<label class = \"{3}\"><input type=\"checkbox\" name=\"{4}\" value=\"'+ item.Value +'\" checked=\"checked\">' + item.Text + '</label>');}} else{{control.append('<label class = \"{3}\"><input type=\"checkbox\" name=\"{4}\" value=\"'+ item.Value +'\">' + item.Text + '</label>');}} }}); }} }});", sourceUrl, clientId, roboAttribute.ParentControl, cssClass, roboAttribute.Name, selectedItems);
            }

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var scriptRegister = new ScriptRegister(workContext);
            scriptRegister.IncludeInline(sb.ToString());

            return string.Format("<div class=\"row no-padding\" id=\"{0}\"></div>", clientId);
        }

        public override string RenderCascadingDropDownAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCascadingDropDownAttribute roboAttribute)
        {
            var options = new RoboCascadingDropDownOptions();

            // Long : when Attribute Name contains localization
            if (roboAttribute.Name.Contains('['))
            {
                var attrs = roboAttribute.Name.Split('.');
                if (attrs.Length > 2)
                {
                    options = roboForm.GetCascadingDropDownDataSource(string.Format("{0}.{1}", attrs[0], attrs[2]));
                }
                else
                {
                    options = roboForm.GetCascadingDropDownDataSource(roboAttribute.Name.RemoveBetween('[', ']'));
                }
            }
            else
            {
                options = roboForm.GetCascadingDropDownDataSource(roboAttribute.Name);
            }
            // Long

            var attributes = roboAttribute.HtmlAttributes;
            if (roboAttribute.IsReadOnly || roboForm.ReadOnly)
            {
                MergeHtmlAttribute(attributes, "disabled", "disabled");
            }

            var parentControl = options.ParentControl ?? roboAttribute.ParentControl;

            if (string.IsNullOrEmpty(parentControl))
            {
                throw new ArgumentException("The ParentControl must be not null or empty.");
            }

            if (!roboAttribute.AbsoluteParentControl)
            {
                parentControl = roboAttribute.Name.Replace(roboAttribute.Name.Split('.').Last(), parentControl);
            }

            MergeHtmlAttribute(attributes, "class", ControlCssClass);

            if (roboAttribute.IsRequired)
            {
                MergeHtmlAttribute(attributes, "data-val", "true");
                MergeHtmlAttribute(attributes, "data-val-required", T(KoreWebLocalizableStrings.Validation.Required));
            }

            if (roboAttribute.AllowMultiple)
            {
                MergeHtmlAttribute(attributes, "multiple", "multiple");
            }

            if (!string.IsNullOrEmpty(roboAttribute.OnSelectedIndexChanged))
            {
                MergeHtmlAttribute(attributes, "onchange", roboAttribute.OnSelectedIndexChanged);
            }

            if (roboAttribute.Value != null)
            {
                MergeHtmlAttribute(attributes, "data-value", roboAttribute.Value.ToString());
            }

            var clientId = roboForm.ViewData.TemplateInfo.GetFullHtmlFieldId(roboAttribute.Name);
            var parentControlId = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(parentControl);

            var sb = new StringBuilder();

            sb.AppendFormat("$('#{0}').change(function(){{", parentControlId);

            sb.Append("if($(this).is(':hidden')){ return; }");

            if (roboAttribute.EnableChosen)
            {
                if (roboAttribute.AllowMultiple)
                {
                    var multilValue = "";
                    if (roboAttribute.Value != null)
                    {
                        var arr = ((IEnumerable)roboAttribute.Value).Cast<object>()
                                 .Select(x => x.ToString())
                                 .ToArray();
                        multilValue = string.Format("[{0}]", string.Join(",", arr));
                    }

                    sb.AppendFormat("$.ajax({{url: '{0}', data: 'sender={3}&command={2}&' + $(this.form).serialize(), type: 'POST', dataType: 'json', success: function(result){{ {5} var control = $('#{1}'); var oldValue = control.data('value'); control.empty(); if(!result || !result.length){{ return; }} $.each(result, function(index, item){{ control.append($('<option></option>').attr('value', item.Value).text(item.Text)); }}); if(oldValue){{ control.val(oldValue); }} control.change(); if ($('#{1}').attr('data-value')!== undefined) {{ $('#{1}').val({4}).trigger(\"liszt:updated\"); $('#{1}').removeAttr('data-value');}} $('#{1}').trigger(\"chosen:updated\"); }} }});", options.SourceUrl, clientId, options.Command, roboAttribute.Name, multilValue, roboAttribute.OnSuccess);
                }
                else
                {
                    sb.AppendFormat("$.ajax({{url: '{0}', data: 'sender={3}&command={2}&' + $(this.form).serialize(), type: 'POST', dataType: 'json', success: function(result){{ {4} var control = $('#{1}'); var oldValue = control.data('value'); control.empty(); if(!result || !result.length){{ return; }} $.each(result, function(index, item){{ control.append($('<option></option>').attr('value', item.Value).text(item.Text)); }}); if(oldValue){{ control.val(oldValue); }} control.change(); $('#{1}').trigger(\"chosen:updated\"); }} }});", options.SourceUrl, clientId, options.Command, roboAttribute.Name, roboAttribute.OnSuccess);
                }
            }
            else
            {
                sb.AppendFormat("$.ajax({{url: '{0}', data: 'sender={3}&command={2}&' + $(this.form).serialize(), type: 'POST', dataType: 'json', success: function(result){{ {4} var control = $('#{1}'); var oldValue = control.data('value'); control.empty(); if(!result || !result.length){{ return; }} $.each(result, function(index, item){{ control.append($('<option></option>').attr('value', item.Value).text(item.Text)); }}); if(oldValue){{ control.val(oldValue); }} control.change(); }} }});", options.SourceUrl, clientId, options.Command, parentControl, roboAttribute.OnSuccess);
            }

            sb.Append("});");

            if (roboAttribute.EnableChosen)
            {
                sb.AppendFormat("$('#{0}').chosen({{ no_results_text: \"No results matched\", allow_single_deselect:true }});", clientId);
            }

            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var scriptRegister = new ScriptRegister(workContext);
            scriptRegister.IncludeInline(sb.ToString());

            // Trigger parent control change once time only
            scriptRegister.IncludeInline(string.Format("$('#{0}').change();", parentControlId), true);

            var valMsg = string.Format("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", roboAttribute.Name);

            return string.Join("", new[] { htmlHelper.DropDownList(roboAttribute.Name, new List<SelectListItem>(), attributes).ToHtmlString(), valMsg });
        }

        public override string RenderChoiceAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboChoiceAttribute roboAttribute)
        {
            switch (roboAttribute.Type)
            {
                case RoboChoiceType.CheckBox: return RenderCheckBox(htmlHelper, roboForm, roboAttribute);
                case RoboChoiceType.CheckBoxList: return RenderCheckBoxList(htmlHelper, roboForm, roboAttribute);
                case RoboChoiceType.DropDownList:
                case RoboChoiceType.RadioButtonList: return RenderSingleChoice(htmlHelper, roboForm, roboAttribute);
                default: throw new ArgumentOutOfRangeException();
            }
        }

        public override string RenderColorPickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboColorPickerAttribute roboAttribute)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var scriptRegister = new ScriptRegister(workContext);
            var clientId = roboForm.ViewData.TemplateInfo.GetFullHtmlFieldId(roboAttribute.Name);
            scriptRegister.IncludeInline(string.Format("$('#{0}').simplecolorpicker({{ picker: {1}, theme: '{2}' }});", clientId, roboAttribute.Picker.ToString().ToLowerInvariant(), roboAttribute.Theme));

            return RenderChoiceAttribute(htmlHelper, roboForm, roboAttribute);
        }

        public override string RenderComplexAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboComplexAttribute roboAttribute)
        {
            var sb = new StringBuilder(128);

            var attributes = new List<RoboControlAttribute>();
            foreach (var propertyInfo in roboAttribute.PropertyType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var attribute = propertyInfo.GetCustomAttribute<RoboControlAttribute>();
                if (attribute != null)
                {
                    attribute.Name = roboAttribute.Name + "." + propertyInfo.Name;
                    attribute.PropertyName = propertyInfo.Name;
                    attribute.PropertyType = propertyInfo.PropertyType;
                    attribute.PropertyInfo = propertyInfo;
                    attributes.Add(attribute);
                }
            }

            if (roboAttribute.EnableGrid)
            {
                var groupedLayoutColumns = roboAttribute.Column;

                if (groupedLayoutColumns == 0)
                {
                    groupedLayoutColumns = 1;
                }

                var groupedLayoutRows = (int)Math.Ceiling((double)attributes.Count / groupedLayoutColumns);

                Write(sb, "<table style=\"width: 100%;\">");

                var columnWith = 100 / groupedLayoutColumns;

                Write(sb, "<colgroup>");

                for (int i = 0; i < groupedLayoutColumns; i++)
                {
                    Write(sb, string.Format("<col style=\"width: {0}%\">", columnWith));
                }

                Write(sb, "</colgroup>");

                var index = 0;

                for (var i = 0; i < groupedLayoutRows; i++)
                {
                    Write(sb, "<tr>");

                    for (var j = 0; j < groupedLayoutColumns; j++)
                    {
                        if (index == attributes.Count)
                        {
                            continue;
                        }
                        var attribute = attributes[index];

                        attribute.Value = roboForm.GetPropertyValue(roboAttribute.Value, attribute.PropertyName);

                        if (!string.IsNullOrEmpty(attribute.ControlSpan))
                        {
                            continue;
                        }

                        var roboFormAttributes = attributes.Where(x => x.ControlSpan == attribute.PropertyName).ToList();
                        foreach (var roboFormAttribute in roboFormAttributes)
                        {
                            roboFormAttribute.Value = roboForm.GetPropertyValue(roboAttribute.Value, roboFormAttribute.PropertyName);
                        }

                        var spanControls = roboFormAttributes.Select(x => RenderControl(htmlHelper, roboForm, x)).ToList();
                        spanControls.Insert(0, RenderControl(htmlHelper, roboForm, attribute));

                        if (attribute is RoboHiddenAttribute)
                        {
                            Write(sb, attribute, spanControls.ToArray());
                            index++;
                            j--;
                            continue;
                        }

                        Write(sb, "<td>");

                        Write(sb, attribute, spanControls.ToArray());

                        Write(sb, "</td>");
                        index++;
                    }

                    Write(sb, "</tr>");
                }

                Write(sb, "</table>");
            }
            else
            {
                foreach (var attribute in attributes)
                {
                    if (roboAttribute.IsReadOnly)
                    {
                        attribute.IsReadOnly = true;
                    }

                    attribute.Value = roboForm.GetPropertyValue(roboAttribute.Value, attribute.PropertyName);

                    if (!string.IsNullOrEmpty(attribute.ControlSpan))
                    {
                        continue;
                    }

                    var roboFormAttributes = attributes.Where(x => x.ControlSpan == attribute.PropertyName).ToList();
                    foreach (var roboFormAttribute in roboFormAttributes)
                    {
                        roboFormAttribute.Value = roboForm.GetPropertyValue(roboAttribute.Value, roboFormAttribute.PropertyName);
                    }

                    var spanControls = roboFormAttributes.Select(x => RenderControl(htmlHelper, roboForm, x)).ToList();
                    spanControls.Insert(0, RenderControl(htmlHelper, roboForm, attribute));

                    Write(sb, attribute, spanControls.ToArray());
                }
            }

            return sb.ToString();
        }

        public override string RenderDatePickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboDatePickerAttribute roboAttribute)
        {
            string id = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(roboAttribute.Name);

            var dateFormat = roboAttribute.DateFormat;
            if (string.IsNullOrEmpty(dateFormat))
            {
                dateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            }
            string formatedValue = null;

            var rawValue = roboAttribute.Value;
            if (rawValue is DateTime)
            {
                var dtValue = (DateTime)rawValue;
                formatedValue = dtValue != DateTime.MinValue ? dtValue.ToString(dateFormat) : string.Empty;
            }
            else if (rawValue is string)
            {
                formatedValue = rawValue as string;
            }

            var attributes = roboAttribute.HtmlAttributes;
            MergeHtmlAttribute(attributes, "autocomplete", "off");
            MergeHtmlAttribute(attributes, "class", ControlCssClass + " datepicker");

            if (roboForm.ReadOnly || roboAttribute.IsReadOnly)
            {
                attributes.Add("readonly", "readonly");
                return htmlHelper.TextBox(roboAttribute.Name, formatedValue, attributes).ToHtmlString();
            }

            attributes.Add("data-val", "true");
            attributes.Add("data-val-date", T(KoreWebLocalizableStrings.Validation.Date));

            if (roboAttribute.IsRequired)
            {
                attributes.Add("data-val-required", roboAttribute.EnableSortRequired ? "*" : T(KoreWebLocalizableStrings.Validation.Required));
            }

            attributes.Add("data-jqui-type", "datepicker");
            attributes.Add("data-jqui-dpicker-showon", roboAttribute.ShowOn);
            attributes.Add("data-jqui-dpicker-buttonimageonly", "true");
            attributes.Add("data-jqui-dpicker-buttonimage", "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAdlJREFUeNqUU71OAkEQnpWNJ0QKAyJ4oIZTNBbG5AobLKC3Mr6AtaHwASgoLX0CazW+gbGAxsLEwqgRMAZQIoZgYc4/7taZ5e7EePHnSyY3Oz/ffnO7y+qMAQOA6ugobC8vL6E7h3aZKxaPpx8egFDxyGmYE7hgNkEB/fmyEFPq1ZV+m0qdzDB2A33wyJ0jQZ6/9fKLyUJhxTg6gna7DWo2q6vptN5PYJRKn7lMRr/O5wcpzl96ecV6egJleBj4xgYoqgrdx8f+/i85qsU+RRIYgiYBv2kYoG5tQejsDCzThDYWBnd2XIJwJCKNQLXY5/+qAIONXA78gYAMPON6KBwGL/hwU0eB/K1omwJh/dEI1EO9XNikJrIeHB4C5sCyLPdr4jj07Y+tr67SEfqocYAIaNHFBCGZTEr7yada6qFe7oxFQSrQolEZoN1m43Hp064Lmub69mZSgTMCf7cVVJpNWUS4qNXkCITTclmSEt57CrjnCGMjIxALhaQfx2ObjMWkryUSMD0x8X0ER0HXZr/vdFwFjVbLVVCt110FVOso4M/4FobwTH2cQ9S+KL+BagVjCt0hfifE+CRjwd39ffgPUFeQeukl6xHG1vBaJf5D8ApQbwmx9yHAAOL/JyG8uQSRAAAAAElFTkSuQmCC");
            attributes.Add("data-jqui-dpicker-dateformat", ConvertDateFormat(dateFormat));

            if (!string.IsNullOrEmpty(roboAttribute.MinimumValue))
            {
                attributes.Add("data-jqui-dpicker-mindate", roboAttribute.MinimumValue);
            }

            if (!string.IsNullOrEmpty(roboAttribute.MaximumValue))
            {
                attributes.Add("data-jqui-dpicker-maxdate", roboAttribute.MaximumValue);
            }

            if (!string.IsNullOrEmpty(roboAttribute.YearRangeFormat))
            {
                attributes.Add("data-jqui-dpicker-changemonth", "true");
                attributes.Add("data-jqui-dpicker-changeyear", "true");
                attributes.Add("data-jqui-dpicker-yearrange", roboAttribute.YearRangeFormat);
            }
            else if (roboAttribute.YearRangeMinOffset.HasValue && roboAttribute.YearRangeMaxOffset.HasValue)
            {
                attributes.Add("data-jqui-dpicker-changemonth", "true");
                attributes.Add("data-jqui-dpicker-changeyear", "true");

                var dt = DateTime.Now.Year;
                attributes.Add("data-jqui-dpicker-yearrange", string.Format("{0}:{1}", dt + roboAttribute.YearRangeMinOffset.Value, dt + roboAttribute.YearRangeMaxOffset.Value));
            }

            if (roboAttribute.ChangeMonth)
            {
                attributes.Add("data-jqui-dpicker-changemonth", "true");
            }

            if (roboAttribute.ChangeYear)
            {
                attributes.Add("data-jqui-dpicker-changeyear", "true");
            }

            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(roboAttribute.StartDateRange))
            {
                attributes.Add("data-jqui-dpicker-onclose", id + "_OnClose");

                sb.AppendFormat("function {0}_OnClose(selectedDate){{ $('#{1}').datepicker('option', 'maxDate', selectedDate);  }}", id, id.Replace(roboAttribute.Name, roboAttribute.StartDateRange));
            }

            if (!string.IsNullOrEmpty(roboAttribute.EndDateRange))
            {
                if (sb.Length > 0)
                {
                    throw new NotSupportedException("Cannot set value for both StartDateRange and EndDateRange.");
                }

                attributes.Add("data-jqui-dpicker-onclose", id + "_OnClose");
                sb.AppendFormat("function {0}_OnClose(selectedDate){{ $('#{1}').datepicker('option', 'minDate', selectedDate);  }}", id, id.Replace(roboAttribute.Name, roboAttribute.EndDateRange));
            }

            if (!string.IsNullOrEmpty(roboAttribute.ToChildrenDate))
            {
                attributes.Add("data-jqui-dpicker-onclose", id + "_OnClose");
                var idDateChild = id.Replace(roboAttribute.PropertyName ?? roboAttribute.Name, roboAttribute.ToChildrenDate);
                sb.AppendFormat("function {0}_OnClose(selectedDate){{ $('#{1}').datepicker('option', 'minDate', selectedDate); var currentValue = $('#{1}').val(); if(!currentValue && currentValue.length == 0) $('#{1}').val(selectedDate);  }}", id, idDateChild);
            }

            if (sb.Length > 0)
            {
                sb.Insert(0, "<script type=\"text/javascript\">");
                sb.Append("</script>");
            }

            var sb2 = new StringBuilder();

            if (!string.IsNullOrEmpty(roboAttribute.PrependText))
            {
                sb2.Append(string.Format("<span class=\"help-inline\">{0}</span>", roboAttribute.PrependText));
            }

            sb2.Append(htmlHelper.TextBox(roboAttribute.Name, formatedValue, attributes));

            if (!string.IsNullOrEmpty(roboAttribute.AppendText))
            {
                sb2.AppendFormat("<span class=\"help-inline\">{0}</span>", roboAttribute.AppendText);
            }

            sb2.AppendFormat("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", roboAttribute.Name);

            if (!string.IsNullOrEmpty(roboAttribute.HelpText))
            {
                sb2.AppendFormat("<span class=\"help-block\">{0}</span>", roboAttribute.HelpText);
            }

            sb2.Append(sb);

            return sb2.ToString();
        }

        public override string RenderDivAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboDivAttribute roboAttribute)
        {
            return new FluentTagBuilder("div")
                .MergeAttributes(roboAttribute.HtmlAttributes)
                .SetInnerHtml(Convert.ToString(roboAttribute.Value))
                .ToString();
        }

        public override string RenderFileUploadAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboFileUploadAttribute roboAttribute)
        {
            string id = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(roboAttribute.Name);

            if (!roboAttribute.EnableFineUploader)
            {
                var tagBuilder = new FluentTagBuilder("input", TagRenderMode.SelfClosing)
                    .MergeAttribute("type", "file")
                    .MergeAttribute("name", roboAttribute.Name)
                    .MergeAttribute("id", id);

                if (roboAttribute.IsRequired)
                {
                    tagBuilder.MergeAttribute("required", "required");
                    tagBuilder.MergeAttribute("data-val", "true");
                    tagBuilder.MergeAttribute("data-val-required", T(KoreWebLocalizableStrings.Validation.Required));
                }

                tagBuilder.MergeAttributes(roboAttribute.HtmlAttributes);

                string input = tagBuilder.ToString();

                if (roboAttribute.IsRequired)
                {
                    return string.Concat(input, string.Format("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", roboAttribute.Name));
                }
                return input;
            }

            var options = roboForm.GetFileUploadOptions(roboAttribute.Name);
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var uploadUrl = options.UploadUrl ?? urlHelper.Action("UploadFiles", "UploadFiles", new { area = "Admin" });
            var uploadFolder = options.UploadFolder;
            if (string.IsNullOrEmpty(uploadFolder))
            {
                uploadFolder = roboAttribute.UploadFolder;
            }

            string browseButton = null;
            if (roboAttribute.EnableBrowse)
            {
                browseButton = string.Format("<button class=\"qq-browse-button btn btn-info\" onclick=\"$.fancybox.open({{ href: '{0}?target={1}', type: 'iframe', modal: true, padding: 0, width: 500, height: 200, autoSize: false, minHeight: 250 }}); return false;\"><i class=\"kore-icon kore-icon-folder-open\"></i></button>", urlHelper.Action("Browse", "Media", new { area = "Admin" }), id);
            }

            var clientOptions = new JObject
            {
                new JProperty("multiple", false),
                new JProperty("paramsInBody", true),
                new JProperty("validation", new JObject(
                    new JProperty("allowedExtensions", RoboFileUploadAttribute.GetAllowedExtensions(options.AllowedExtensions ?? roboAttribute.AllowedExtensions)),
                    new JProperty("sizeLimit", options.SizeLimit))),
                    new JProperty("request", new JObject(new JProperty("endpoint", uploadUrl), new JProperty("params", new JObject(new JProperty("folder", uploadFolder ?? ""))))),
                    new JProperty("text", new JObject(new JProperty("uploadButton", "<i class=\"kore-icon kore-icon-upload\"></i>"))),
                new JProperty("template", string.Format("<div class=\"input-group\"><input type=\"text\" class=\"form-control\" name=\"{0}\" id=\"{1}\" value=\"{2}\" autocomplete=\"off\" data-val=\"{4}\" data-val-required=\"{5}\" /><div class=\"input-group-btn\"><div class=\"qq-upload-drop-area\"><span>{{dragZoneText}}</span></div><div class=\"qq-upload-button btn btn-danger\">{{uploadButtonText}}</div>{3}</div></div><div class=\"qq-drop-processing\"><span>{{dropProcessingText}}</span><span class=\"qq-drop-processing-spinner\"></span></div><div class=\"qq-upload-list\"></div>",
                    roboAttribute.Name, id, roboAttribute.Value, browseButton, roboAttribute.IsRequired.ToString().ToLowerInvariant(), T(KoreWebLocalizableStrings.Validation.Required))),
                new JProperty("fileTemplate", "<div><div class=\"qq-progress-bar hide\"></div><span class=\"qq-upload-spinner\"></span><span class=\"qq-upload-file hide\"></span><span class=\"qq-upload-size\"></span><a class=\"qq-upload-cancel\" href=\"#\">{cancelButtonText}</a><span class=\"qq-upload-status-text\">{statusText}</span></div>"),
            };

            var sb = new StringBuilder();

            var containerDiv = new FluentTagBuilder("div")
                .MergeAttribute("id", string.Format("{0}_Container", id))
                .MergeAttributes(roboAttribute.HtmlAttributes)
                .ToString();

            sb.AppendFormat(containerDiv);

            if (roboAttribute.IsRequired)
            {
                sb.AppendFormat("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", roboAttribute.Name);
            }

            if (roboAttribute.ShowThumbnail)
            {
                if (roboAttribute.Value != null)
                {
                    sb.AppendFormat("<a href=\"{0}\" target=\"_blank\" title=\"Click to view larger image\"><img src=\"{0}\" data-src=\"holder.js/128x128\" style=\"max-width: 128px; max-height: 128px; margin-top: 5px;\" /></a>", roboAttribute.Value);
                }
            }

            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var scriptRegister = new ScriptRegister(workContext);
            scriptRegister.IncludeInline(string.Format("$('#{0}_Container').fineUploader({1}).on('upload', function(){{ var f = document.getElementById('{0}').form; var o ={{}};var a = $(f).serializeArray(); $.each(a, function(){{ if(o[this.name] !== undefined){{ if(!o[this.name].push){{ o[this.name]=[o[this.name]]; }} o[this.name].push(this.value || ''); }}else{{ o[this.name] = this.value || '';}} }}); $(this).fineUploader('setParams', o); }}).on('complete', function(event, id, name, responseJSON){{ if(responseJSON.success){{ $('#{0}').val(responseJSON.mediaUrl); }} else {{ $('#{0}').val(''); }} }}).on('complete', function(){{ $('#{0}_Container .qq-upload-list').hide(); }});",
                id, clientOptions.ToString(Formatting.None)));

            return sb.ToString();
        }

        public override string RenderGridAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboGridAttribute roboAttribute)
        {
            roboAttribute.EnsureProperties();
            var clientId = "tbl_" + Guid.NewGuid().ToString("N").ToLowerInvariant();

            var sb = new StringBuilder();

            sb.Append("<div class=\"box\">");

            sb.AppendFormat("<div class=\"box-header\"><span class=\"title\">{0}</span></div><div class=\"box-content nopadding\">", roboAttribute.LabelText);

            // Fake Index value
            sb.AppendFormat("<input type=\"hidden\" name=\"{0}.Index\" value=\"\" />", roboAttribute.Name);

            var tableStartTag = new FluentTagBuilder("table", TagRenderMode.StartTag)
                .MergeAttribute("id", clientId)
                .MergeAttribute("data-min-rows", roboAttribute.MinRows.ToString(CultureInfo.InvariantCulture))
                .MergeAttributes(roboAttribute.HtmlAttributes)
                .ToString();

            sb.AppendFormat(tableStartTag);

            if (!roboAttribute.ShowAsStack && (roboAttribute.ShowTableHead || !string.IsNullOrEmpty(roboAttribute.TableHeadHtml)))
            {
                sb.Append("<thead>");

                if (string.IsNullOrEmpty(roboAttribute.TableHeadHtml))
                {
                    sb.Append("<tr>");

                    sb.Append("<th style=\"display: none;\"></th>");

                    foreach (var attribute in roboAttribute.Attributes)
                    {
                        if (attribute is RoboHiddenAttribute)
                        {
                            sb.Append("<th style=\"display: none;\">&nbsp;</th>");
                        }
                        else
                        {
                            if (attribute.ColumnWidth > 0)
                            {
                                sb.AppendFormat("<th style=\"width: {1}px;\">{0}</th>", attribute.LabelText, attribute.ColumnWidth);
                            }
                            else
                            {
                                sb.AppendFormat("<th>{0}</th>", attribute.LabelText);
                            }
                        }
                    }

                    if (roboAttribute.ShowRowsControl)
                    {
                        if (!roboAttribute.IsReadOnly && !roboForm.ReadOnly)
                        {
                            sb.AppendFormat("<th style=\"width: 1%;\">&nbsp;</th>");
                        }
                    }

                    sb.Append("</tr>");
                }
                else
                {
                    sb.Append(roboAttribute.TableHeadHtml);
                }
                sb.Append("</thead>");
            }

            var actualRows = 0;

            var value = roboAttribute.Value as IEnumerable<object>;
            if (value != null)
            {
                actualRows = value.Count();
            }

            var maxRows = roboAttribute.ShowRowsControl ? roboAttribute.MaxRows : actualRows;

            sb.Append("<tbody>");

            for (var i = 0; i < maxRows; i++)
            {
                if (i >= actualRows && (i >= roboAttribute.DefaultRows || actualRows > 0))
                {
                    sb.Append("<tr style=\"display: none;\">");
                    sb.AppendFormat("<td style=\"display: none;\"><input type=\"checkbox\" name=\"{0}.Index\" class=\"RoboGrid__Index\" value=\"{1}\" autocomplete=\"off\" /></td>", roboAttribute.Name, i);
                }
                else
                {
                    sb.Append("<tr>");
                    sb.AppendFormat("<td style=\"display: none;\"><input type=\"checkbox\" name=\"{0}.Index\" class=\"RoboGrid__Index\" value=\"{1}\" checked=\"checked\" autocomplete=\"off\" /></td>", roboAttribute.Name, i);
                }

                if (roboAttribute.ShowAsStack)
                {
                    sb.Append("<td style=\"padding: 0;\">");

                    foreach (var attribute in roboAttribute.Attributes)
                    {
                        if (!string.IsNullOrEmpty(attribute.ControlSpan))
                        {
                            continue;
                        }

                        attribute.Name = roboAttribute.Name + "[" + i + "]." + attribute.PropertyName;

                        if (value != null && i < actualRows)
                        {
                            var obj = value.ElementAt(i);
                            attribute.Value = roboForm.GetPropertyValue(obj, attribute.PropertyName);
                        }
                        else
                        {
                            attribute.Value = RoboGridAttribute.GetDefaultValue(attribute.PropertyType);
                        }

                        if (attribute is RoboHiddenAttribute)
                        {
                            sb.Append(RenderControl(htmlHelper, roboForm, attribute));
                        }
                        else
                        {
                            var propertyName = attribute.PropertyName;
                            var spanAttributes = roboAttribute.Attributes.Where(x => x.ControlSpan == propertyName).ToList();

                            foreach (var spanAttribute in spanAttributes)
                            {
                                spanAttribute.Name = string.Concat(roboAttribute.Name, "[", i, "].", spanAttribute.PropertyName);

                                if (value != null && i < actualRows)
                                {
                                    var obj = value.ElementAt(i);
                                    spanAttribute.Value = roboForm.GetPropertyValue(obj, spanAttribute.PropertyName);
                                }
                                else
                                {
                                    spanAttribute.Value = RoboGridAttribute.GetDefaultValue(spanAttribute.PropertyType);
                                }
                            }

                            var spanControls = spanAttributes.Select(x => RenderControl(htmlHelper, roboForm, x)).ToList();
                            spanControls.Insert(0, RenderControl(htmlHelper, roboForm, attribute));

                            if (attribute.HasLabelControl)
                            {
                                sb.AppendFormat("<div class=\"control-group\"><label class=\"control-label\">{0}</label>", attribute.LabelText);
                                sb.AppendFormat("<div class=\"controls\">{0}</div></div>", string.Join("&nbsp;&nbsp;&nbsp;", spanControls));
                            }
                            else
                            {
                                sb.AppendFormat("<div class=\"control-group\"><div class=\"controls\">{0}</div></div>", string.Join("&nbsp;&nbsp;&nbsp;", spanControls));
                            }
                        }
                    }

                    sb.Append("</td>");
                }
                else
                {
                    foreach (var attribute in roboAttribute.Attributes)
                    {
                        attribute.Name = string.Concat(roboAttribute.Name, "[", i, "].", attribute.PropertyName);

                        if (value != null && i < actualRows)
                        {
                            var obj = value.ElementAt(i);
                            attribute.Value = roboForm.GetPropertyValue(obj, attribute.PropertyName);
                        }
                        else
                        {
                            attribute.Value = RoboGridAttribute.GetDefaultValue(attribute.PropertyType);
                        }

                        if (roboAttribute.IsReadOnly)
                        {
                            if (attribute is RoboHiddenAttribute)
                            {
                                sb.AppendFormat("<td style=\"display: none;\">{0}</td>", attribute.Value);
                            }
                            else
                            {
                                sb.AppendFormat("<td>{0}</td>", attribute.Value);
                            }
                        }
                        else
                        {
                            if (attribute is RoboHiddenAttribute)
                            {
                                sb.AppendFormat("<td style=\"display: none;\">{0}</td>", RenderControl(htmlHelper, roboForm, attribute));
                            }
                            else
                            {
                                sb.AppendFormat("<td>{0}</td>", RenderControl(htmlHelper, roboForm, attribute));
                            }
                        }
                    }
                }

                if (roboAttribute.ShowRowsControl)
                {
                    if (!roboAttribute.IsReadOnly && !roboForm.ReadOnly)
                    {
                        sb.AppendFormat("<td style=\"width: 1%; vertical-align: top;\"><button type=\"button\" onclick=\"var visible = $('#{0} tbody tr:visible').length; var min = parseInt($('#{0}').data('min-rows')); if(visible >= min) {{ $(this).closest('tr').hide().find('.RoboGrid__Index').removeAttr('checked'); $('#{0}_AddButton').show(); }}\" class=\"{1} {2} pull-right\"><i class=\"kore-icon kore-icon-remove kore-icon-white\"></i></button></td>", clientId, GetButtonSizeCssClass(ButtonSize.ExtraSmall), GetButtonStyleCssClass(ButtonStyle.Danger));
                    }
                }

                sb.Append("</tr>");
            }
            sb.Append("</tbody>");

            if (roboAttribute.ShowRowsControl)
            {
                if (!roboAttribute.IsReadOnly && !roboForm.ReadOnly)
                {
                    if (roboAttribute.ShowAsStack)
                    {
                        if (actualRows == maxRows || roboAttribute.DefaultRows == maxRows)
                        {
                            sb.AppendFormat("<tfoot><tr><td colspan=\"{0}\"><button style=\"display:none;\" id=\"{1}_AddButton\" type=\"button\" onclick=\"$('#{1} tbody tr:hidden').first().show().find('.RoboGrid__Index').attr('checked','checked'); var hidden = $('#{1} tbody tr:hidden').length; if(hidden == 0){{ $('#{1}_AddButton').hide(); }}\" class=\"{2} {3} pull-right\"><i class=\"kore-icon kore-icon-add kore-icon-white\"></i></button></td></tr></tfoot>", 3, clientId, GetButtonSizeCssClass(ButtonSize.ExtraSmall), GetButtonStyleCssClass(ButtonStyle.Info));
                        }
                        else
                        {
                            sb.AppendFormat("<tfoot><tr><td colspan=\"{0}\"><button id=\"{1}_AddButton\" type=\"button\" onclick=\"$('#{1} tbody tr:hidden').first().show().find('.RoboGrid__Index').attr('checked','checked'); var hidden = $('#{1} tbody tr:hidden').length; if(hidden == 0){{ $('#{1}_AddButton').hide(); }}\" class=\"{2} {3} pull-right\"><i class=\"kore-icon kore-icon-add kore-icon-white\"></i></button></td></tr></tfoot>", 3, clientId, GetButtonSizeCssClass(ButtonSize.ExtraSmall), GetButtonStyleCssClass(ButtonStyle.Success));
                        }
                    }
                    else
                    {
                        if (actualRows == maxRows || roboAttribute.DefaultRows == maxRows)
                        {
                            sb.AppendFormat("<tfoot><tr><td colspan=\"{0}\"><button style=\"display:none;\" id=\"{1}_AddButton\" type=\"button\" onclick=\"$('#{1} tbody tr:hidden').first().show().find('.RoboGrid__Index').attr('checked','checked'); var hidden = $('#{1} tbody tr:hidden').length; if(hidden == 0){{ $('#{1}_AddButton').hide(); }}\" class=\"{2} {3} pull-right\"><i class=\"kore-icon kore-icon-add kore-icon-white\"></i></button></td></tr></tfoot>", roboAttribute.Attributes.Count + 1, clientId, GetButtonSizeCssClass(ButtonSize.ExtraSmall), GetButtonStyleCssClass(ButtonStyle.Success));
                        }
                        else
                        {
                            sb.AppendFormat("<tfoot><tr><td colspan=\"{0}\"><button id=\"{1}_AddButton\" type=\"button\" onclick=\"var row = $('#{1} tbody tr:hidden').first().show(); $('select', row).change(); row.find('.RoboGrid__Index').attr('checked','checked'); var hidden = $('#{1} tbody tr:hidden').length; if(hidden == 0){{ $('#{1}_AddButton').hide(); }}\" class=\"{2} {3} pull-right\"><i class=\"kore-icon kore-icon-add kore-icon-white\"></i></button></td></tr></tfoot>", roboAttribute.Attributes.Count + 1, clientId, GetButtonSizeCssClass(ButtonSize.ExtraSmall), GetButtonStyleCssClass(ButtonStyle.Success));
                        }
                    }
                }
            }

            sb.Append("</table></div></div>");

            if (roboAttribute.EnabledScroll)
            {
                var workContext = EngineContext.Current.Resolve<IWorkContext>();
                var scriptRegister = new ScriptRegister(workContext);
                scriptRegister.IncludeInline(string.Format("$('#{0}').parent().css('overflow','auto');", clientId));
            }

            return sb.ToString();
        }

        public override string RenderHiddenFieldAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboHiddenAttribute roboAttribute)
        {
            return htmlHelper.Hidden(roboAttribute.Name, roboAttribute.Value, roboAttribute.HtmlAttributes).ToHtmlString();
        }

        public override string RenderHtmlViewAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboHtmlViewAttribute roboAttribute)
        {
            var controllerContext = new ControllerContext
            {
                RouteData = htmlHelper.ViewContext.RouteData,
                HttpContext = htmlHelper.ViewContext.HttpContext
            };
            var result = ViewEngines.Engines.FindPartialView(controllerContext, roboAttribute.ViewName);

            if (result != null && result.View != null)
            {
                using (var writer = new StringWriter())
                {
                    var viewData = new ViewDataDictionary(htmlHelper.ViewData);
                    var viewContext = new ViewContext(controllerContext, result.View, viewData, new TempDataDictionary(), writer);
                    viewData.Model = roboAttribute.Model ?? roboAttribute.Value;

                    result.View.Render(viewContext, writer);
                    return writer.ToString();
                }
            }

            return null;
        }

        public override string RenderIconFontPickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboIconFontPickerAttribute roboAttribute)
        {
            var tagBuilder = new TagBuilder("button");

            if (roboAttribute.Value != null)
            {
                tagBuilder.Attributes.Add("data-icon", roboAttribute.Value.ToString());
            }

            tagBuilder.Attributes.Add("id", roboForm.ViewData.TemplateInfo.GetFullHtmlFieldId(roboAttribute.Name));
            tagBuilder.Attributes.Add("name", roboForm.ViewData.TemplateInfo.GetFullHtmlFieldName(roboAttribute.Name));
            tagBuilder.Attributes.Add("role", "iconpicker");
            tagBuilder.Attributes.Add("data-iconset", roboAttribute.IconSet);
            tagBuilder.Attributes.Add("data-placement", roboAttribute.Placement);
            tagBuilder.Attributes.Add("data-rows", Convert.ToString(roboAttribute.Rows));
            tagBuilder.Attributes.Add("data-cols", Convert.ToString(roboAttribute.Columns));

            tagBuilder.MergeAttributes(roboAttribute.HtmlAttributes);

            return tagBuilder.ToString();
        }

        public override string RenderImageAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboImageAttribute roboAttribute)
        {
            if (roboAttribute.Value != null)
            {
                var sb = new StringBuilder();
                sb.AppendFormat("<div style=\"{0}\">", GenerateStyleAttribute("width", roboAttribute.Width));
                sb.AppendFormat("<a href=\"{0}\" class=\"thumbnail\" target=\"_blank\">", roboAttribute.Value);
                sb.AppendFormat("<img src=\"{0}\" alt=\"\" style=\"width: 100%;{1}{2}\" />", roboAttribute.Value, GenerateStyleAttribute("height", roboAttribute.Height), GenerateStyleAttribute("max-height", roboAttribute.MaxHeight));
                sb.Append("</a>");
                sb.Append("</div>");

                return sb.ToString();
            }
            return null;
        }

        public override string RenderLabelAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboLabelAttribute roboAttribute)
        {
            var encodedValue = roboAttribute.Value == null ? string.Empty : roboAttribute.Value.ToString().HtmlEncode();

            return new FluentTagBuilder("p")
                .MergeAttributes(roboAttribute.HtmlAttributes)
                .SetInnerText(encodedValue)
                    .StartTag("input", TagRenderMode.SelfClosing)
                    .MergeAttribute("type", "hidden")
                    .MergeAttribute("name", roboAttribute.Name)
                    .MergeAttribute("value", encodedValue)
                    .EndTag()
                .ToString();
        }

        public override string RenderNumericAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboNumericAttribute roboAttribute)
        {
            var attributes = roboAttribute.HtmlAttributes;
            MergeHtmlAttribute(attributes, "class", ControlCssClass);

            if (roboAttribute.IsReadOnly || roboForm.ReadOnly)
            {
                MergeHtmlAttribute(attributes, "readonly", "readonly");
            }
            else
            {
                MergeHtmlAttribute(attributes, "data-val", "true");
                MergeHtmlAttribute(attributes, "data-val-number", T(KoreWebLocalizableStrings.Validation.Number));

                if (roboAttribute.IsRequired)
                {
                    MergeHtmlAttribute(attributes, "data-val-required", T(KoreWebLocalizableStrings.Validation.Required));
                }

                if (roboAttribute.MaxLength > 0)
                {
                    MergeHtmlAttribute(attributes, "maxlength", Convert.ToString(roboAttribute.MaxLength));
                }

                var typeCode = Type.GetTypeCode(roboAttribute.PropertyType);
                if (typeCode == TypeCode.Object)
                {
                    if (roboAttribute.PropertyType.Name == "Nullable`1")
                    {
                        typeCode = Type.GetTypeCode(roboAttribute.PropertyType.GetGenericArguments()[0]);
                    }
                    else
                    {
                        throw new NotSupportedException();
                    }
                }

                string minimumValue = null;
                string maximumValue = null;

                switch (typeCode)
                {
                    case TypeCode.SByte:
                        minimumValue = "-128";
                        maximumValue = "127";
                        break;

                    case TypeCode.Byte:
                        minimumValue = "0";
                        maximumValue = "255";
                        break;

                    case TypeCode.Int16:
                        minimumValue = "-32768";
                        maximumValue = "32767";
                        break;

                    case TypeCode.UInt16:
                        minimumValue = "0";
                        maximumValue = "65535";
                        break;

                    case TypeCode.Int32:
                        minimumValue = "-2147483648";
                        maximumValue = "2147483647";
                        break;

                    case TypeCode.UInt32:
                        minimumValue = "0";
                        maximumValue = "4294967295";
                        break;

                    case TypeCode.Int64:
                        minimumValue = "-9223372036854775808";
                        maximumValue = "9223372036854775807";
                        break;

                    case TypeCode.UInt64:
                        minimumValue = "0";
                        maximumValue = "18446744073709551615";
                        break;

                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        break;
                }

                if (!string.IsNullOrEmpty(roboAttribute.MinimumValue))
                {
                    minimumValue = roboAttribute.MinimumValue;

                    if (minimumValue == "{YearNow}")
                    {
                        minimumValue = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    }
                }

                if (!string.IsNullOrEmpty(roboAttribute.MaximumValue))
                {
                    maximumValue = roboAttribute.MaximumValue;

                    if (maximumValue == "{YearNow}")
                    {
                        maximumValue = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                    }
                }

                if (!string.IsNullOrEmpty(minimumValue) && !string.IsNullOrEmpty(maximumValue))
                {
                    MergeHtmlAttribute(attributes, "data-val-range-min", minimumValue);
                    MergeHtmlAttribute(attributes, "data-val-range-max", maximumValue);
                    MergeHtmlAttribute(attributes, "data-val-range", string.Format(T(KoreWebLocalizableStrings.Validation.Range), minimumValue, maximumValue));
                }
                else if (!string.IsNullOrEmpty(minimumValue))
                {
                    MergeHtmlAttribute(attributes, "data-val-range-min", minimumValue);
                    MergeHtmlAttribute(attributes, "data-val-range", string.Format(T(KoreWebLocalizableStrings.Validation.RangeMin), minimumValue));
                }
                else if (!string.IsNullOrEmpty(maximumValue))
                {
                    MergeHtmlAttribute(attributes, "data-val-range-max", minimumValue);
                    MergeHtmlAttribute(attributes, "data-val-range", string.Format(T(KoreWebLocalizableStrings.Validation.RangeMax), maximumValue));
                }
            }

            var valMsg = new MvcHtmlString(string.Format("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", roboAttribute.Name));

            var sb = new StringBuilder();

            if (!string.IsNullOrEmpty(roboAttribute.PrependText))
            {
                sb.Append(string.Format("<span class=\"help-inline\">{0}</span>", roboAttribute.PrependText));
            }

            sb.Append(htmlHelper.TextBox(roboAttribute.Name, roboAttribute.Value, attributes));

            if (!string.IsNullOrEmpty(roboAttribute.AppendText))
            {
                sb.AppendFormat("<span class=\"help-inline\">{0}</span>", roboAttribute.AppendText);
            }

            if (!string.IsNullOrEmpty(roboAttribute.HelpText))
            {
                sb.AppendFormat("<span class=\"help-block\">{0}</span>", roboAttribute.HelpText);
            }

            sb.Append(valMsg);

            return sb.ToString();
        }

        public override string RenderSlugAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboSlugAttribute roboAttribute)
        {
            var id = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(roboAttribute.Name);
            var attributes = roboAttribute.HtmlAttributes;
            MergeHtmlAttribute(attributes, "class", ControlCssClass);

            if (roboAttribute.MaxLength > 0)
            {
                attributes.Add("maxlength", roboAttribute.MaxLength);
            }

            MergeHtmlAttribute(attributes, "id", id);
            MergeHtmlAttribute(attributes, "name", roboAttribute.Name);
            MergeHtmlAttribute(attributes, "value", Convert.ToString(roboAttribute.Value));
            MergeHtmlAttribute(attributes, "type", "text");
            MergeHtmlAttribute(attributes, "readonly", "readonly");

            if (roboAttribute.IsReadOnly || roboForm.ReadOnly)
            {
                return htmlHelper.TextBox(roboAttribute.Name, roboAttribute.Value, attributes).ToHtmlString();
            }

            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(attributes);

            return string.Format("<div class=\"input-group\">{0}<span class=\"input-group-btn\"><button class=\"btn btn-default robo-slug-trigger\" type=\"button\" onclick=\"var $this = $('#{1}'); $this.attr('readonly') ? $this.removeAttr('readonly') : $this.attr('readonly', 'readonly');\"><i class=\"kore-icon kore-icon-edit\"></i></button></span></div>", tagBuilder.ToString(TagRenderMode.SelfClosing), id);
        }

        public override string RenderTextAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboTextAttribute roboAttribute)
        {
            // For security reason, does not show password value
            if (roboAttribute.Type == RoboTextType.Password)
            {
                roboAttribute.Value = string.Empty;
            }

            var htmlAttributes = roboAttribute.HtmlAttributes;

            MergeHtmlAttribute(htmlAttributes, "class", ControlCssClass);

            IHtmlString hiddenText = null;

            if (roboForm.ReadOnly || roboAttribute.IsReadOnly)
            {
                MergeHtmlAttribute(htmlAttributes, "readonly", "readonly");
                hiddenText = new MvcHtmlString(string.Format(@"<input type=""hidden"" value=""{0}"" name=""{1}"" />", roboAttribute.Value, roboAttribute.Name));
            }
            else
            {
                MergeHtmlAttribute(htmlAttributes, "data-val", "true");

                if (roboAttribute.IsRequired)
                {
                    MergeHtmlAttribute(htmlAttributes, "required", "required");
                    MergeHtmlAttribute(htmlAttributes, "data-val-required", T(KoreWebLocalizableStrings.Validation.Required));
                }

                if (roboAttribute.MinLength > 0 && roboAttribute.MaxLength > 0)
                {
                    MergeHtmlAttribute(htmlAttributes, "data-val-length-min", Convert.ToString(roboAttribute.MinLength));
                    MergeHtmlAttribute(htmlAttributes, "data-val-length-max", Convert.ToString(roboAttribute.MaxLength));
                    MergeHtmlAttribute(htmlAttributes, "data-val-length", string.Format(T(KoreWebLocalizableStrings.Validation.RangeLength), roboAttribute.MinLength, roboAttribute.MaxLength));
                    MergeHtmlAttribute(htmlAttributes, "maxlength", Convert.ToString(roboAttribute.MaxLength));
                }
                else if (roboAttribute.MinLength > 0)
                {
                    MergeHtmlAttribute(htmlAttributes, "data-val-length-min", Convert.ToString(roboAttribute.MinLength));
                    MergeHtmlAttribute(htmlAttributes, "data-val-length", string.Format(T(KoreWebLocalizableStrings.Validation.MinLength), roboAttribute.MinLength));
                }
                else if (roboAttribute.MaxLength > 0)
                {
                    MergeHtmlAttribute(htmlAttributes, "data-val-length-max", Convert.ToString(roboAttribute.MaxLength));
                    MergeHtmlAttribute(htmlAttributes, "data-val-length", string.Format(T(KoreWebLocalizableStrings.Validation.MaxLength), roboAttribute.MaxLength));
                    MergeHtmlAttribute(htmlAttributes, "maxlength", Convert.ToString(roboAttribute.MaxLength));
                }

                switch (roboAttribute.Type)
                {
                    case RoboTextType.Email:
                        MergeHtmlAttribute(htmlAttributes, "data-val-email", T(KoreWebLocalizableStrings.Validation.Email));
                        MergeHtmlAttribute(htmlAttributes, "type", "email");
                        break;

                    case RoboTextType.Url:
                        MergeHtmlAttribute(htmlAttributes, "data-val-url", T(KoreWebLocalizableStrings.Validation.Url));
                        MergeHtmlAttribute(htmlAttributes, "type", "url");
                        break;

                    case RoboTextType.Password:
                        MergeHtmlAttribute(htmlAttributes, "type", "password");

                        if (!string.IsNullOrEmpty(roboAttribute.EqualTo))
                        {
                            MergeHtmlAttribute(htmlAttributes, "data-val-equalto", T(KoreWebLocalizableStrings.Validation.EqualTo));
                            MergeHtmlAttribute(htmlAttributes, "data-val-equalto-other", roboAttribute.EqualTo);
                        }
                        break;
                }
            }

            string helpText = null;
            if (!string.IsNullOrEmpty(roboAttribute.HelpText))
            {
                helpText = string.Format("<span class=\"help-block\">{0}</span>", roboAttribute.HelpText);
            }

            var valMsg = string.Format("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", roboAttribute.Name);

            switch (roboAttribute.Type)
            {
                case RoboTextType.MultiText:
                    {
                        if (roboAttribute.Rows > 0)
                        {
                            MergeHtmlAttribute(htmlAttributes, "rows", roboAttribute.Rows.ToString(CultureInfo.InvariantCulture));
                        }
                        else
                        {
                            MergeHtmlAttribute(htmlAttributes, "rows", "5");
                        }

                        if (roboAttribute.Cols > 0)
                        {
                            MergeHtmlAttribute(htmlAttributes, "cols", roboAttribute.Cols.ToString(CultureInfo.InvariantCulture));
                        }

                        return string.Join("", htmlHelper.TextArea(roboAttribute.Name, (string)roboAttribute.Value, htmlAttributes), valMsg, helpText);
                    }
                case RoboTextType.RichText:
                    {
                        if (roboAttribute.IsReadOnly || roboForm.ReadOnly)
                        {
                            return string.Format("<p>{0}</p>", roboAttribute.Value);
                        }

                        htmlAttributes.Remove("class");
                        MergeHtmlAttribute(htmlAttributes, "class", "richtext ckeditor");
                        return string.Join("", htmlHelper.TextArea(roboAttribute.Name, (string)roboAttribute.Value, htmlAttributes), valMsg, helpText);
                    }
                default:
                    {
                        var sb = new StringBuilder();
                        sb.Append(new CombinedHtmlString(
                            htmlHelper.TextBox(roboAttribute.Name, roboAttribute.Value, htmlAttributes),
                            hiddenText,
                            valMsg,
                            helpText));

                        if (!string.IsNullOrEmpty(roboAttribute.PrependText))
                        {
                            sb.Insert(0, string.Format("<span class=\"help-inline\">{0}</span>", roboAttribute.PrependText));
                        }

                        if (!string.IsNullOrEmpty(roboAttribute.AppendText))
                        {
                            sb.AppendFormat("<span class=\"help-inline\">{0}</span>", roboAttribute.AppendText);
                        }

                        return sb.ToString();
                    }
            }
        }

        private string RenderCheckBox<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboChoiceAttribute attribute) where TModel : class
        {
            if (attribute.PropertyType != typeof(bool) && attribute.PropertyType != typeof(bool?))
            {
                throw new NotSupportedException("Cannot apply robo choice for non-Boolean property as checkbox.");
            }

            var attributes = attribute.HtmlAttributes;

            if (attribute.IsRequired)
            {
                MergeHtmlAttribute(attributes, "data-val", "true");
                MergeHtmlAttribute(attributes, "data-val-required", T(KoreWebLocalizableStrings.Validation.Required));
            }

            if (!string.IsNullOrEmpty(attribute.OnSelectedIndexChanged))
            {
                MergeHtmlAttribute(attributes, "onchange", attribute.OnSelectedIndexChanged);
            }

            if (attribute.IsReadOnly || roboForm.ReadOnly)
            {
                MergeHtmlAttribute(attributes, "disabled", "disabled");
            }

            var sbCheckBox = new StringBuilder();

            var cssClass = attributes.ContainsKey("class") ? attributes["class"].ToString() : string.Empty;

            if (string.IsNullOrEmpty(cssClass))
            {
                sbCheckBox.Append("<div class=\"checkbox\"><label>");
            }
            else
            {
                sbCheckBox.AppendFormat("<div class=\"checkbox\"><label class=\"{0}\">", cssClass);
            }

            var checkBox = htmlHelper.CheckBox(attribute.Name, Convert.ToBoolean(attribute.Value), attributes);
            sbCheckBox.Append(checkBox);

            if (!string.IsNullOrEmpty(attribute.LabelText))
            {
                sbCheckBox.Append("&nbsp;");
                sbCheckBox.Append(roboForm.Localizer(attribute.LabelText));
            }

            sbCheckBox.Append("</label></div>");

            if (!string.IsNullOrEmpty(attribute.HelpText))
            {
                sbCheckBox.AppendFormat("<span class=\"help-block\">{0}</span>", attribute.HelpText);
            }

            return sbCheckBox.ToString();
        }

        private string RenderCheckBoxList<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboChoiceAttribute attribute) where TModel : class
        {
            if (!typeof(IEnumerable).IsAssignableFrom(attribute.PropertyType))
            {
                throw new NotSupportedException("Cannot apply robo choice for non enumerable property as checkbox list.");
            }

            var value = attribute.Value as IEnumerable;
            var values = new List<string>();
            if (value != null)
            {
                values.AddRange(from object item in value select Convert.ToString(item));
            }

            IList<SelectListItem> selectItems;

            if (attribute.SelectListItems == null)
            {
                selectItems = roboForm.GetExternalDataSource(attribute.Name.RemoveBetween('[', ']'));

                if (selectItems == null)
                {
                    throw new NotSupportedException("You need to register an external data source for " + attribute.Name);
                }
            }
            else
            {
                selectItems = attribute.SelectListItems.ToList();
            }

            string cssClass = "checkbox";

            if (attribute.HtmlAttributes.ContainsKey("class"))
            {
                cssClass = attribute.HtmlAttributes["class"].ToString();
            }

            var sb = new StringBuilder();

            if (attribute.GroupedByCategory)
            {
                var items = selectItems.Cast<ExtendedSelectListItem>().ToList();
                var groups = items.GroupBy(x => x.Category).ToList();

                if (attribute.Columns > 1)
                {
                    var rows = (int)Math.Ceiling((groups.Count * 1d) / attribute.Columns);
                    var columnWidth = 12 / attribute.Columns;
                    for (var i = 0; i < rows; i++)
                    {
                        sb.Append("<div class=\"row\">");

                        for (var j = 0; j < attribute.Columns; j++)
                        {
                            var index = (i * attribute.Columns) + j;
                            sb.AppendFormat("<div class=\"col-xs-{0}\">", columnWidth);

                            if (groups.Count > index)
                            {
                                var group = groups[index];

                                sb.AppendFormat("<strong>{0}</strong>", group.Key);

                                foreach (var item in group)
                                {
                                    var isChecked = values.Contains(item.Value);
                                    var checkbox = new TagBuilder("input");

                                    string name = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(attribute.Name);
                                    checkbox.MergeAttribute("type", "checkbox");
                                    checkbox.MergeAttribute("name", name);
                                    checkbox.MergeAttribute("id", htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(attribute.Name) + "_" + index);
                                    checkbox.MergeAttribute("value", item.Value);
                                    if (isChecked)
                                    {
                                        checkbox.MergeAttribute("checked", "checked");
                                    }

                                    if (attribute.IsReadOnly || roboForm.ReadOnly)
                                    {
                                        checkbox.MergeAttribute("disabled", "disabled");
                                    }

                                    sb.AppendFormat("<label for=\"{3}\" class=\"{2}\">{1}{0}</label>", item.Text, checkbox.ToString(TagRenderMode.SelfClosing), cssClass, name);
                                    index++;
                                }
                            }

                            sb.Append("</div>");
                        }

                        sb.Append("</div>");
                    }
                }
                else
                {
                    int index = 0;

                    foreach (var @group in groups)
                    {
                        sb.AppendFormat("<strong>{0}</strong>", group.Key);

                        foreach (var item in group)
                        {
                            var isChecked = values.Contains(item.Value);
                            var checkbox = new TagBuilder("input");

                            string name = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(attribute.Name);
                            checkbox.MergeAttribute("type", "checkbox");
                            checkbox.MergeAttribute("name", name);
                            checkbox.MergeAttribute("id", htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(attribute.Name) + "_" + index);
                            checkbox.MergeAttribute("value", item.Value);
                            if (isChecked)
                            {
                                checkbox.MergeAttribute("checked", "checked");
                            }

                            if (attribute.IsReadOnly || roboForm.ReadOnly)
                            {
                                checkbox.MergeAttribute("disabled", "disabled");
                            }

                            sb.AppendFormat("<label for=\"{3}\" class=\"{2}\">{1}{0}</label>", item.Text, checkbox.ToString(TagRenderMode.SelfClosing), cssClass, name);
                            index++;
                        }
                    }
                }
            }
            else
            {
                var columns = (attribute.Columns > 0) ? attribute.Columns : 1;
                var rows = (int)Math.Ceiling((selectItems.Count() * 1d) / columns);
                var columnWidth = (int)Math.Ceiling(12d / columns);
                int index = 0;

                for (var i = 0; i < columns; i++)
                {
                    var items = selectItems.Skip(i * rows).Take(rows);
                    sb.AppendFormat("<div class=\"col-xs-{0} col-sm-{0} col-md-{0} col-lg-{0}\">", columnWidth);

                    foreach (var item in items)
                    {
                        var isChecked = values.Contains(item.Value);
                        var checkbox = new TagBuilder("input");
                        checkbox.MergeAttribute("type", "checkbox");
                        checkbox.MergeAttribute("name", htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldName(attribute.Name));
                        checkbox.MergeAttribute("id", htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(attribute.Name) + "_" + index);
                        checkbox.MergeAttribute("value", item.Value);
                        if (isChecked)
                        {
                            checkbox.MergeAttribute("checked", "checked");
                        }

                        if (attribute.IsReadOnly || roboForm.ReadOnly)
                        {
                            checkbox.MergeAttribute("disabled", "disabled");
                        }

                        sb.AppendFormat("<label class=\"{2}\">{1}{0}</label>", item.Text, checkbox.ToString(TagRenderMode.SelfClosing), cssClass);
                        index++;
                    }

                    sb.Append("</div>");
                }
            }

            return sb.ToString();
        }

        private string RenderSingleChoice<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboChoiceAttribute attribute) where TModel : class
        {
            var attributes = attribute.HtmlAttributes;
            var valMsg = new MvcHtmlString(string.Format("<span data-valmsg-for=\"{0}\" data-valmsg-replace=\"true\"></span>", attribute.Name));

            var selectedValue = Convert.ToString(attribute.Value);

            IList<SelectListItem> selectItems;
            if (attribute.PropertyType != null && attribute.PropertyType.IsEnum)
            {
                var values = Enum.GetValues(attribute.PropertyType);

                selectItems = (from object value in values
                               select new SelectListItem
                               {
                                   Text = GetEnumValueDescription(attribute.PropertyType, value),
                                   Value = Convert.ToString(value),
                               }).ToList();
            }
            else
            {
                if (attribute.SelectListItems == null)
                {
                    var tmpSelectItems = roboForm.GetExternalDataSource(attribute.Name);

                    if (tmpSelectItems == null && attribute.Name.Contains('['))
                    {
                        // Long : when Attribute Name contains localization
                        var attrs = attribute.Name.Split('.');
                        if (attrs.Length > 2)
                        {
                            tmpSelectItems = roboForm.GetExternalDataSource(string.Format("{0}.{1}", attrs[0], attrs[2]));
                        }
                        else
                        {
                            tmpSelectItems = roboForm.GetExternalDataSource(attribute.Name.RemoveBetween('[', ']'));
                        }
                        // Long : when Attribute Name contains localization
                    }

                    if (tmpSelectItems == null && attribute.Name.Contains('.'))
                    {
                        tmpSelectItems = roboForm.GetExternalDataSource((attribute.Name + ".").RemoveBetween('.', '.'));
                    }

                    if (tmpSelectItems == null)
                    {
                        throw new NotSupportedException("You need to register an external data source for " + attribute.Name);
                    }
                    selectItems = tmpSelectItems.ToList();
                }
                else
                {
                    selectItems = attribute.SelectListItems.ToList();
                }
            }

            string cssClass;

            if (attribute.HtmlAttributes.ContainsKey("class"))
            {
                cssClass = ControlCssClass + " " + attribute.HtmlAttributes["class"];
            }
            else
            {
                cssClass = ControlCssClass;
            }

            MergeHtmlAttribute(attributes, "class", ControlCssClass);

            if (attribute.IsReadOnly || roboForm.ReadOnly)
            {
                string selectedText = null;

                var item = selectItems.FirstOrDefault(x => x.Value == selectedValue);
                if (item != null)
                {
                    selectedText = item.Text;
                }

                return string.Format(
                    @"<input type=""hidden"" id=""{3}"" name=""{0}"" value=""{1}"" /><input type=""text"" class=""{4}"" readonly=""readonly"" value=""{2}"" />",
                    attribute.Name,
                    selectedValue,
                    selectedText,
                    htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(attribute.Name),
                    cssClass);
            }

            if (attribute.IsRequired)
            {
                MergeHtmlAttribute(attributes, "data-val", "true");
                if (attribute.RequiredIfHaveItemsOnly == false || selectItems.Any())
                {
                    MergeHtmlAttribute(attributes, "data-val-required", T(KoreWebLocalizableStrings.Validation.Required));
                }
            }

            if (attribute.Type == RoboChoiceType.DropDownList)
            {
                var builder = new StringBuilder();

                if (!string.IsNullOrEmpty(attribute.OnSelectedIndexChanged))
                {
                    MergeHtmlAttribute(attributes, "onchange", attribute.OnSelectedIndexChanged);
                }

                var selectedValues = new List<string>();

                if (attribute.AllowMultiple)
                {
                    MergeHtmlAttribute(attributes, "multiple", "multiple");

                    var value = attribute.Value as IEnumerable;
                    if (value != null)
                    {
                        selectedValues.AddRange(from object item in value select Convert.ToString(item));
                    }
                }

                var selectTag = htmlHelper.DropDownList(attribute.Name, new SelectListItem[] { }, attributes);

                if (!attribute.IsRequired)
                {
                    builder.Append("<option>");
                    builder.Append(attribute.OptionLabel);
                    builder.AppendLine("</option>");
                }

                if (attribute.GroupedByCategory && attribute.EnableChosen)
                {
                    var items = selectItems.Cast<ExtendedSelectListItem>().ToList();
                    var groups = items.GroupBy(x => x.Category);

                    foreach (var @group in groups)
                    {
                        builder.AppendFormat("<optgroup label=\"{0}\">", group.Key);

                        foreach (var item in group)
                        {
                            var optionTag = new TagBuilder("option") { InnerHtml = HttpUtility.HtmlEncode(item.Text) };
                            if (item.Value != null)
                            {
                                optionTag.Attributes["value"] = item.Value;
                            }

                            if (attribute.AllowMultiple)
                            {
                                if (selectedValues.Contains(item.Value))
                                {
                                    optionTag.Attributes["selected"] = "selected";
                                }
                            }
                            else
                            {
                                if (item.Value == selectedValue)
                                {
                                    optionTag.Attributes["selected"] = "selected";
                                }
                            }

                            if (item.HtmlAttributes != null)
                            {
                                var htmlAttributes = item.HtmlAttributes as IDictionary<string, object>;
                                optionTag.MergeAttributes(htmlAttributes ?? HtmlHelper.AnonymousObjectToHtmlAttributes(item.HtmlAttributes));
                            }

                            builder.AppendLine(optionTag.ToString(TagRenderMode.Normal));
                        }

                        builder.Append("</optgroup>");
                    }
                }
                else
                {
                    foreach (var selectItem in selectItems)
                    {
                        var optionTag = new TagBuilder("option") { InnerHtml = HttpUtility.HtmlEncode(selectItem.Text) };
                        if (selectItem.Value != null)
                        {
                            optionTag.Attributes["value"] = selectItem.Value;
                        }

                        if (attribute.AllowMultiple)
                        {
                            if (selectedValues.Contains(selectItem.Value))
                            {
                                optionTag.Attributes["selected"] = "selected";
                            }
                        }
                        else
                        {
                            if (selectItem.Value == selectedValue)
                            {
                                optionTag.Attributes["selected"] = "selected";
                            }
                        }

                        var extendedSelectListItem = selectItem as ExtendedSelectListItem;
                        if (extendedSelectListItem != null && extendedSelectListItem.HtmlAttributes != null)
                        {
                            var htmlAttributes = extendedSelectListItem.HtmlAttributes as IDictionary<string, object>;
                            optionTag.MergeAttributes(htmlAttributes ?? HtmlHelper.AnonymousObjectToHtmlAttributes(extendedSelectListItem.HtmlAttributes));
                        }

                        builder.AppendLine(optionTag.ToString(TagRenderMode.Normal));
                    }
                }

                var html = selectTag.ToHtmlString();

                builder.Insert(0, html.Replace("</select>", ""));
                builder.Append("</select>");

                if (!string.IsNullOrEmpty(attribute.PrependText) && !string.IsNullOrEmpty(attribute.AppendText))
                {
                    builder.Insert(0, string.Format("<div class=\"input-prepend input-append\"><span class=\"add-on\">{0}</span>", attribute.PrependText));
                    builder.AppendFormat("<span class=\"add-on\">{0}</span>", attribute.AppendText);
                    builder.Append("</div>");
                }
                else if (!string.IsNullOrEmpty(attribute.PrependText))
                {
                    builder.Insert(0, string.Format("<div class=\"input-prepend\"><span class=\"add-on\">{0}</span>", attribute.PrependText));
                    builder.Append("</div>");
                }
                else if (!string.IsNullOrEmpty(attribute.AppendText))
                {
                    builder.Insert(0, "<div class=\"input-append\">");
                    builder.AppendFormat("<span class=\"add-on\">{0}</span>", attribute.AppendText);
                    builder.Append("</div>");
                }

                builder.Append(valMsg);

                if (!string.IsNullOrEmpty(attribute.HelpText))
                {
                    builder.AppendFormat("<span class=\"help-block\">{0}</span>", attribute.HelpText);
                }

                #region Scripts

                if (attribute.EnableChosen)
                {
                    var workContext = EngineContext.Current.Resolve<IWorkContext>();
                    var scriptRegister = new ScriptRegister(workContext);
                    scriptRegister.IncludeInline(string.Format("$('#{0}').chosen({{ no_results_text: \"No results matched\", allow_single_deselect:true, display_selected_options: true, inherit_select_classes : true }});", htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(attribute.Name)));
                }

                #endregion Scripts

                return builder.ToString();
            }

            #region Radio Buttons

            if (attributes.ContainsKey("class"))
            {
                cssClass = "radio";
            }
            attributes.Remove("class");

            var sb = new StringBuilder();
            var index = 0;
            foreach (var selectItem in selectItems)
            {
                IDictionary<string, object> htmlAttributes = attributes;

                string dataBind = null;
                var extendedSelectListItem = selectItem as ExtendedSelectListItem;
                if (extendedSelectListItem != null && extendedSelectListItem.HtmlAttributes != null)
                {
                    htmlAttributes = attributes
                        .Union(new RouteValueDictionary(extendedSelectListItem.HtmlAttributes))
                        .ToDictionary(k => k.Key, v => v.Value);

                    if (htmlAttributes.ContainsKey("container-data-bind"))
                    {
                        dataBind = Convert.ToString(htmlAttributes["container-data-bind"]);
                        htmlAttributes.Remove("container-data-bind");
                    }
                }

                htmlAttributes["id"] = htmlHelper.ViewData.TemplateInfo.GetFullHtmlFieldId(attribute.Name + "_" + index);

                var radioButton = htmlHelper.RadioButton(attribute.Name, selectItem.Value, selectItem.Value == selectedValue, htmlAttributes);
                sb.AppendFormat("<div class=\"{2}\" data-bind=\"{3}\"><label>{1}{0}</label></div>", selectItem.Text, radioButton, cssClass, dataBind);
                index++;
            }

            return sb.ToString();

            #endregion Radio Buttons
        }

        #endregion IRoboUIFormProvider Members

        protected virtual void WriteActions(StringBuilder builder, HtmlHelper htmlHelper, IEnumerable<RoboUIFormAction> actions)
        {
            if (!actions.Any())
            {
                return;
            }

            Write(builder, string.Format("<div class=\"{0}\">", FormGroupCssClass));

            Write(builder, "<div class=\"btn-toolbar\">");

            foreach (var action in actions)
            {
                Write(builder, "<div class=\"btn-group\">");
                Write(builder, RenderAction(htmlHelper, action));
                Write(builder, "</div>");
            }

            Write(builder, "</div></div>");
        }

        protected virtual void WriteActions(StringBuilder builder, string formActionsContainerCssClass, string formActionsCssClass, params string[] htmlActions)
        {
            if (!string.IsNullOrEmpty(formActionsContainerCssClass) && !string.IsNullOrEmpty(formActionsCssClass))
            {
                builder.AppendFormat("<div class=\"{0}\">", formActionsContainerCssClass);

                builder.Append("<div class=\"row\"><div class=\"form-group\">");
                builder.AppendFormat("<div class=\"{0}\">", formActionsCssClass);
                var flag = false;
                foreach (var htmlAction in htmlActions)
                {
                    if (flag)
                    {
                        builder.Append("&nbsp;&nbsp;&nbsp;");
                    }
                    flag = true;
                    builder.Append(htmlAction);
                }

                builder.Append("</div></div></div></div>");
            }
            else
            {
                builder.AppendFormat("<div class=\"{0}\">", formActionsContainerCssClass ?? "col-md-12");
                var flag = false;
                foreach (var htmlAction in htmlActions)
                {
                    if (flag)
                    {
                        builder.Append("&nbsp;&nbsp;&nbsp;");
                    }
                    flag = true;
                    builder.Append(htmlAction);
                }
                builder.Append("</div>");
            }
        }

        private void Write(StringBuilder builder, string htmlString)
        {
            builder.Append(htmlString);
        }

        protected virtual void Write(StringBuilder builder, params string[] inputControls)
        {
            if (inputControls != null && inputControls.Length != 0)
            {
                Write(builder, string.Format("<div class=\"{0}\">", FormGroupCssClass));

                var renderSpaces = false;

                foreach (var inputControl in inputControls.Where(x => x != null))
                {
                    if (renderSpaces)
                    {
                        Write(builder, "&nbsp;&nbsp;&nbsp;");
                    }
                    Write(builder, inputControl);
                    renderSpaces = true;
                }

                Write(builder, "</div>");
            }
        }

        protected virtual void Write(StringBuilder builder, Localizer localizer, RoboControlAttribute formAttribute, string inputControl)
        {
            if (!string.IsNullOrEmpty(formAttribute.ContainerCssClass))
            {
                builder.AppendFormat("<div class=\"{0}\">", formAttribute.ContainerCssClass);
            }

            builder.AppendFormat("<div class=\"form-group\" data-bind=\"{0}\">", formAttribute.ContainerDataBind);

            if (formAttribute.HasLabelControl)
            {
                if (formAttribute.HideLabelControl)
                {
                    builder.Append("<label>&nbsp;</label>");
                }
                else
                {
                    builder.AppendFormat("<label for=\"{2}\" class=\"{1}\">{0}</label>", localizer(formAttribute.LabelText ?? formAttribute.Name), formAttribute.LabelCssClass, formAttribute.Name.Replace(".", "_").Replace("[", "_").Replace("]", "_"));
                }
            }

            if (string.IsNullOrEmpty(formAttribute.ControlContainerCssClass))
            {
                builder.Append(inputControl);
            }
            else
            {
                builder.AppendFormat("<div class=\"{0}\">", formAttribute.ControlContainerCssClass);
                builder.Append(inputControl);
                builder.Append("</div>");
            }

            builder.Append("</div>");

            if (!string.IsNullOrEmpty(formAttribute.ContainerCssClass))
            {
                builder.Append("</div>");
            }
        }

        protected virtual void Write(StringBuilder builder, RoboControlAttribute formAttribute, params string[] inputControls)
        {
            if (inputControls == null || inputControls.Length == 0) return;

            var hasLabelControl = false;
            var controlsCssClass = string.Empty;

            Write(builder, formAttribute != null && !string.IsNullOrEmpty(formAttribute.ContainerDataBind)
                ? string.Format("<div class=\"{1}\" data-bind=\"{0}\">", formAttribute.ContainerDataBind, FormGroupCssClass)
                : string.Format("<div class=\"{0}\">", FormGroupCssClass));

            if (formAttribute != null)
            {
                if (formAttribute.HasLabelControl)
                {
                    hasLabelControl = true;
                    Write(builder, formAttribute.HideLabelControl
                        ? string.Format("<label class=\"{0}\">&nbsp;</label>", LabelCssClass)
                        : string.Format("<label class=\"{2}\" for=\"{1}\">{0}</label>",
                            formAttribute.LabelText ?? formAttribute.PropertyName ?? formAttribute.Name,
                            formAttribute.Name.Replace(".", "_").Replace("[", "_").Replace("]", "_"), LabelCssClass));
                }
            }
            else
            {
                hasLabelControl = true;
                controlsCssClass = " form-buttons";
            }

            if (hasLabelControl)
            {
                Write(builder, string.Format("<div class=\"{0}\">", ControlsCssClass + controlsCssClass));
            }

            var renderSpaces = false;

            foreach (var inputControl in inputControls.Where(x => x != null))
            {
                if (renderSpaces)
                {
                    Write(builder, "&nbsp;&nbsp;&nbsp;");
                }
                Write(builder, inputControl);
                renderSpaces = true;
            }

            if (hasLabelControl)
            {
                Write(builder, "</div>");
            }

            Write(builder, "</div>");
        }
    }
}
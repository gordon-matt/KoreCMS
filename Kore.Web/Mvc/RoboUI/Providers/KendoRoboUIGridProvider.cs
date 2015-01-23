using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kore.Infrastructure;
using Kore.Web.Mvc.Resources;
using Kore.Web.Mvc.RoboUI.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Mvc.RoboUI.Providers
{
    public class KendoRoboUIGridProvider : BaseRoboUIGridProvider
    {
        public override string Render<TModel>(RoboUIGridResult<TModel> roboUIGrid, HtmlHelper htmlHelper)
        {
            var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
            var scriptRegister = new ScriptRegister(workContext);
            var sb = new StringBuilder(2048);

            if (!string.IsNullOrEmpty(roboUIGrid.GridWrapperStartHtml))
            {
                Write(sb, roboUIGrid.GridWrapperStartHtml);
            }

            // Start div container
            Write(sb, string.Format("<div class=\"robo-grid-container\" id=\"{0}_Container\">", roboUIGrid.ClientId));

            var form = BeginForm(htmlHelper, roboUIGrid);
            Write(sb, form);

            if (roboUIGrid.FilterForm != null)
            {
                var filterForm = roboUIGrid.FilterForm.GenerateView();
                Write(sb, filterForm);
            }

            WriteActions(sb, htmlHelper, roboUIGrid.Actions);

            Write(sb, string.Format("<div class=\"{0}\" id=\"{1}\"></div>", roboUIGrid.CssClass, roboUIGrid.ClientId));

            // Hidden values
            foreach (var hiddenValue in roboUIGrid.HiddenValues)
            {
                Write(sb, string.Format("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\"/>", hiddenValue.Key, HttpUtility.HtmlEncode(hiddenValue.Value)));
            }

            if (!string.IsNullOrEmpty(form))
            {
                Write(sb, "</form>");
            }

            // End div container
            Write(sb, "</div>");

            if (!string.IsNullOrEmpty(roboUIGrid.GridWrapperEndHtml))
            {
                Write(sb, roboUIGrid.GridWrapperEndHtml);
            }

            #region Kendo UI Options

            var getRecordsUrl = string.IsNullOrEmpty(roboUIGrid.GetRecordsUrl)
                                ? roboUIGrid.ControllerContext.HttpContext.Request.RawUrl
                                : roboUIGrid.GetRecordsUrl;

            var columns = new JArray();
            var modelFields = new JObject();

            foreach (var column in roboUIGrid.Columns)
            {
                var options = new JObject(
                    new JProperty("field", column.PropertyName),
                    new JProperty("filterable", column.Filterable),
                    new JProperty("encoded", false),
                    new JProperty("title", column.HeaderText),
                    new JProperty("hidden", column.Hidden),
                    new JProperty("attributes", new JObject
                    {
                        {"style", "text-align:" + column.Align + ";"}
                    }),
                    new JProperty("headerAttributes", new JObject
                    {
                        {"style", "text-align:" + column.Align + ";"}
                    }));

                if (column.Width.HasValue)
                {
                    options.Add("width", column.Width.Value);
                }

                columns.Add(options);

                var fieldType = GetFieldType(column.PropertyType);
                modelFields.Add(column.PropertyName, new JObject { { "type", fieldType } });
            }
            var postData = new JObject();

            if (roboUIGrid.CustomVariables.Count > 0)
            {
                foreach (var customrVar in roboUIGrid.CustomVariables)
                {
                    postData.Add(customrVar.Key, new JRaw(customrVar.Value));
                }
            }

            var dataSourceOptions = new JObject
            {
                {
                    "type",
                    new JRaw(
                        "(function(){if(kendo.data.transports['aspnetmvc-ajax']){return 'aspnetmvc-ajax';} else{throw new Error('The kendo.aspnetmvc.min.js script is not included.');}})()")
                },
                {
                    "transport", new JObject
                    {
                        {
                            "read", new JObject
                            {
                                {"url", getRecordsUrl},
                                {"dataType", "json"},
                                {"type", "POST"},
                                {"data", postData}
                            }
                        }
                    }
                },
                {"requestEnd", new JRaw("function(e){ if(e.response.callback){ eval(e.response.callback); } }")},
                {
                    "schema", new JObject
                    {
                        {"data", "results"},
                        {"total", "__count"},
                        {
                            "model", new JObject
                            {
                                {"id", "_id"},
                                {"fields", modelFields}
                            }
                        },
                    }
                },
                {"pageSize", roboUIGrid.EnablePaginate ? roboUIGrid.DefaultPageSize : int.MaxValue},
                {"serverPaging", roboUIGrid.EnablePaginate},
                {"serverFiltering", true},
                {"serverSorting", true}
            };

            if (roboUIGrid.RowActions.Count > 0 && !roboUIGrid.HideActionsColumn)
            {
                var options = new JObject(
                    new JProperty("field", "_RowActions"),
                    new JProperty("filterable", false),
                    new JProperty("sortable", false),
                    new JProperty("groupable", false),
                    new JProperty("encoded", false),
                    new JProperty("menu", false),
                    new JProperty("title", roboUIGrid.ActionsHeaderText),
                    new JProperty("headerAttributes", new JObject
                    {
                        {"style", "text-align: center;"}
                    }),
                    new JProperty("attributes", new JObject
                    {
                        {"style", "text-align: center;"}
                    }));

                if (roboUIGrid.ActionsColumnWidth.HasValue)
                {
                    options.Add("width", roboUIGrid.ActionsColumnWidth.Value);
                }

                columns.Add(options);
            }

            var dataTableOptions = new JObject
                {
                    {"dataSource", dataSourceOptions},
                    {"filterable", roboUIGrid.EnableFilterable && roboUIGrid.Columns.Any(x => x.Filterable)},
                    {"sortable", roboUIGrid.EnableSortable},
                    {"resizable", true},
                    {"columnMenu", true},
                    {"columns", columns}
                };

            if (roboUIGrid.EnablePaginate)
            {
                dataTableOptions.Add("pageable", new JObject
                {
                    {"refresh", true},
                    {"pageSize", roboUIGrid.DefaultPageSize},
                    {"buttonCount", 10}
                });
            }
            else
            {
                dataTableOptions.Add("pageable", false);
            }

            //if (roboUIGrid.RowsList != null)
            //{
            //    dataTableOptions.Add("rowList", roboUIGrid.RowsList);
            //}

            //if (roboUIGrid.ShowFooterRow)
            //{
            //    dataTableOptions.Add("footerrow", true);
            //}

            //if (roboUIGrid.CustomVariables.Count > 0)
            //{
            //    var postData = new JObject();

            //    foreach (var customrVar in roboUIGrid.CustomVariables)
            //    {
            //        postData.Add(customrVar.Key, new JRaw(customrVar.Value));
            //    }

            //    dataTableOptions.Add("postData", postData);
            //}

            //dataTableOptions.Add("jsonReader", new JObject(new JProperty("id", "_id"), new JProperty("subgrid", new JObject(new JProperty("repeatitems", false)))));

            // Sub Grid
            if (roboUIGrid.SubGrid != null)
            {
                var subGridNames = new JArray(roboUIGrid.SubGrid.Columns.Select(x => x.HeaderText));
                var subGridWidths = new JArray(roboUIGrid.SubGrid.Columns.Select(x => x.Width.HasValue ? x.Width.Value : 100));
                var subGridAligns = new JArray(roboUIGrid.SubGrid.Columns.Select(x => x.Align));
                var subGridMappings = new JArray(roboUIGrid.SubGrid.Columns.Select(x => x.PropertyName));

                var subRowActions = roboUIGrid.SubGrid.GetRowActions();
                if (subRowActions.Any())
                {
                    subGridNames.Add(roboUIGrid.SubGrid.ActionsColumnText);
                    subGridWidths.Add(roboUIGrid.SubGrid.ActionsColumnWidth);
                    subGridAligns.Add("center");
                    subGridMappings.Add("_RowActions");
                }

                var subGridModel = new JObject
                    {
                        {"name", subGridNames},
                        {"width", subGridWidths},
                        {"align", subGridAligns},
                        {"mapping", subGridMappings}
                    };

                var queryString = string.Join(string.Empty, roboUIGrid.ControllerContext.HttpContext.Request.RawUrl.Split('?').Skip(1));
                var queryStrings = HttpUtility.ParseQueryString(queryString);
                queryStrings["subGrid"] = "1";

                dataTableOptions.Add("subGrid", true);
                dataTableOptions.Add("subGridUrl", roboUIGrid.ControllerContext.HttpContext.Request.Url.GetLeftPart(UriPartial.Path) + "?" + string.Join("&", queryStrings.AllKeys.Select(x => x + "=" + HttpUtility.UrlEncode(queryStrings[x]))));
                dataTableOptions.Add("subGridModel", new JArray(subGridModel));

                if (roboUIGrid.SubGrid.Width.HasValue)
                {
                    dataTableOptions.Add("subGridWidth", roboUIGrid.SubGrid.Width.Value);
                }

                if (roboUIGrid.SubGrid.AjaxOptions != null)
                {
                    dataTableOptions.Add("ajaxSubgridOptions", roboUIGrid.SubGrid.AjaxOptions);
                }
            }

            // Tree grid
            if (roboUIGrid.TreeGridEnabled)
            {
                var treeReader = new JObject
                    {
                        { "level_field", "_level" },
                        { "parent_id_field", "_parentId" },
                        { "leaf_field", "_isLeaf" },
                        { "expanded_field", "_isExpanded" }
                    };

                dataTableOptions.Add("treeGrid", true);
                dataTableOptions.Add("treeGridModel", "adjacency");
                dataTableOptions.Add("treeReader", treeReader);
            }

            scriptRegister.IncludeInline(string.Format("$('#{0}').kendoGrid({1});", roboUIGrid.ClientId, dataTableOptions.ToString(Formatting.None)));

            if (roboUIGrid.ReloadEvents.Count > 0)
            {
                scriptRegister.IncludeInline(string.Format("$('body').bind('SystemMessageEvent', function(event){{ var events = [{1}]; if(events.indexOf(event.SystemMessage) > -1){{ $('#{0}').data('kendoGrid').dataSource.read(); }} }});", roboUIGrid.ClientId, string.Join(", ", roboUIGrid.ReloadEvents.Select(x => "'" + x + "'"))));
            }

            #endregion Kendo UI Options

            return sb.ToString();
        }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            scriptRegister.IncludeBundle("jquery-ui");
            styleRegister.IncludeBundle("jquery-ui");

            scriptRegister.IncludeBundle("jqueryval");

            //scriptRegister.IncludeBundle("fancybox");
            //styleRegister.IncludeBundle("fancybox");

            scriptRegister.IncludeBundle("kendo-ui");
            styleRegister.IncludeBundle("kendo-ui");
        }

        public override RoboUIGridRequest CreateGridRequest(ControllerContext controllerContext)
        {
            var result = new RoboUIGridRequest();
            var request = controllerContext.HttpContext.Request;

            var page = request.Form["page"];
            if (page != null)
            {
                result.PageIndex = Convert.ToInt32(page);
            }

            var pageSize = request.Form["pageSize"];
            if (pageSize != null)
            {
                result.PageSize = Convert.ToInt32(pageSize);
            }

            var bindingContext = new ModelBindingContext
            {
                ValueProvider = controllerContext.Controller.ValueProvider
            };

            string value;
            if (TryGetValue(bindingContext, "sort", out value))
                result.Sorts = GridDescriptorSerializer.Deserialize<SortDescriptor>(value);

            if (TryGetValue(bindingContext, "filter", out value))
            {
                if (!string.IsNullOrEmpty(value))
                {
                    // Process [today], [beginWeek], [endWeek], [beginMonth], [endMonth], [beginPrevMonth] tokens
                    var dtNow = DateTime.UtcNow.Date;
                    int startIndex;
                    var endIndex = 0;
                    while ((startIndex = value.IndexOf("[today", endIndex, StringComparison.Ordinal)) != -1)
                    {
                        endIndex = value.IndexOf("]", startIndex, StringComparison.Ordinal);
                        var days = value.Substring(startIndex + 6, endIndex - startIndex - 6);
                        value = value.Replace("[today" + days + "]", dtNow.AddDays(Convert.ToInt32(days)).ToString("O"));
                    }

                    value = value.Replace("[beginWeek]", dtNow.StartOfWeek(DayOfWeek.Monday).ToString("O"));
                    value = value.Replace("[endWeek]", dtNow.EndOfWeek(DayOfWeek.Sunday).AddDays(1).ToString("O"));
                    value = value.Replace("[beginMonth]", new DateTime(dtNow.Year, dtNow.Month, 1, 0, 0, 0, DateTimeKind.Utc).ToString("O"));
                    value = value.Replace("[endMonth]", new DateTime(dtNow.Year, dtNow.Month + 1, 1, 0, 0, 0, DateTimeKind.Utc).ToString("O"));
                    value = value.Replace("[beginPrevMonth]", new DateTime(dtNow.Year, dtNow.Month - 1, 1, 0, 0, 0, DateTimeKind.Utc).ToString("O"));
                    value = value.Replace("[beginYear]", new DateTime(dtNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToString("O"));
                    value = value.Replace("[endYear]", new DateTime(dtNow.Year + 1, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToString("O"));

                    result.Filters = FilterDescriptorFactory.Create(value);
                }
            }

            return result;
        }

        public override void ExecuteGridRequest<TModel>(RoboUIGridResult<TModel> roboUIGrid, RoboUIGridRequest request,
            ControllerContext controllerContext)
        {
            var formProvider = roboUIGrid.RoboUIFormProvider ?? RoboSettings.DefaultFormProvider;

            var response = controllerContext.HttpContext.Response;
            response.ContentType = "application/json";
            response.ContentEncoding = Encoding.UTF8;

            if (controllerContext.HttpContext.Request.QueryString["subGrid"] == "1")
            {
                var id = controllerContext.HttpContext.Request.Form["id"];
                var data = roboUIGrid.SubGrid.GetSubGridData(id);
                WriteJsonData(response, request, data,
                    data.Count(),
                    formProvider,
                    roboUIGrid.SubGrid.Columns,
                    roboUIGrid.SubGrid.GetRowActions(),
                    x => roboUIGrid.SubGrid.GetModelId(x), true, false, null, null);
            }
            else
            {
                var data = roboUIGrid.FetchAjaxSource(request);
                WriteJsonData(response, request, data,
                    data.TotalRecords > 0 ? data.TotalRecords : data.Count,
                    formProvider,
                    roboUIGrid.Columns.Select(x => (RoboUIGridColumn)x).ToList(),
                    roboUIGrid.RowActions.Count > 0 && !roboUIGrid.HideActionsColumn
                        ? roboUIGrid.RowActions.Select(x => (IRoboUIGridRowAction)x).ToList()
                        : new List<IRoboUIGridRowAction>(),
                    roboUIGrid.GetModelId, false, roboUIGrid.TreeGridEnabled, roboUIGrid.TreeGridParentId, roboUIGrid.TreeGridHasChildren);
            }
        }

        private static void WriteJsonData<TModelRecord>(
            HttpResponseBase response,
            RoboUIGridRequest request,
            RoboUIGridAjaxData<TModelRecord> data,
            int totalRecords,
            IRoboUIFormProvider formProvider,
            IEnumerable<RoboUIGridColumn> columns,
            IEnumerable<IRoboUIGridRowAction> rowActions,
            Func<TModelRecord, object> getModelId,
            bool isSubGrid,
            bool isTreeGrid,
            Func<TModelRecord, dynamic> getParentId,
            Func<TModelRecord, bool> hasChildren)
        {
            using (var writer = new JsonTextWriter(response.Output) { Formatting = Formatting.None })
            {
                writer.WriteStartObject();
                writer.WritePropertyName("__count");
                writer.WriteValue(totalRecords);

                if (data.Callbacks.Count > 0)
                {
                    writer.WritePropertyName("callback");
                    writer.WriteValue(string.Join("", data.Callbacks));
                }

                writer.WritePropertyName("results");

                writer.WriteStartArray();

                var needWriteValueDelimiter = false;
                foreach (var item in data)
                {
                    var jsonObject = new JObject { { "_id", Convert.ToString(getModelId(item)) } };

                    foreach (var column in columns)
                    {
                        jsonObject.Add(column.PropertyName, Convert.ToString(column.BuildHtml(item)));
                    }

                    if (rowActions.Any())
                    {
                        var sb = new StringBuilder();
                        sb.Append("<div style=\"white-space: nowrap;\">");
                        sb.Append("<ul class=\"list-inline\" style=\"margin-bottom: 0;\">");

                        foreach (var action in rowActions)
                        {
                            if (!action.IsVisible(item))
                            {
                                continue;
                            }

                            var enabled = action.IsEnabled(item);
                            var attributes = new RouteValueDictionary(action.GetAttributes(item));

                            sb.Append("<li>");
                            if (action.IsSubmitButton)
                            {
                                var value = action.GetValue(item);

                                var cssClass =
                                    (formProvider.GetButtonSizeCssClass(action.ButtonSize) + " " +
                                     formProvider.GetButtonStyleCssClass(action.ButtonStyle) + " " + action.CssClass).Trim();

                                if (!string.IsNullOrEmpty(cssClass))
                                {
                                    attributes.Add("class", cssClass);
                                }

                                if (!enabled)
                                {
                                    attributes.Add("disabled", "disabled");
                                }

                                if (!string.IsNullOrEmpty(action.ClientClickCode))
                                {
                                    attributes.Add("onclick", action.ClientClickCode);
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(action.ConfirmMessage))
                                    {
                                        attributes.Add("onclick", string.Format("return confirm('{0}');", action.ConfirmMessage));
                                    }
                                }

                                attributes.Add("id", "btn" + Guid.NewGuid().ToString("N").ToLowerInvariant());
                                attributes.Add("style", "white-space: nowrap;");

                                var tagBuilder = new TagBuilder("button");
                                tagBuilder.MergeAttribute("type", "submit");
                                tagBuilder.MergeAttribute("name", action.Name);
                                tagBuilder.InnerHtml = action.Text;
                                tagBuilder.MergeAttribute("value", Convert.ToString(value));
                                tagBuilder.MergeAttributes(attributes, true);
                                sb.Append(tagBuilder.ToString(TagRenderMode.Normal));
                            }
                            else
                            {
                                var href = action.GetUrl(item);

                                var cssClass =
                                    (formProvider.GetButtonSizeCssClass(action.ButtonSize) + " " +
                                     formProvider.GetButtonStyleCssClass(action.ButtonStyle) + " " + action.CssClass).Trim();

                                if (!string.IsNullOrEmpty(cssClass))
                                {
                                    if (enabled)
                                    {
                                        attributes.Add("class", cssClass);
                                    }
                                    else
                                    {
                                        attributes.Add("class", cssClass + " disabled");
                                    }
                                }
                                else
                                {
                                    if (!enabled)
                                    {
                                        attributes.Add("class", "disabled");
                                    }
                                }

                                attributes.Add("style", "white-space: nowrap;");

                                if (action.IsShowModalDialog && enabled)
                                {
                                    attributes.Add("data-toggle", "fancybox");
                                    attributes.Add("data-fancybox-type", "iframe");
                                    attributes.Add("data-fancybox-width", action.ModalDialogWidth);
                                }

                                var tagBuilder = new TagBuilder("a");
                                if (enabled)
                                {
                                    tagBuilder.MergeAttribute("href", href ?? "javascript:void(0)");
                                }
                                else
                                {
                                    tagBuilder.MergeAttribute("href", "javascript:void(0)");
                                }
                                tagBuilder.InnerHtml = action.Text;
                                tagBuilder.MergeAttributes(attributes, true);
                                sb.Append(tagBuilder.ToString(TagRenderMode.Normal));
                            }
                            sb.Append("</li>");
                        }

                        sb.Append("</ul>");
                        sb.Append("</div>");

                        jsonObject.Add("_RowActions", sb.ToString());
                    }
                    else
                    {
                        jsonObject.Add("_RowActions", null);
                    }

                    if (needWriteValueDelimiter)
                    {
                        writer.WriteRaw(",");
                    }

                    writer.WriteRaw(jsonObject.ToString());
                    needWriteValueDelimiter = true;
                }

                if (isSubGrid && data.UserData.ContainsKey("_RowActions"))
                {
                    if (needWriteValueDelimiter)
                    {
                        writer.WriteRaw(",");
                    }

                    writer.WriteStartObject();
                    writer.WritePropertyName("_RowActions");
                    writer.WriteValue(data.UserData["_RowActions"]);
                    writer.WriteEndObject();

                    data.UserData.Remove("_RowActions");
                }

                writer.WriteEndArray();

                if (data.UserData.Count > 0)
                {
                    writer.WritePropertyName("userdata");

                    writer.WriteStartObject();

                    foreach (var item in data.UserData)
                    {
                        writer.WritePropertyName(item.Key);
                        writer.WriteValue(item.Value);
                    }

                    writer.WriteEndObject();
                }

                writer.WriteEndObject();
                writer.Flush();
            }
        }

        private static bool TryGetValue<T>(ModelBindingContext bindingContext, string key, out T result)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(key);
            if (valueProviderResult == null)
            {
                result = default(T);
                return false;
            }

            result = (T)valueProviderResult.ConvertTo(typeof(T));
            return true;
        }

        private static string GetFieldType(Type propertyType)
        {
            var localType = propertyType;

        LabelBegin:

            if (localType.IsEnum)
            {
                return "string";
            }

            switch (Type.GetTypeCode(localType))
            {
                case TypeCode.Boolean:
                    return "string";

                case TypeCode.DateTime:
                    return "string";

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return "number";

                case TypeCode.Object:
                    localType = Nullable.GetUnderlyingType(propertyType);
                    if (localType == null || localType.FullName == "System.Guid")
                    {
                        return "string";
                    }
                    goto LabelBegin;
                default:
                    return "string";
            }
        }

        private static class GridDescriptorSerializer
        {
            public static IList<T> Deserialize<T>(string from) where T : IDescriptor, new()
            {
                var list = new List<T>();
                if (string.IsNullOrEmpty(from))
                    return list;

                foreach (var source in from.Split("~".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    var obj = new T();
                    obj.Deserialize(source);
                    list.Add(obj);
                }
                return list;
            }
        }
    }
}
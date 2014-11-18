using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class JQGridRoboUIGridProvider : BaseRoboUIGridProvider
    {
        public override string Render<TModel>(RoboUIGridResult<TModel> roboUIGrid, HtmlHelper htmlHelper)
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
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

            Write(sb, string.Format("<table class=\"{0}\" id=\"{1}\"></table>", roboUIGrid.CssClass, roboUIGrid.ClientId));

            Write(sb, string.Format("<div id=\"{0}_Pager\"></div>", roboUIGrid.ClientId));

            // Hidden values
            foreach (var hiddenValue in roboUIGrid.HiddenValues)
            {
                Write(sb, string.Format("<input type=\"hidden\" name=\"{0}\" id=\"{0}\" value=\"{1}\"/>", hiddenValue.Key, HttpUtility.HtmlEncode(hiddenValue.Value)));
            }

            if (!string.IsNullOrEmpty(form))
            {
                Write(sb, "</form>");
            }

            if (!roboUIGrid.DisableBlockUI && roboUIGrid.IsAjaxSupported)
            {
                // Block UI
                Write(sb, "<div class=\"blockUI\" style=\"display:none; z-index: 100; border: none; margin: 0; padding: 0; width: 100%; height: 100%; top: 0; left: 0; background-color: #000000; opacity: 0.05; filter: alpha(opacity = 5); cursor: wait; position: absolute;\"></div>");

                // Block Msg
                Write(sb, "<div class=\"blockUIMsg\" style=\"display:none;\">Processing...</div>");

                scriptRegister.IncludeInline("$(document).bind(\"ajaxSend\", function(){ $(\".blockUI, .blockUIMsg\").show(); }).bind(\"ajaxComplete\", function(){ $(\".blockUI, .blockUIMsg\").hide(); });", true);
            }

            // End div container
            Write(sb, "</div>");

            if (!string.IsNullOrEmpty(roboUIGrid.GridWrapperEndHtml))
            {
                Write(sb, roboUIGrid.GridWrapperEndHtml);
            }

            #region jqGrid Options

            var dataTableOptions = new JObject
                {
                    {"rowNum", roboUIGrid.EnablePaginate ? roboUIGrid.DefaultPageSize : int.MaxValue},
                    {"autowidth", true},
                    {"caption", roboUIGrid.Title},
                    {"viewrecords", true},
                    {"loadonce", false},
                    {"userDataOnFooter", true},
                    {"hidegrid", roboUIGrid.EnableShowHideGrid},
                    {"height", "100%"},
                    {"recordpos", roboUIGrid.RecordsInfoPosition},
                    {"multiselect", roboUIGrid.EnableCheckboxes},
                    {"loadComplete", new JRaw(string.Format("function(data){{ if({1} && data.records === 0) {{ $('#{0}_Pager_center').hide(); }} else {{ $('#{0}_Pager_center').show(); }} var width = $('#{0}_Container').width(); $('#{0}').setGridWidth(width); if(data.callback){{ eval(data.callback); }} }}", roboUIGrid.ClientId, roboUIGrid.HidePagerWhenEmpty ? "true" : "false"))}
                };

            if (!roboUIGrid.DisableBlockUI)
            {
                dataTableOptions.Add("loadui", "disable");
            }

            if (roboUIGrid.EnablePaginate)
            {
                dataTableOptions.Add("pager", string.Format("#{0}_Pager", roboUIGrid.ClientId));
            }

            if (roboUIGrid.RowsList != null)
            {
                dataTableOptions.Add("rowList", roboUIGrid.RowsList);
            }

            if (roboUIGrid.ShowFooterRow)
            {
                dataTableOptions.Add("footerrow", true);
            }

            var colNames = new JArray();
            var colModel = new JArray();

            foreach (var column in roboUIGrid.Columns)
            {
                colNames.Add(column.HeaderText);
                var options = new JObject(
                    new JProperty("name", column.PropertyName),
                    new JProperty("index", column.PropertyName),
                    new JProperty("align", column.Align),
                    new JProperty("sortable", roboUIGrid.EnableSortable && column.Sortable));

                if (column.Width.HasValue)
                {
                    options.Add(new JProperty("width", column.Width));
                    options.Add(new JProperty("fixed", true));
                }

                if (!string.IsNullOrEmpty(column.CssClass))
                {
                    options.Add(new JProperty("classes", column.CssClass));
                }

                if (column.Filterable)
                {
                    options.Add(new JProperty("search", true));
                    var typeCode = Type.GetTypeCode(column.PropertyType);

                    switch (typeCode)
                    {
                        case TypeCode.Boolean:
                            options.Add(new JProperty("stype", "select"));
                            options.Add(new JProperty("editoptions", new JObject(new JProperty("value", ":All;true:Yes;false:No"))));
                            break;

                        case TypeCode.String:
                            options.Add(new JProperty("stype", "text"));
                            options.Add(new JProperty("searchoptions", new JObject(new JProperty("sopt", new JArray("cn")))));
                            break;

                        default:
                            throw new NotSupportedException();
                    }
                }
                else
                {
                    options.Add(new JProperty("search", false));
                }

                colModel.Add(options);
            }

            if (roboUIGrid.RowActions.Count > 0)
            {
                if (!roboUIGrid.HideActionsColumn)
                {
                    colNames.Add(roboUIGrid.ActionsHeaderText);
                    var options = new JObject(
                        new JProperty("name", "_RowActions"),
                        new JProperty("align", "center"),
                        new JProperty("index", "_RowActions"),
                        new JProperty("cellattr", new JRaw("function(){ return 'title=\"\"'; }")),
                        new JProperty("search", false),
                        new JProperty("sortable", false));

                    if (roboUIGrid.ActionsColumnWidth.HasValue)
                    {
                        options.Add(new JProperty("width", roboUIGrid.ActionsColumnWidth.Value));
                        options.Add(new JProperty("fixed", true));
                    }

                    colModel.Add(options);
                }
            }

            dataTableOptions.Add("colNames", colNames);
            dataTableOptions.Add("colModel", colModel);

            if (roboUIGrid.CustomVariables.Count > 0)
            {
                var postData = new JObject();

                foreach (var customrVar in roboUIGrid.CustomVariables)
                {
                    postData.Add(customrVar.Key, new JRaw(customrVar.Value));
                }

                dataTableOptions.Add("postData", postData);
            }

            dataTableOptions.Add("datatype", "json");
            dataTableOptions.Add("jsonReader", new JObject(new JProperty("id", "_id"), new JProperty("subgrid", new JObject(new JProperty("repeatitems", false)))));
            dataTableOptions.Add("mtype", "POST");
            dataTableOptions.Add("url", string.IsNullOrEmpty(roboUIGrid.GetRecordsUrl)
                ? roboUIGrid.ControllerContext.HttpContext.Request.RawUrl
                : roboUIGrid.GetRecordsUrl);

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

            scriptRegister.IncludeInline(string.Format("$('#{0}').jqGrid({1});", roboUIGrid.ClientId, dataTableOptions.ToString(Formatting.None)));

            if (roboUIGrid.EnableSearch && roboUIGrid.Columns.Any(x => x.Filterable))
            {
                scriptRegister.IncludeInline(string.Format("$('#{0}').jqGrid('filterToolbar', {{ stringResult: true }});", roboUIGrid.ClientId));
            }

            if (roboUIGrid.ReloadEvents.Count > 0)
            {
                scriptRegister.IncludeInline(string.Format("$('body').bind('SystemMessageEvent', function(event){{ var events = [{1}]; if(events.indexOf(event.SystemMessage) > -1){{ $('#{0}').jqGrid().trigger('reloadGrid'); }} }});", roboUIGrid.ClientId, string.Join(", ", roboUIGrid.ReloadEvents.Select(x => "'" + x + "'"))));
            }

            // Resize window event
            scriptRegister.IncludeInline(string.Format("$(window).resize(function(){{ var width = $('#{0}_Container').width(); $('#{0}').setGridWidth(width); }});", roboUIGrid.ClientId));

            #endregion jqGrid Options

            return sb.ToString();
        }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            scriptRegister.IncludeBundle("jquery-ui");
            styleRegister.IncludeBundle("jquery-ui");

            scriptRegister.IncludeBundle("jquery-validate");

            //scriptRegister.IncludeBundle("fancybox");
            //styleRegister.IncludeBundle("fancybox");

            scriptRegister.IncludeBundle("jqgrid");
            styleRegister.IncludeBundle("jqgrid");
        }

        public override RoboUIGridRequest CreateGridRequest(ControllerContext controllerContext)
        {
            var result = new RoboUIGridRequest();
            var request = controllerContext.HttpContext.Request;

            var rows = request.Form["rows"];
            if (rows != null)
            {
                result.PageSize = Convert.ToInt32(rows);
            }

            var page = request.Form["page"];
            if (page != null)
            {
                result.PageIndex = Convert.ToInt32(page);
            }

            if (result.PageIndex < 1)
            {
                result.PageIndex = 1;
            }

            result.NodeId = request.Form["nodeid"];
            if (request.Form.AllKeys.Contains("n_level"))
            {
                var level = request.Form["n_level"];
                if (string.IsNullOrEmpty(level))
                {
                    result.NodeLevel = -1;
                }
                else
                {
                    result.NodeLevel = Convert.ToInt32(request.Form["n_level"]);
                }
            }
            else
            {
                result.NodeLevel = -1;
            }

            if (!string.IsNullOrEmpty(request.Form["sidx"]))
            {
                result.Sorts = new List<SortDescriptor>
                {
                    new SortDescriptor
                    {
                        Member = request.Form["sidx"],
                        SortDirection = request.Form["sord"] == "asc" ? ListSortDirection.Ascending : ListSortDirection.Descending
                    }
                };
            }

            var isSearch = request.Form["_search"];
            if (isSearch == "true")
            {
                result.Filters = new List<IFilterDescriptor>();

                var filters = JObject.Parse(request.Form["filters"]);
                var rules = (JArray)filters["rules"];
                foreach (var rule in rules)
                {
                    switch (rule["op"].Value<string>())
                    {
                        case "eq":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.IsEqualTo,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "ne":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.IsNotEqualTo,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "lt":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.IsLessThan,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "le":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.IsLessThanOrEqualTo,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "gt":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.IsGreaterThan,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "ge":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.IsGreaterThanOrEqualTo,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "bw":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.StartsWith,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "bn":

                            break;

                        case "in":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.IsContainedIn,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "ni":

                            break;

                        case "ew":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.EndsWith,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "en":

                            break;

                        case "cn":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.Contains,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        case "nc":
                            result.Filters.Add(new FilterDescriptor
                            {
                                Member = rule["field"].Value<string>(),
                                Operator = FilterOperator.DoesNotContain,
                                Value = rule["data"].Value<string>()
                            });
                            break;

                        default: throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return result;
        }

        public override void ExecuteGridRequest<TModel>(RoboUIGridResult<TModel> roboUIGrid, RoboUIGridRequest request, ControllerContext controllerContext)
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

                writer.WritePropertyName("page");
                writer.WriteValue(request.PageIndex);

                writer.WritePropertyName("records");
                writer.WriteValue(totalRecords);

                writer.WritePropertyName("total");
                writer.WriteValue((int)Math.Ceiling((totalRecords * 1d) / request.PageSize));

                if (data.Callbacks.Count > 0)
                {
                    writer.WritePropertyName("callback");
                    writer.WriteValue(string.Join("", data.Callbacks));
                }

                writer.WritePropertyName("rows");

                writer.WriteStartArray();

                var needWriteValueDelimiter = false;
                foreach (TModelRecord item in data)
                {
                    var jsonObject = new JObject { { "_id", Convert.ToString(getModelId(item)) } };

                    foreach (var column in columns)
                    {
                        jsonObject.Add(column.PropertyName, Convert.ToString(column.BuildHtml(item)));
                    }

                    if (isTreeGrid)
                    {
                        jsonObject.Add("_level", request.NodeLevel + 1);
                        jsonObject.Add("_parentId", getParentId(item));
                        jsonObject.Add("_isLeaf", !hasChildren(item));
                        jsonObject.Add("_isExpanded", false);
                    }

                    if (rowActions.Any())
                    {
                        var sb = new StringBuilder();
                        sb.Append("<table style=\"margin-left: auto; margin-right: auto; border: none; padding: 0;\">");
                        sb.Append("<tr>");

                        foreach (var action in rowActions)
                        {
                            if (!action.IsVisible(item))
                            {
                                continue;
                            }

                            var enabled = action.IsEnabled(item);
                            var attributes = new RouteValueDictionary(action.GetAttributes(item));

                            sb.Append("<td style=\"border: none; background-color: transparent; padding: 0 2px;\">");
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
                            sb.Append("</td>");
                        }

                        sb.Append("</tr>");
                        sb.Append("</table>");

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
    }
}
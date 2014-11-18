//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Web.Mvc;
//using Kore.Web.Mvc.JQGrid.Utility;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;

//namespace Kore.Web.Mvc.JQGrid
//{
//    public class JQGridResult<T> : ActionResult
//    {
//        private Func<T, dynamic> getId;

//        public JQGridResult(Func<T, dynamic> getId)
//        {
//            Actions = new List<Func<T, string>>();
//            Columns = new List<RenderedColumn>();
//            this.getId = getId;
//            ApplyDefaultActionRowStyle = true;
//        }

//        public List<Func<T, string>> Actions { get; set; }

//        public bool ApplyDefaultActionRowStyle { get; set; }

//        public IEnumerable<T> Records { get; set; }

//        public JQGridAjaxRequest Request { get; set; }

//        public int TotalRecords { get; set; }

//        private List<RenderedColumn> Columns { get; set; }

//        /// <summary>
//        /// Helper method to add buttons to the actions column
//        /// </summary>
//        /// <param name="linkText">The inner text of the button element.</param>
//        /// <param name="buttonName">Use this in conjunction with TableButtonAttribute to specify which action method should be used.</param>
//        /// <param name="idSelector">Used to get the ID of the records in each row</param>
//        /// <param name="htmlAttributes">Custom HTML attributes</param>
//        public void AddActionButton(string linkText, string buttonName, Func<T, dynamic> idSelector, object htmlAttributes)
//        {
//            Actions.Add(x =>
//                {
//                    string id = idSelector(x).ToString();

//                    return new FluentTagBuilder("button")
//                        .SetInnerText(linkText)
//                        .MergeAttribute("name", buttonName)
//                        .MergeAttribute("type", "submit")
//                        .MergeAttribute("value", id)
//                        .MergeAttribute("id", Guid.NewGuid().ToString("N").ToLowerInvariant())
//                        .MergeAttributes(htmlAttributes)
//                        .ToString();
//                });
//        }

//        public void AddColumn(Expression<Func<T, dynamic>> column, Func<T, string> htmlBuilder = null)
//        {
//            Columns.Add(new RenderedColumn { Column = column, HtmlBuilder = htmlBuilder });
//        }

//        public override void ExecuteResult(ControllerContext context)
//        {
//            var jsonData = new JObject
//            {
//                { "total", TotalRecords / Request.PageSize + 1 },
//                { "page", Request.PageIndex },
//                { "records", TotalRecords }
//            };

//            var rows = new JArray();

//            foreach (var record in Records)
//            {
//                var item = new JObject
//                {
//                    { "id", getId(record) }
//                };

//                var cell = new JObject();

//                foreach (var column in Columns)
//                {
//                    string propertyName = Utils.GetFullPropertyName(column.Column);

//                    if (column.HtmlBuilder == null)
//                    {
//                        var value = column.Column.Compile()(record);
//                        cell.Add(new JProperty(propertyName, value));
//                    }
//                    else
//                    {
//                        cell.Add(new JProperty(propertyName, column.HtmlBuilder(record)));
//                    }
//                }

//                if (Actions.Any())
//                {
//                    var sb = new StringBuilder();

//                    if (ApplyDefaultActionRowStyle)
//                    {
//                        sb.Append("<table>");
//                        sb.Append("<tr>");

//                        foreach (var action in Actions)
//                        {
//                            sb.Append("<td style=\"border:none;padding:0px 2px;\">");
//                            sb.Append(action(record));
//                            sb.Append("</td>");
//                        }

//                        sb.Append("</tr>");
//                        sb.Append("</table>");
//                    }
//                    else
//                    {
//                        foreach (var action in Actions)
//                        {
//                            sb.Append(action(record));
//                        }
//                    }

//                    cell.Add("_RowActions", sb.ToString());
//                }
//                else
//                {
//                    cell.Add("_RowActions", null);
//                }

//                item.Add(new JProperty("cell", cell));
//                rows.Add(item);
//            }

//            jsonData.Add(new JProperty("rows", rows));

//            var response = context.HttpContext.Response;
//            response.ContentType = "application/json";

//            var writer = new JsonTextWriter(response.Output) { Formatting = Formatting.None };
//            var serializer = JsonSerializer.Create();
//            serializer.Serialize(writer, jsonData);

//            writer.Flush();
//        }

//        private class RenderedColumn
//        {
//            public Expression<Func<T, dynamic>> Column { get; set; }

//            public Func<T, string> HtmlBuilder { get; set; }
//        }
//    }
//}
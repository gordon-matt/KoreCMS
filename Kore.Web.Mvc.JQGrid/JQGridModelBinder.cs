//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Json;
//using System.Text;
//using System.Web.Mvc;

//namespace Kore.Web.Mvc.JQGrid
//{
//    public class GridModelBinder : IModelBinder
//    {
//        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
//        {
//            try
//            {
//                var request = controllerContext.HttpContext.Request;
//                return new JQGridAjaxRequest
//                {
//                    IsSearch = bool.Parse(request["_search"] ?? "false"),
//                    PageIndex = int.Parse(request["page"] ?? "1"),
//                    PageSize = int.Parse(request["rows"] ?? "10"),
//                    SortColumn = request["sidx"] ?? "",
//                    SortOrder = request["sord"] ?? "asc",
//                    Filter = Filter.Create(request["filters"] ?? "")
//                };
//            }
//            catch
//            {
//                return null;
//            }
//        }
//    }

//    [DataContract]
//    public class Filter
//    {
//        [DataMember(Name = "groupOp")]
//        public string GroupOp { get; set; }

//        [DataMember(Name = "rules")]
//        public Rule[] Rules { get; set; }

//        public static Filter Create(string jsonData)
//        {
//            try
//            {
//                var serializer = new DataContractJsonSerializer(typeof(Filter));
//                //var ms = new System.IO.MemoryStream(Encoding.Default.GetBytes(jsonData));
//                var ms = new System.IO.MemoryStream(
//                    Encoding.Convert(
//                        Encoding.Default,
//                        Encoding.UTF8,
//                        Encoding.Default.GetBytes(jsonData)));
//                return serializer.ReadObject(ms) as Filter;
//            }
//            catch
//            {
//                return null;
//            }
//        }
//    }

//    [ModelBinder(typeof(GridModelBinder))]
//    public class JQGridAjaxRequest
//    {
//        public Filter Filter { get; set; }

//        public bool IsSearch { get; set; }

//        public int PageIndex { get; set; }

//        public int PageSize { get; set; }

//        public string SortColumn { get; set; }

//        public string SortOrder { get; set; }
//    }

//    [DataContract]
//    public class Rule
//    {
//        [DataMember(Name = "field")]
//        public string Field { get; set; }

//        [DataMember(Name = "op")]
//        public string Operator { get; set; }

//        [DataMember(Name = "data")]
//        public string Value { get; set; }
//    }
//}
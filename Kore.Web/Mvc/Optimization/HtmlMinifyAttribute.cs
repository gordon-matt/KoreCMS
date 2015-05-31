//using System.Text.RegularExpressions;
//using System.Web.Mvc;
//using Kore.IO;

//namespace Kore.Web.Mvc.Optimization
//{
//    // Thanks to: Arran Maclean (https://arranmaclean.wordpress.com/2010/08/10/minify-html-with-net-mvc-actionfilter/)
//    public class HtmlMinifyAttribute : ActionFilterAttribute
//    {
//        public override void OnActionExecuting(ActionExecutingContext filterContext)
//        {
//            var request = filterContext.HttpContext.Request;
//            var response = filterContext.HttpContext.Response;

//            response.Filter = new FilteredStream(response.Filter, s =>
//            {
//                s = Regex.Replace(s, @"\s+", " ");
//                s = Regex.Replace(s, @"\s*\n\s*", "\n");
//                s = Regex.Replace(s, @"\s*\>\s*\<\s*", "><");
//                s = Regex.Replace(s, @"<!--(.*?)-->", "");   //Remove comments

//                // single-line doctype must be preserved
//                var firstEndBracketPosition = s.IndexOf(">");
//                if (firstEndBracketPosition >= 0)
//                {
//                    s = s.Remove(firstEndBracketPosition, 1);
//                    s = s.Insert(firstEndBracketPosition, ">");
//                }
//                return s;
//            });
//        }
//    }
//}
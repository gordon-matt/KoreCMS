//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;
//using Kore.Web.ContentManagement.Areas.Admin.Messaging.Models;

//namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Services
//{
//    public class UrlContentParserEngine : IParserEngine
//    {
//        public int Priority { get { return 5; } }

//        public string ParseTemplate(string template, ParseTemplateContext context, WebWorkContext workContext)
//        {
//            var urlTemplates = new List<string>();
//            int index;
//            var startPosition = 0;
//            do
//            {
//                index = template.IndexOf("[Url:", startPosition, StringComparison.InvariantCulture);
//                if (index > -1)
//                {
//                    var endPosition = template.IndexOf("]", index, StringComparison.InvariantCulture);
//                    if (endPosition > -1)
//                    {
//                        urlTemplates.Add(template.Substring(index, endPosition - index + 1));
//                        startPosition = endPosition;
//                    }
//                    else
//                    {
//                        startPosition = index + 1;
//                    }
//                }
//            } while (index > -1);

//            urlTemplates = urlTemplates.Distinct().ToList();

//            if (urlTemplates.Count <= 0) return template;

//            var templateContent = new StringBuilder(template);

//            var tasks = urlTemplates.Select(x => Task.Run(async () =>
//            {
//                var url = x.Replace("[Url:", "").Trim(' ', ']');
//                if (!string.IsNullOrEmpty(url))
//                {
//                    var client = new HttpClient();
//                    var content = await client.GetStringAsync(url);
//                    if (content.Contains("</body>"))
//                    {
//                        content = ExtractHtmlBody(content);
//                    }
//                    templateContent.Replace(x, content);
//                }
//            })).ToArray();
//            Task.WaitAll(tasks);

//            return templateContent.ToString();
//        }

//        /// <summary>
//        /// Extract html body content from html
//        /// </summary>
//        /// <param name="html"></param>
//        /// <returns></returns>
//        private static string ExtractHtmlBody(string html)
//        {
//            var start = html.IndexOf("<body", StringComparison.InvariantCultureIgnoreCase);

//            if (start <= -1) return html;

//            start = html.IndexOf(">", start + 1, StringComparison.InvariantCultureIgnoreCase);
//            if (start <= -1) return html;

//            var end = html.LastIndexOf("</body>", html.Length, StringComparison.InvariantCultureIgnoreCase);
//            return end > -1 ? html.Substring(start + 1, end - start - 1) : html;
//        }
//    }
//}
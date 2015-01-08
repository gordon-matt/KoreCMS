using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kore.Web.ContentManagement.Areas.Admin.Messaging.Models;

namespace Kore.Web.ContentManagement.Messaging.Services
{
    public class SimpleTextParserEngine : IParserEngine
    {
        public int Priority { get { return 5; } }

        public string ParseTemplate(string template, ParseTemplateContext context, WebWorkContext workContext)
        {
            if (context.ViewBag != null)
            {
                var variables = context.ViewBag as IDictionary<string, object>;
                if (variables != null)
                {
                    var templateContent = new StringBuilder(template);
                    templateContent = variables.Aggregate(templateContent, (current, variable) => current.Replace(string.Format("[{0}]", variable.Key), Convert.ToString(variable.Value)));
                    return templateContent.ToString();
                }
            }
            return template;
        }
    }
}
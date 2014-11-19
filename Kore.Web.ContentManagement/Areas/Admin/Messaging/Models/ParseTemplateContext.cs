using System.Collections.Generic;
using System.Dynamic;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Models
{
    public class ParseTemplateContext
    {
        public ParseTemplateContext()
        {
            ViewBag = new ExpandoObject();
        }

        public object Model { get; set; }

        public dynamic ViewBag { get; set; }

        public void AppendToViewBag(string prefix, object obj)
        {
            if (obj == null)
            {
                return;
            }

            var properties = obj.GetType().GetProperties();
            var data = (IDictionary<string, object>)ViewBag;
            foreach (var propertyInfo in properties)
            {
                data[prefix + propertyInfo.Name] = propertyInfo.GetValue(obj);
            }
        }
    }
}
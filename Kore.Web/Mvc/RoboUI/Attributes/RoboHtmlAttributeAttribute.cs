using System;

namespace Kore.Web.Mvc.RoboUI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RoboHtmlAttributeAttribute : Attribute
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public RoboHtmlAttributeAttribute(string name, object value)
        {
            Name = name;
            Value = value.ToString();
        }
    }
}
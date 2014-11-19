using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboGridAttribute : RoboControlAttribute
    {
        private readonly byte defaultRows;
        private List<RoboControlAttribute> attributes;

        public RoboGridAttribute()
        {
            ShowTableHead = true;
            EnabledScroll = false;
        }

        public RoboGridAttribute(byte minRows, byte maxRows)
            : this()
        {
            ShowRowsControl = true;
            defaultRows = minRows;
            MinRows = minRows;
            MaxRows = maxRows;
        }

        public RoboGridAttribute(byte minRows, byte maxRows, byte defaultRows)
            : this()
        {
            if (maxRows < minRows || maxRows == 0)
            {
                throw new ArgumentOutOfRangeException("maxRows");
            }

            if (defaultRows < minRows || defaultRows > maxRows)
            {
                throw new ArgumentOutOfRangeException("defaultRows");
            }

            ShowRowsControl = true;
            this.defaultRows = defaultRows;
            MinRows = minRows;
            MaxRows = maxRows;
        }

        public byte DefaultRows
        {
            get { return defaultRows; }
        }

        public ICollection<RoboControlAttribute> Attributes
        {
            get { return attributes; }
        }

        public bool EnabledScroll { get; set; }

        public override bool HasLabelControl
        {
            get { return ShowLabelControl; }
        }

        public byte MaxRows { get; set; }

        public byte MinRows { get; set; }

        public bool ShowAsStack { get; set; }

        public bool ShowLabelControl { get; set; }

        public bool ShowRowsControl { get; set; }

        public bool ShowTableHead { get; set; }

        public string TableHeadHtml { get; set; }

        public void EnsureProperties()
        {
            //TODO ?
            //if (typeof(IRoboFormProvider).IsAssignableFrom(PropertyType))
            //{
            //    var value = (IRoboFormProvider)Value;
            //    attributes = new List<RoboControlAttribute>(value.GetAttributes());

            //    foreach (var attribute in attributes)
            //    {
            //        if (string.IsNullOrEmpty(attribute.PropertyName))
            //        {
            //            attribute.PropertyName = attribute.Name;
            //        }
            //    }

            //    return;
            //}

            if (attributes == null)
            {
                if (!typeof(IEnumerable).IsAssignableFrom(PropertyType) || !PropertyType.IsGenericType)
                {
                    throw new NotSupportedException("Cannot apply robo grid for non enumerable property as grid.");
                }

                var type = PropertyType.GetGenericArguments()[0];
                attributes = new List<RoboControlAttribute>();

                foreach (var propertyInfo in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    var attribute = propertyInfo.GetCustomAttribute<RoboControlAttribute>(false);
                    if (attribute != null)
                    {
                        attribute.Name = propertyInfo.Name;
                        attribute.PropertyName = propertyInfo.Name;
                        attribute.PropertyType = propertyInfo.PropertyType;
                        if (attribute.LabelText == null)
                        {
                            attribute.LabelText = propertyInfo.Name;
                        }
                        attributes.Add(attribute);
                    }
                }

                attributes.Sort((x, y) => x.Order.CompareTo(y.Order));
            }
        }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            EnsureProperties();
            attributes.ForEach(roboAttribute => roboAttribute.GetAdditionalResources(scriptRegister, styleRegister));
        }

        public static object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
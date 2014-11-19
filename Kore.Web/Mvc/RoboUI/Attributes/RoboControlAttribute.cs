using System;
using System.Collections.Generic;
using System.Reflection;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RoboControlAttribute : Attribute, ICloneable<RoboControlAttribute>
    {
        private IDictionary<string, object> htmlAttributes;
        private bool hasLabelControl;

        public RoboControlAttribute()
        {
            hasLabelControl = true;
            ContainerRowIndex = -100;
            ContainerCssClass = "col-md-12";
        }

        public string AppendText { get; set; }

        public int ColumnWidth { get; set; }

        public string ContainerCssClass { get; set; }

        public string ContainerDataBind { get; set; }

        public int ContainerRowIndex { get; set; }

        public string ControlContainerCssClass { get; set; }

        public string ControlSpan { get; set; }

        public virtual bool HasLabelControl
        {
            get { return hasLabelControl; }
            set { hasLabelControl = value; }
        }

        public string HelpText { get; set; }

        public virtual bool HideLabelControl { get; set; }

        public IDictionary<string, object> HtmlAttributes
        {
            get { return htmlAttributes ?? (htmlAttributes = new Dictionary<string, object>()); }
            set { htmlAttributes = value; }
        }

        public string Id { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsRequired { get; set; }

        public string LabelCssClass { get; set; }

        public string LabelText { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public short Order { get; set; }

        public string PrependText { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public string PropertyName { get; set; }

        public Type PropertyType { get; set; }

        public string Title { get; set; }

        public object Value { get; set; }

        public virtual void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
        }

        #region Implementation of ICloneable<RoboUIHtmlControlAttribute>

        public RoboControlAttribute DeepCopy()
        {
            return ShallowCopy();
        }

        public RoboControlAttribute ShallowCopy()
        {
            return (RoboControlAttribute)MemberwiseClone();
        }

        #endregion Implementation of ICloneable<RoboUIHtmlControlAttribute>
    }
}
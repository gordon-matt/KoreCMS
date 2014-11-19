using System;
using System.Collections.Generic;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUIFormAction : BaseRoboUIAction<RoboUIFormAction>
    {
        private readonly List<MenuItem> menuItems;

        public RoboUIFormAction()
            : this(false, false)
        {
        }

        public RoboUIFormAction(bool isSubmitButton, bool isValidationSupported)
            : base(isSubmitButton, isValidationSupported)
        {
            menuItems = new List<MenuItem>();
        }

        public Func<string> HtmlBuilder { get; set; }

        public string Url { get; set; }

        public string Value { get; set; }

        public byte Order { get; set; }

        public List<MenuItem> MenuItems
        {
            get { return menuItems; }
        }

        public RoboUIFormAction AddMenuItem(string linkText, string href, string onClick = null, bool hasDividerAfter = false, byte order = 0)
        {
            menuItems.Add(new MenuItem
            {
                LinkText = linkText,
                Href = href,
                OnClick = onClick,
                HasDividerAfter = hasDividerAfter,
                Order = order
            });

            return this;
        }

        public RoboUIFormAction HasHtmlBuilder(Func<string> htmlBuilder)
        {
            HtmlBuilder = htmlBuilder;
            return this;
        }

        public RoboUIFormAction HasUrl(string value)
        {
            IsSubmitButton = false;
            Url = value;
            return this;
        }

        public RoboUIFormAction HasValue(string value)
        {
            Value = value;
            return this;
        }

        public RoboUIFormAction HasOrder(byte value)
        {
            Order = value;
            return this;
        }
    }

    public class MenuItem
    {
        public string LinkText { get; set; }

        public string Href { get; set; }

        public string OnClick { get; set; }

        public byte Order { get; set; }

        public bool HasDividerAfter { get; set; }
    }
}
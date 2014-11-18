using System.Collections.Generic;

namespace Kore.Web.Mvc.RoboUI
{
    public abstract class BaseRoboUIAction<T> where T : BaseRoboUIAction<T>
    {
        protected BaseRoboUIAction(bool isSubmitButton, bool isValidationSupported)
        {
            IsSubmitButton = isSubmitButton;
            IsValidationSupported = isValidationSupported;
            HtmlAttributes = new Dictionary<string, string>();
        }

        public ButtonSize ButtonSize { get; set; }

        public ButtonStyle ButtonStyle { get; set; }

        public string ClientClickCode { get; set; }

        public string ClientId { get; set; }

        public string ConfirmMessage { get; set; }

        public string CssClass { get; set; }

        public string Description { get; set; }

        public Dictionary<string, string> HtmlAttributes { get; set; }

        public string IconCssClass { get; set; }

        public bool IsShowModalDialog { get; set; }

        public bool IsSubmitButton { get; set; }

        public bool IsValidationSupported { get; set; }

        public int ModalDialogWidth { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }

        public T HasAttribute(string key, string value)
        {
            HtmlAttributes.Add(key, value);
            return (T)this;
        }

        public T HasButtonSize(ButtonSize buttonSize)
        {
            ButtonSize = buttonSize;
            return (T)this;
        }

        public T HasButtonStyle(ButtonStyle style)
        {
            ButtonStyle = style;
            return (T)this;
        }

        public T HasClientId(string id)
        {
            ClientId = id;
            return (T)this;
        }

        public T HasConfirmMessage(string value)
        {
            ConfirmMessage = value;
            return (T)this;
        }

        public T HasCssClass(string value)
        {
            CssClass = value;
            return (T)this;
        }

        public T HasDescription(string value)
        {
            Description = value;
            return (T)this;
        }

        public T HasIconCssClass(string value)
        {
            IconCssClass = value;
            return (T)this;
        }

        public T HasName(string value)
        {
            Name = value;
            return (T)this;
        }

        public T HasText(string value)
        {
            Text = value;
            return (T)this;
        }

        public T OnClientClick(string jsCode)
        {
            ClientClickCode = jsCode;
            return (T)this;
        }

        public T ShowModalDialog(int width = 600)
        {
            IsShowModalDialog = true;
            ModalDialogWidth = width;
            return (T)this;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Kore.Web.Mvc.RoboUI
{
    public interface IRoboUIGridRowAction
    {
        string Name { get; }

        string Text { get; }

        bool IsSubmitButton { get; }

        bool IsShowModalDialog { get; }

        int ModalDialogWidth { get; }

        ButtonSize ButtonSize { get; }

        ButtonStyle ButtonStyle { get; }

        string CssClass { get; }

        string ConfirmMessage { get; }

        string ClientClickCode { get; set; }

        bool IsVisible(object item);

        bool IsEnabled(object item);

        object GetValue(object item);

        string GetUrl(object item);

        IDictionary<string, object> GetAttributes(object obj);
    }

    public class RoboUIGridRowAction<TModel> : BaseRoboUIAction<RoboUIGridRowAction<TModel>>, IRoboUIGridRowAction
    {
        private readonly IDictionary<string, Func<TModel, object>> attributes;

        public RoboUIGridRowAction(bool isSubmitButton, bool isAjaxSupport)
            : base(isSubmitButton, isAjaxSupport)
        {
            attributes = new Dictionary<string, Func<TModel, object>>();
        }

        public Func<TModel, object> ValueSelector { get; set; }

        public Func<TModel, string> UrlBuilder { get; set; }

        public Func<TModel, bool> VisibleWhenFunc { get; set; }

        public Func<TModel, bool> EnableWhenFunc { get; set; }

        public RoboUIGridRowAction<TModel> HasValue(Func<TModel, object> valueSelector)
        {
            ValueSelector = valueSelector;
            return this;
        }

        public RoboUIGridRowAction<TModel> HasUrl(Func<TModel, string> urlBuilder)
        {
            IsSubmitButton = false;
            UrlBuilder = urlBuilder;
            return this;
        }

        public RoboUIGridRowAction<TModel> VisibleWhen(Func<TModel, bool> func)
        {
            VisibleWhenFunc = func;
            return this;
        }

        public RoboUIGridRowAction<TModel> EnableWhen(Func<TModel, bool> func)
        {
            EnableWhenFunc = func;
            return this;
        }

        public RoboUIGridRowAction<TModel> HasAttribute(string name, Func<TModel, object> func)
        {
            attributes.Add(name, func);
            return this;
        }

        public bool IsVisible(object item)
        {
            return VisibleWhenFunc == null || VisibleWhenFunc((TModel)item);
        }

        public bool IsEnabled(object item)
        {
            return EnableWhenFunc == null || EnableWhenFunc((TModel)item);
        }

        public object GetValue(object item)
        {
            return ValueSelector != null ? ValueSelector((TModel)item) : null;
        }

        public string GetUrl(object item)
        {
            return UrlBuilder != null ? UrlBuilder((TModel)item) : null;
        }

        public IDictionary<string, object> GetAttributes(object obj)
        {
            var model = (TModel)obj;
            var dic = attributes.ToDictionary(k => k.Key, v => v.Value(model));
            var keys = (from pair in dic where pair.Value == null select pair.Key).ToList();
            foreach (var key in keys)
            {
                dic.Remove(key);
            }
            return dic;
        }
    }
}
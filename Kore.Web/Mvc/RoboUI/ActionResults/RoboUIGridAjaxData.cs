using System.Collections.Generic;
using System.Linq;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUIGridAjaxData<TModel> : List<TModel>
    {
        public RoboUIGridAjaxData()
        {
            UserData = new Dictionary<string, object>();
            Callbacks = new List<string>();
        }

        public RoboUIGridAjaxData(IEnumerable<TModel> items)
            : this()
        {
            AddRange(items);
        }

        public RoboUIGridAjaxData(IEnumerable<TModel> items, int totalRecords)
            : this()
        {
            AddRange(items);
            TotalRecords = totalRecords;
        }

        public int TotalRecords { get; private set; }

        public IDictionary<string, object> UserData { get; set; }

        public IList<string> Callbacks { get; private set; }

        #region Methods

        public void NotifyInfo(string message)
        {
            Callbacks.Add(string.Format("$('body').trigger({{ type: 'SystemMessageEvent', SystemMessage: 'NOTIFY_INFO', Data: {{ Message: {0} }} }});", message.JsEncode()));
        }

        public void NotifyWarning(string message)
        {
            Callbacks.Add(string.Format("$('body').trigger({{ type: 'SystemMessageEvent', SystemMessage: 'NOTIFY_WARNING', Data: {{ Message: {0} }} }});", message.JsEncode()));
        }

        public void NotifyError(string message)
        {
            Callbacks.Add(string.Format("$('body').trigger({{ type: 'SystemMessageEvent', SystemMessage: 'NOTIFY_ERROR', Data: {{ Message: {0} }} }});", message.JsEncode()));
        }

        #endregion Methods

        public static implicit operator RoboUIGridAjaxData<object>(RoboUIGridAjaxData<TModel> model)
        {
            var result = new RoboUIGridAjaxData<object>();
            result.AddRange(model.Cast<object>());
            result.TotalRecords = model.TotalRecords;
            result.UserData = model.UserData;
            return result;
        }
    }
}
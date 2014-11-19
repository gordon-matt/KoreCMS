using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Mvc.RoboUI
{
    public abstract class RoboUISubGrid
    {
        private readonly ICollection<RoboUIGridColumn> columns;

        protected RoboUISubGrid()
        {
            columns = new List<RoboUIGridColumn>();
        }

        public ICollection<RoboUIGridColumn> Columns
        {
            get { return columns; }
        }

        public abstract RoboUIGridAjaxData<object> GetSubGridData(string parentId);

        public abstract object GetModelId(object item);

        public abstract IEnumerable<IRoboUIGridRowAction> GetRowActions();

        public string ActionsColumnText { get; set; }

        public int ActionsColumnWidth { get; set; }

        public int? Width { get; set; }

        public JObject AjaxOptions { get; set; }
    }

    public class RoboUISubGrid<TModel, TModelKey> : RoboUISubGrid
    {
        private Func<TModelKey, RoboUIGridAjaxData<TModel>> fetchDataSource;
        private Func<TModel, object> getModelId;
        private readonly IList<RoboUIGridRowAction<TModel>> rowActions;

        public RoboUISubGrid()
        {
            getModelId = model => model.GetHashCode();
            rowActions = new List<RoboUIGridRowAction<TModel>>();
            ActionsColumnWidth = 120;
        }

        public RoboUIGridColumn<TModel> AddColumn<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            var name = Linq.Utils.GetFullPropertyName(expression);
            var column = new RoboUIGridColumn<TModel> { PropertyName = name, HeaderText = name };
            column.SetValueGetter(expression.Compile());
            Columns.Add(column);
            return column;
        }

        public RoboUISubGrid<TModel, TModelKey> HasFetchDataSource(Func<TModelKey, RoboUIGridAjaxData<TModel>> func)
        {
            fetchDataSource = func;
            return this;
        }

        public RoboUISubGrid<TModel, TModelKey> HasGetModelId(Func<TModel, object> func)
        {
            getModelId = func;
            return this;
        }

        public RoboUISubGrid<TModel, TModelKey> HasWidth(int width)
        {
            Width = width;
            return this;
        }

        public RoboUIGridRowAction<TModel> AddRowAction(bool isSubmitButton = false, bool isAjaxSupport = true)
        {
            var action = new RoboUIGridRowAction<TModel>(isSubmitButton, isAjaxSupport);
            rowActions.Add(action);
            return action;
        }

        public override RoboUIGridAjaxData<object> GetSubGridData(string parentId)
        {
            TModelKey key;
            var typeOfKey = typeof(TModelKey);
            if (typeOfKey == typeof(Guid))
            {
                key = (TModelKey)(dynamic)(new Guid(parentId));
            }
            else
            {
                key = parentId.ConvertTo<TModelKey>();
            }

            return fetchDataSource(key);
        }

        public override object GetModelId(object item)
        {
            return getModelId((TModel)item);
        }

        public override IEnumerable<IRoboUIGridRowAction> GetRowActions()
        {
            return rowActions.Select(x => (IRoboUIGridRowAction)x).ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Kore.Data;
using Kore.Infrastructure;
using Kore.Web.Mvc.Resources;
using Kore.Web.Mvc.RoboUI.Providers;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUIGridResult<TModel> : RoboUIResult, IHtmlString where TModel : class
    {
        #region Private Members

        private readonly ICollection<RoboUIFormAction> actions;
        private readonly ICollection<RoboUIGridColumn<TModel>> columns;
        private readonly ControllerContext controllerContext;
        private readonly IDictionary<string, string> customVariables;
        private readonly ICollection<string> reloadEvents;
        private readonly ICollection<RoboUIGridRowAction<TModel>> rowActions;
        private Action<RoboUIGridResult<TModel>> setupAction;
        private string clientId;

        #endregion Private Members

        #region Constructors

        public RoboUIGridResult(ControllerContext controllerContext)
        {
            this.controllerContext = controllerContext;
            columns = new List<RoboUIGridColumn<TModel>>();
            actions = new List<RoboUIFormAction>();
            rowActions = new List<RoboUIGridRowAction<TModel>>();
            reloadEvents = new List<string>();
            customVariables = new Dictionary<string, string>();
            IsAjaxSupported = true;
            DefaultPageSize = 10;
            EnableSortable = true;
            EnableFilterable = true;
            ActionsHeaderText = "Actions";
            RecordsInfoPosition = "right";

            if (typeof(IEntity).IsAssignableFrom(typeof(TModel)))
            {
                GetModelId = model => (model as IEntity).KeyValues;
            }
            else
            {
                GetModelId = model => model.GetHashCode();
            }
        }

        #endregion Constructors

        #region Properties

        public ICollection<RoboUIFormAction> Actions
        {
            get { return actions; }
        }

        public int? ActionsColumnWidth { get; set; }

        public string ActionsHeaderText { get; set; }

        public string ClientId
        {
            get
            {
                if (string.IsNullOrEmpty(clientId))
                {
                    clientId = "robo-grid-" + Guid.NewGuid().ToString("N").ToLowerInvariant();
                }
                return clientId;
            }
            set { clientId = value; }
        }

        public ICollection<RoboUIGridColumn<TModel>> Columns
        {
            get { return columns; }
        }

        public ControllerContext ControllerContext
        {
            get { return controllerContext; }
        }

        public string CssClass { get; set; }

        public IDictionary<string, string> CustomVariables
        {
            get { return customVariables; }
        }

        public int DefaultPageSize { get; set; }

        public bool EnableCheckboxes { get; set; }

        public bool EnablePageSizeChange { get; set; }

        public bool EnablePaginate { get; set; }

        public bool EnableSearch { get; set; }

        public bool EnableShowHideGrid { get; set; }

        public bool EnableSortable { get; set; }

        public bool EnableFilterable { get; set; }

        public Func<RoboUIGridRequest, RoboUIGridAjaxData<TModel>> FetchAjaxSource { get; set; }

        public Func<TModel, object> GetModelId { get; set; }

        public string GetRecordsUrl { get; set; }

        public string GridWrapperEndHtml { get; set; }

        public string GridWrapperStartHtml { get; set; }

        public bool HideActionsColumn { get; set; }

        public bool HidePagerWhenEmpty { get; set; }

        public bool IsAjaxSupported { get; set; }

        public string RecordsInfoPosition { get; set; }

        public ICollection<string> ReloadEvents
        {
            get { return reloadEvents; }
        }

        public ICollection<RoboUIGridRowAction<TModel>> RowActions
        {
            get { return rowActions; }
        }

        public JArray RowsList { get; set; }

        public bool ShowFooterRow { get; set; }

        public RoboUISubGrid SubGrid { get; private set; }

        public bool TreeGridEnabled { get; private set; }

        public string FormActionUrl { get; set; }

        public RoboUIResult FilterForm { get; set; }

        public bool DisableBlockUI { get; set; }

        public IRoboUIGridProvider RoboUIGridProvider { get; set; }

        public Func<TModel, bool> TreeGridHasChildren { get; private set; }

        public Func<TModel, dynamic> TreeGridParentId { get; private set; }

        #endregion Properties

        #region Public Methods

        #region Sub Grid

        public RoboUISubGrid<TSubModel, TModelKey> EnableSubGrid<TSubModel, TModelKey>()
        {
            if (TreeGridEnabled)
            {
                throw new ArgumentException("Cannot enable both tree grid and sub grid.");
            }

            if (SubGrid != null)
            {
                throw new ArgumentException("Sub Grid already enabled.");
            }

            var roboSubGridForm = new RoboUISubGrid<TSubModel, TModelKey>();
            SubGrid = roboSubGridForm;
            return roboSubGridForm;
        }

        #endregion Sub Grid

        #region Tree Grid

        public void EnableTreeGrid<TValue>(Expression<Func<TModel, TValue>> expression, Func<TModel, bool> hasChildren = null)
        {
            if (SubGrid != null)
            {
                throw new ArgumentException("Cannot enable both tree grid and sub grid.");
            }

            TreeGridEnabled = true;
            var func = expression.Compile();
            TreeGridParentId = x => func(x);

            if (hasChildren == null)
            {
                TreeGridHasChildren = model => true;
            }
            else
            {
                TreeGridHasChildren = hasChildren;
            }
        }

        #endregion Tree Grid

        public RoboUIFormAction AddAction(bool isSubmitButton = false, bool isAjaxSupport = true)
        {
            var action = new RoboUIFormAction(isSubmitButton, isAjaxSupport);
            actions.Add(action);
            return action;
        }

        public RoboUIFormAction AddAction(RoboUIFormAction action)
        {
            actions.Add(action);
            return action;
        }

        public RoboUIGridColumn<TModel> AddColumn<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            return AddColumn(expression, null);
        }

        public RoboUIGridColumn<TModel> AddColumn<TValue>(Expression<Func<TModel, TValue>> expression, string headerText)
        {
            var column = new RoboUIGridColumn<TModel>();
            column.SetValueGetter(expression.Compile());

            if (!string.IsNullOrEmpty(headerText))
            {
                column.HeaderText = headerText;
            }
            else
            {
                var modelMetadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>());
                column.HeaderText = modelMetadata.DisplayName ?? modelMetadata.PropertyName;
            }
            column.PropertyName = Linq.Utils.GetFullPropertyName(expression);
            column.PropertyType = typeof(TValue);

            columns.Add(column);
            return column;
        }

        public RoboUIGridColumn<TModel> AddColumn(string columnName)
        {
            var column = new RoboUIGridColumn<TModel> { PropertyName = columnName };
            columns.Add(column);
            return column;
        }

        public void AddCustomVariable(string name, object value, bool isFunction = false)
        {
            if (value == null)
            {
                throw new NullReferenceException();
            }

            if (isFunction || !(value is string))
            {
                if (isFunction)
                {
                    customVariables.Add(name, "function(){ return " + value + " }");
                }
                else
                {
                    customVariables.Add(name, value.ToString());
                }
            }
            else
            {
                customVariables.Add(name, value.ToString().JsEncode());
            }
        }

        public void AddReloadEvent(string eventName)
        {
            reloadEvents.Add(eventName);
        }

        public RoboUIGridRowAction<TModel> AddRowAction(bool isSubmitButton = false, bool isAjaxSupport = true)
        {
            var action = new RoboUIGridRowAction<TModel>(isSubmitButton, isAjaxSupport);
            rowActions.Add(action);
            return action;
        }

        public void AddRowAction(RoboUIGridRowAction<TModel> action)
        {
            rowActions.Add(action);
        }

        public override string GenerateView()
        {
            var viewData = controllerContext.Controller.ViewData;
            viewData.Model = null;

            var gridProvider = RoboUIGridProvider ?? RoboSettings.DefaultGridProvider;

            var viewContext = new ViewContext
            {
                HttpContext = controllerContext.HttpContext,
                Controller = controllerContext.Controller,
                RequestContext = controllerContext.RequestContext,
                ClientValidationEnabled = false,
                Writer = new StringWriter(),
                ViewData = viewData
            };

            var htmlHelper = new HtmlHelper<TModel>(viewContext, new ViewDataContainer(viewData));

            try
            {
                return gridProvider.Render(this, htmlHelper);
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
        }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            var gridProvider = RoboUIGridProvider ?? RoboSettings.DefaultGridProvider;
            gridProvider.GetAdditionalResources(scriptRegister, styleRegister);
        }

        public override bool OverrideExecuteResult()
        {
            if (controllerContext.HttpContext.Request.IsAjaxRequest())
            {
                try
                {
                    var gridProvider = RoboUIGridProvider ?? RoboSettings.DefaultGridProvider;

                    // Return data only
                    var request = gridProvider.CreateGridRequest(controllerContext);

                    if (request.PageIndex <= 0)
                    {
                        request.PageIndex = 1;
                    }

                    if (request.PageSize <= 0)
                    {
                        request.PageSize = DefaultPageSize;
                    }

                    gridProvider.ExecuteGridRequest(this, request, controllerContext);
                }
                catch (Exception ex)
                {
                    controllerContext.HttpContext.Response.Clear();
                    controllerContext.HttpContext.Response.Write(ex.GetBaseException().Message);
                }
                return true;
            }
            return base.OverrideExecuteResult();
        }

        #endregion Public Methods

        public RoboUIGridResult<TModel> Setup(Action<RoboUIGridResult<TModel>> action)
        {
            setupAction = action;
            return this;
        }

        public string ToHtmlString()
        {
            if (setupAction != null)
            {
                setupAction(this);
            }
            var workContext = EngineContext.Current.Resolve<IWebWorkContext>();
            var scriptRegister = new ScriptRegister(workContext);
            var styleRegister = new StyleRegister(workContext);
            GetAdditionalResources(scriptRegister, styleRegister);
            return GenerateView();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Kore.Caching;
using Kore.Collections;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public enum RoboUIFormLayout : byte
    {
        Flat,
        Grid,
        Grouped,
        Tab,
        Wizard,
        Table
    }

    public class RoboUIFormResult : RoboUIFormResult<dynamic>
    {
        public RoboUIFormResult(ControllerContext controllerContext)

            : base(new object(), controllerContext)
        {
        }

        public override IEnumerable<RoboControlAttribute> GetProperties()
        {
            return AdditionalFields.Values;
        }
    }

    public class RoboUIFormResult<TModel> : RoboUIResult, IHtmlString where TModel : class
    {
        #region Private Members

        private readonly IDictionary<string, RoboAutoCompleteOptions<TModel>> autoCompleteDataSources;
        private readonly IDictionary<string, string> cascadingCheckboxDataSource;
        private readonly IDictionary<string, RoboCascadingDropDownOptions> cascadingDropDownDataSource;
        private readonly ControllerContext controllerContext;
        private readonly ICollection<string> excludedProperties;
        private readonly IDictionary<string, Func<TModel, IEnumerable<SelectListItem>>> externalDataSources;
        private readonly IDictionary<string, RoboFileUploadOptions> fileUploadOptions;
        private readonly IDictionary<string, GridLayout> gridLayouts;
        private readonly ICollection<RoboUIGroupedLayout<TModel>> groupedLayouts;
        private readonly TModel model;
        private readonly Type modelType;
        private readonly ICollection<RoboUITabbedLayout<TModel>> tabbedLayouts;
        private ICollection<RoboUIFormAction> actions;
        private IDictionary<string, RoboControlAttribute> additionalFields;
        private ICollection<string> readOnlyProperties;
        private Action<RoboUIFormResult<TModel>> setupAction;

        #endregion Private Members

        #region Constructor

        public RoboUIFormResult(TModel model, ControllerContext controllerContext, Type modelType = null)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            if (Localizer == null)
            {
                Localizer = LocalizationUtilities.Resolve(controllerContext.Controller.GetType().FullName);
            }

            this.model = model;
            this.modelType = modelType ?? model.GetType();
            this.controllerContext = controllerContext;
            RoboUIFormProvider = RoboSettings.DefaultFormProvider;

            gridLayouts = new Dictionary<string, GridLayout>();
            groupedLayouts = new List<RoboUIGroupedLayout<TModel>>();
            tabbedLayouts = new List<RoboUITabbedLayout<TModel>>();
            excludedProperties = new List<string>();

            autoCompleteDataSources = new Dictionary<string, RoboAutoCompleteOptions<TModel>>();
            cascadingCheckboxDataSource = new Dictionary<string, string>();
            cascadingDropDownDataSource = new Dictionary<string, RoboCascadingDropDownOptions>();
            externalDataSources = new Dictionary<string, Func<TModel, IEnumerable<SelectListItem>>>();
            fileUploadOptions = new Dictionary<string, RoboFileUploadOptions>();

            AjaxEnabled = true;
            CancelButtonText = Localizer("Cancel");
            Layout = RoboUIFormLayout.Flat;
            ShowCancelButton = true;
            CancelButtonHtmlAttributes = new Dictionary<string, object>();
            ShowSubmitButton = true;
            SubmitButtonHtmlAttributes = new Dictionary<string, object>();
            SubmitButtonText = Localizer("Save");
            ShowAsModalDialog = false;
            FormMethod = FormMethod.Post;
            ShowValidationSummary = true;
            ShowBoxHeader = true;
        }

        #endregion Constructor

        #region Public Properties

        public Localizer Localizer { get; set; }

        public ICollection<RoboUIFormAction> Actions
        {
            get { return actions ?? (actions = new List<RoboUIFormAction>()); }
            set { actions = value; }
        }

        public IDictionary<string, RoboControlAttribute> AdditionalFields
        {
            get { return additionalFields ?? (additionalFields = new Dictionary<string, RoboControlAttribute>()); }
            set { additionalFields = value; }
        }

        public bool AjaxEnabled { get; set; }

        public IDictionary<string, object> CancelButtonHtmlAttributes { get; private set; }

        public string CancelButtonText { get; set; }

        public string CancelButtonUrl { get; set; }

        public ControllerContext ControllerContext
        {
            get { return controllerContext; }
        }

        public string Description { get; set; }

        public bool DisableBlockUI { get; set; }

        public bool DisableGenerateForm { get; set; }

        public bool EnableKnockoutJs { get; set; }

        public ICollection<string> ExcludedProperties
        {
            get { return excludedProperties; }
        }

        public string CssClass { get; set; }

        public string FormActionsContainerCssClass { get; set; }

        public string FormActionsCssClass { get; set; }

        public string FormActionUrl { get; set; }

        public string FormId { get; set; }

        public FormMethod FormMethod { get; set; }

        public TModel FormModel
        {
            get { return model; }
        }

        public string FormWrapperEndHtml { get; set; }

        public string FormWrapperStartHtml { get; set; }

        public IDictionary<string, GridLayout> GridLayouts
        {
            get { return gridLayouts; }
        }

        public ICollection<RoboUIGroupedLayout<TModel>> GroupedLayouts
        {
            get { return groupedLayouts; }
        }

        public object HtmlAttributes { get; set; }

        public RoboUIFormLayout Layout { get; set; }

        public bool ReadOnly { get; set; }

        public ICollection<string> ReadOnlyProperties
        {
            get { return readOnlyProperties ?? (readOnlyProperties = new List<string>()); }
            set { readOnlyProperties = value; }
        }

        public bool ShowAsModalDialog { get; set; }

        public bool ShowBoxHeader { get; set; }

        public bool ShowCancelButton { get; set; }

        public bool ShowCloseButton { get; set; }

        public bool ShowSubmitButton { get; set; }

        public bool ShowValidationSummary { get; set; }

        public IDictionary<string, object> SubmitButtonHtmlAttributes { get; private set; }

        public string SubmitButtonText { get; set; }

        public ICollection<RoboUITabbedLayout<TModel>> TabbedLayouts
        {
            get { return tabbedLayouts; }
        }

        public Func<RoboUIFormResult<TModel>, TModel, int, bool> ValidateWizardStep { get; set; }

        public string ValidationSummary { get; set; }

        public string WizardUpdateActionName { get; set; }

        #endregion Public Properties

        #region Public Methods

        public RoboUIFormAction AddAction(bool isSubmitButton = false, bool ajaxEnabled = true)
        {
            var action = new RoboUIFormAction(isSubmitButton, ajaxEnabled);
            Actions.Add(action);
            return action;
        }

        public RoboUIGroupedLayout<TModel> AddGroupedLayout(string title, bool getExisting = false)
        {
            if (getExisting)
            {
                var existing = groupedLayouts.FirstOrDefault(x => x.Title == title);
                if (existing != null)
                {
                    return existing;
                }
            }

            var layout = new RoboUIGroupedLayout<TModel>(title);
            GroupedLayouts.Add(layout);
            return layout;
        }

        public virtual void AddProperty(string name, RoboControlAttribute attribute, object value = null)
        {
            attribute.Name = name;
            attribute.Value = value;
            AdditionalFields[name] = attribute;
        }

        public RoboUITabbedLayout<TModel> AddTabbedLayout(string title)
        {
            var layout = new RoboUITabbedLayout<TModel>(title);
            TabbedLayouts.Add(layout);
            return layout;
        }

        public void AssignGridLayout<TValue>(Expression<Func<TModel, TValue>> expression, int col, int row, int colSpan = 1, int rowSpan = 1)
        {
            AssignGridLayout(ExpressionHelper.GetExpressionText(expression), col, row, colSpan, rowSpan);
        }

        public void AssignGridLayout(string property, int col, int row, int colSpan = 1, int rowSpan = 1)
        {
            if (colSpan < 1)
            {
                throw new ArgumentOutOfRangeException("colSpan");
            }

            GridLayouts.Add(property, new GridLayout(col, row, colSpan, rowSpan));
        }

        public void ExcludeProperty(string name)
        {
            excludedProperties.Add(name);
        }

        public void ExcludeProperty<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            excludedProperties.Add(ExpressionHelper.GetExpressionText(expression));
        }

        public override string GenerateView()
        {
            var viewContext = new ViewContext
            {
                HttpContext = controllerContext.HttpContext,
                Controller = controllerContext.Controller,
                RequestContext = controllerContext.RequestContext,
                ClientValidationEnabled = false,
                Writer = new StringWriter(),
                ViewData = ViewData
            };

            var htmlHelper = new HtmlHelper(viewContext, new ViewDataContainer(ViewData));

            return RoboUIFormProvider.RenderForm(htmlHelper, this);
        }

        public int GetCurrentWizardStep()
        {
            var httpContext = controllerContext.HttpContext;

            if (httpContext.Items["__RoboWizard_CurrentStep"] != null)
            {
                return (int)httpContext.Items["__RoboWizard_CurrentStep"];
            }

            var currentStep = Convert.ToInt32(httpContext.Request.Form["__CurrentStep"]);

            // Bind data from form to model
            if (httpContext.Request.HttpMethod.ToLowerInvariant() == "post")
            {
                foreach (var key in httpContext.Request.Form.AllKeys.Where(x => x.StartsWith("RoboWizard_")))
                {
                    var value = httpContext.Request.Form[key];
                    value = value.HtmlEncode();
                    value = value.HtmlDecode();
                    SetPropertyValue(key.Replace("RoboWizard_", string.Empty), value.SharpDeserialize<object>());
                }

                TryUpdateModel(model);
            }

            if (currentStep > 0 && ValidateWizardStep != null)
            {
                var isValid = ValidateWizardStep(this, model, currentStep);
                if (!isValid)
                {
                    currentStep--;
                    while (currentStep > 0 && !ValidateWizardStep(this, model, currentStep))
                    {
                        currentStep--;
                    }
                }
            }

            httpContext.Items["__RoboWizard_CurrentStep"] = currentStep;

            return currentStep;
        }

        public virtual IEnumerable<RoboControlAttribute> GetProperties()
        {
            var cacheManager = EngineContext.Current.Resolve<ICacheManager>();

            var attributes = cacheManager.Get<List<RoboControlAttribute>>("RoboForms_Properties_" + modelType.FullName, () =>
            {
                var propertyInfos = modelType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                var result = new List<RoboControlAttribute>();

                foreach (var propertyInfo in propertyInfos)
                {
                    var controlAttribute = propertyInfo.GetCustomAttribute<RoboControlAttribute>(false);

                    if (controlAttribute != null)
                    {
                        controlAttribute.Name = propertyInfo.Name;
                        controlAttribute.PropertyName = propertyInfo.Name;
                        controlAttribute.PropertyType = propertyInfo.PropertyType;
                        controlAttribute.PropertyInfo = propertyInfo;

                        if (controlAttribute.LabelText == null)
                        {
                            controlAttribute.LabelText = propertyInfo.Name;
                        }

                        var htmlAttributes = propertyInfo.GetCustomAttributes<RoboHtmlAttributeAttribute>().ToList();
                        if (htmlAttributes.Any())
                        {
                            foreach (var htmlAttribute in htmlAttributes)
                            {
                                controlAttribute.HtmlAttributes.Add(htmlAttribute.Name, htmlAttribute.Value);
                            }
                        }

                        result.Add(controlAttribute);
                    }
                }
                return result.OrderBy(x => x.Order).ToList();
            });

            var properties = new List<RoboControlAttribute>();

            foreach (var attribute in attributes)
            {
                var property = attribute.ShallowCopy();
                property.Value = GetPropertyValue(model, property.Name);
                properties.Add(property);
            }

            return properties.Concat(AdditionalFields.Values).OrderBy(x => x.Order);
        }

        public virtual object GetPropertyValue(object modelObject, string property)
        {
            if (modelObject == null)
            {
                return null;
            }

            var type = modelObject.GetType();
            var propertyInfo = type.GetProperty(property);
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(modelObject);
            }

            var provider = modelObject as IDynamicMetaObjectProvider;
            if (provider != null)
            {
                dynamic dynamicObject = provider;
                return dynamicObject[property];
            }

            return null;
        }

        public void MakePropertyReadOnly<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            ReadOnlyProperties.Add(ExpressionHelper.GetExpressionText(expression));
        }

        #endregion Public Methods

        #region Protected Methods

        protected virtual void SetPropertyValue(string property, object value)
        {
            var attributes = GetProperties();
            var propertyInfo = attributes.FirstOrDefault(x => x.Name == property);
            if (propertyInfo != null)
            {
                propertyInfo.PropertyInfo.SetValue(model, value);
            }
        }

        protected virtual void TryUpdateModel(TModel modelObject)
        {
            var binder = new DefaultModelBinder();
            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => modelObject, typeof(TModel)),
                ModelState = controllerContext.Controller.ViewData.ModelState,
                ValueProvider = controllerContext.Controller.ValueProvider
            };

            binder.BindModel(controllerContext, bindingContext);
        }

        #endregion Protected Methods

        public override bool OverrideExecuteResult()
        {
            if (Layout == RoboUIFormLayout.Wizard)
            {
                GetProperties();
                var currentStep = GetCurrentWizardStep();
                if (currentStep >= groupedLayouts.Count)
                {
                    controllerContext.Controller.ValueProvider = new RoboWizardValueProvider(controllerContext.Controller.ValueProvider, model);
                    var invoker = new ControllerActionInvoker();
                    invoker.InvokeAction(controllerContext, WizardUpdateActionName);
                    return true;
                }
            }
            return base.OverrideExecuteResult();
        }

        #region DataSources

        public virtual RoboAutoCompleteOptions GetAutoCompleteDataSource(string property)
        {
            if (!autoCompleteDataSources.ContainsKey(property))
            {
                throw new ArgumentOutOfRangeException("property", "You must register a auto complete options for: " + property);
            }
            var dataSource = autoCompleteDataSources[property];
            return dataSource;
        }

        public virtual string GetCascadingCheckBoxDataSource(string property)
        {
            if (cascadingCheckboxDataSource.ContainsKey(property))
            {
                return cascadingCheckboxDataSource[property];
            }

            throw new NotSupportedException(string.Format("You must register a cascading dropdown data source for '{0}'.", property));
        }

        public virtual RoboCascadingDropDownOptions GetCascadingDropDownDataSource(string property)
        {
            if (cascadingDropDownDataSource.ContainsKey(property))
            {
                return cascadingDropDownDataSource[property];
            }

            throw new NotSupportedException(string.Format("You must register a cascading dropdown data source for '{0}'.", property));
        }

        public virtual IList<SelectListItem> GetExternalDataSource(string property)
        {
            if (!externalDataSources.ContainsKey(property))
            {
                return null;
            }

            var dataSource = externalDataSources[property];
            return dataSource.Invoke(model).ToList();
        }

        public virtual RoboFileUploadOptions GetFileUploadOptions(string property)
        {
            if (fileUploadOptions.ContainsKey(property))
            {
                return fileUploadOptions[property];
            }

            return new RoboFileUploadOptions();
        }

        public void RegisterAutoCompleteDataSource<TValue>(Expression<Func<TModel, TValue>> expression, RoboAutoCompleteOptions<TModel> options)
        {
            autoCompleteDataSources.Add(ExpressionHelper.GetExpressionText(expression), options);
        }

        public void RegisterAutoCompleteDataSource<TValue>(Expression<Func<TModel, TValue>> expression, string sourceUrl)
        {
            autoCompleteDataSources.Add(ExpressionHelper.GetExpressionText(expression), new RoboAutoCompleteOptions<TModel> { SourceUrl = sourceUrl });
        }

        public void RegisterAutoCompleteDataSource<TValue>(Expression<Func<TModel, TValue>> expression, string sourceUrl, Func<TModel, string> textSelector)
        {
            autoCompleteDataSources.Add(ExpressionHelper.GetExpressionText(expression), new RoboAutoCompleteOptions<TModel> { SourceUrl = sourceUrl, TextSelector = textSelector });
        }

        public void RegisterCascadingCheckboxDataSource(string property, string sourceUrl)
        {
            cascadingCheckboxDataSource.Add(property, sourceUrl);
        }

        public void RegisterCascadingDropDownDataSource<TValue>(Expression<Func<TModel, TValue>> expression, string sourceUrl)
        {
            cascadingDropDownDataSource.Add(ExpressionHelper.GetExpressionText(expression), new RoboCascadingDropDownOptions { SourceUrl = sourceUrl });
        }

        public void RegisterCascadingDropDownDataSource<TValue>(Expression<Func<TModel, TValue>> expression, RoboCascadingDropDownOptions options)
        {
            cascadingDropDownDataSource.Add(ExpressionHelper.GetExpressionText(expression), options);
        }

        public void RegisterCascadingDropDownDataSource(string property, string sourceUrl)
        {
            cascadingDropDownDataSource.Add(property, new RoboCascadingDropDownOptions { SourceUrl = sourceUrl });
        }

        public void RegisterCascadingDropDownDataSource(string property, RoboCascadingDropDownOptions options)
        {
            cascadingDropDownDataSource.Add(property, options);
        }

        public void RegisterExternalDataSource<TProperty>(Expression<Func<TModel, TProperty>> expression, Func<TModel, IEnumerable<SelectListItem>> items)
        {
            var str = ExpressionHelper.GetExpressionText(expression);
            externalDataSources.Add(str, items);
        }

        public void RegisterExternalDataSource<TProperty>(Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> items)
        {
            var str = ExpressionHelper.GetExpressionText(expression);
            externalDataSources.Add(str, x => items);
        }

        public void RegisterExternalDataSource<TProperty>(Expression<Func<TModel, TProperty>> expression, params string[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            var func = new Func<TModel, List<SelectListItem>>(item => values.Select(value => new SelectListItem
            {
                Text = value,
                Value = value
            }).ToList());
            externalDataSources.Add(ExpressionHelper.GetExpressionText(expression), func);
        }

        public void RegisterExternalDataSource<TProperty, TKey>(Expression<Func<TModel, TProperty>> expression, IDictionary<TKey, string> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            var func = new Func<TModel, List<SelectListItem>>(item => values.Select(value => new SelectListItem
            {
                Text = value.Value,
                Value = Convert.ToString(value.Key)
            }).ToList());
            externalDataSources.Add(ExpressionHelper.GetExpressionText(expression), func);
        }

        public void RegisterExternalDataSource(string property, Func<TModel, IEnumerable<SelectListItem>> items)
        {
            externalDataSources.Add(property, items);
        }

        public void RegisterExternalDataSource(string property, params string[] values)
        {
            RegisterExternalDataSource(property, values.ToDictionary(k => k, v => v));
        }

        public void RegisterExternalDataSource(string property, IEnumerable<string> values)
        {
            RegisterExternalDataSource(property, values.ToDictionary(k => k, v => v));
        }

        public void RegisterExternalDataSource<TKey>(string property, IDictionary<TKey, string> values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            var func = new Func<TModel, List<SelectListItem>>(item => values.Select(value => new SelectListItem
            {
                Text = value.Value,
                Value = Convert.ToString(value.Key)
            }).ToList());
            externalDataSources.Add(property, func);
        }

        public void RegisterExternalDataSource<TKey>(string property, Dictionary<TKey, string> dataSourceValues)
        {
            externalDataSources[property] = m => new List<SelectListItem>(dataSourceValues.Select(x => new SelectListItem { Text = x.Value, Value = Convert.ToString(x.Key) }));
        }

        public void RegisterExternalDataSource(string property, IEnumerable<SelectListItem> items)
        {
            externalDataSources[property] = m => items;
        }

        public void RegisterFileUploadOptions<TValue>(Expression<Func<TModel, TValue>> expression, RoboFileUploadOptions options)
        {
            fileUploadOptions.Add(ExpressionHelper.GetExpressionText(expression), options);
        }

        public void RegisterFileUploadOptions(string property, string uploadUrl)
        {
            fileUploadOptions.Add(property, new RoboFileUploadOptions { UploadUrl = uploadUrl });
        }

        public void RegisterFileUploadOptions(string property, RoboFileUploadOptions options)
        {
            fileUploadOptions.Add(property, options);
        }

        #endregion DataSources

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            if (ShowValidationSummary)
            {
                scriptRegister.IncludeBundle("jquery-validate");
            }

            GetProperties().ForEach(property => property.GetAdditionalResources(scriptRegister, styleRegister));

            if (EnableKnockoutJs)
            {
                scriptRegister.IncludeBundle("knockoutjs");
            }
        }

        private class RoboWizardValueProvider : IValueProvider
        {
            private readonly object model;
            private readonly IValueProvider valueProvider;

            public RoboWizardValueProvider(IValueProvider valueProvider, object model)
            {
                this.valueProvider = valueProvider;
                this.model = model;
            }

            public bool ContainsPrefix(string prefix)
            {
                return prefix == "model" || valueProvider.ContainsPrefix(prefix);
            }

            public ValueProviderResult GetValue(string key)
            {
                return key == "model"
                    ? new ValueProviderResult(model, null, CultureInfo.InvariantCulture)
                    : valueProvider.GetValue(key);
            }
        }

        public RoboUIFormResult<TModel> Setup(Action<RoboUIFormResult<TModel>> action)
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

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var scriptRegister = new ScriptRegister(workContext);
            var styleRegister = new StyleRegister(workContext);
            GetAdditionalResources(scriptRegister, styleRegister);
            return GenerateView();
        }
    }
}
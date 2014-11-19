using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Mvc;
using Kore.Data;
using Kore.Serialization;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets
{
    public abstract class WidgetBase : IEntity, IWidget
    {
        #region IWidget Members

        [RoboHidden, ExcludeFromSerialization]
        public Guid Id { get; set; }

        [ExcludeFromSerialization]
        public abstract string Name { get; }

        [RoboChoice(RoboChoiceType.CheckBox, Order = -9999, ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = -10, LabelText = "Localized for this language")]
        public bool Localized { get; set; }

        [ExcludeFromSerialization]
        [RoboText(IsRequired = true, LabelText = "Title", MaxLength = 255, Order = -9998, ContainerCssClass = "col-xs-6 col-sm-6", ContainerRowIndex = -9)]
        public string Title { get; set; }

        [Display(Name = "Show Title On Page")]
        [RoboChoice(RoboChoiceType.CheckBox, LabelText = "Show Title On Page", Order = -9997, ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = -9)]
        public bool ShowTitleOnPage { get; set; }

        public virtual bool HasTitle { get { return true; } }

        [ExcludeFromSerialization]
        [Display(Name = "Display Condition")]
        [RoboText(Order = -9994, LabelText = "Display Condition", ContainerCssClass = "col-xs-6 col-sm-6", ContainerRowIndex = -8)]
        public string DisplayCondition { get; set; }

        [ExcludeFromSerialization]
        [RoboChoice(RoboChoiceType.CheckBox, Order = -9993, ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = -8, LabelText = "Enabled")]
        public bool Enabled { get; set; }

        [Display(Name = "Zone")]
        [ExcludeFromSerialization]
        [RoboChoice(RoboChoiceType.DropDownList, IsRequired = true, Order = -9996, LabelText = "Zone", ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = -7)]
        [RoboHtmlAttribute("class", "uniform")]
        public Guid ZoneId { get; set; }

        [ExcludeFromSerialization]
        public Guid? PageId { get; set; }

        [ExcludeFromSerialization]
        [RoboNumeric(IsRequired = true, LabelText = "Order", Order = -9995, ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = -7)]
        public int Order { get; set; }

        [RoboText(MaxLength = 255, LabelText = "Css Class", Order = -9992, ContainerCssClass = "col-xs-3 col-sm-3", ContainerRowIndex = -7)]
        public string CssClass { get; set; }

        [ExcludeFromSerialization]
        public bool IsMoveable { get; set; }

        [ExcludeFromSerialization]
        public string CultureCode { get; set; }

        [ExcludeFromSerialization]
        public Guid? RefId { get; set; }

        public abstract string DisplayTemplatePath { get; }

        public abstract string EditorTemplatePath { get; }

        #region IEntity Members

        public object[] KeyValues
        {
            get { return new object[] { Id }; }
        }

        #endregion IEntity Members

        //public virtual void RegisterResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        //{
        //}

        //public abstract string BuildDisplay(ViewContext viewContext);

        //public virtual ActionResult DisplayCallback(Controller controller)
        //{
        //    return null;
        //}

        //public string GetDisplayCallbackUrl(UrlHelper urlHelper)
        //{
        //    return urlHelper.Action("DisplayCallback", "Widget", new { area = Constants.Areas.Widgets, widgetId = Id });
        //}

        //public virtual ActionResult BuildEditor(Controller controller, RoboUIFormResult<IWidget> roboForm)
        //{
        //    return roboForm;
        //}

        //public virtual ActionResult EditorCallback(Controller controller)
        //{
        //    return null;
        //}

        //public virtual void OnSaving()
        //{
        //}

        //IWidget IWidget.ShallowCopy()
        //{
        //    return (IWidget)MemberwiseClone();
        //}

        #endregion IWidget Members

        #region Helpers

        protected string ViewContent(ViewContext viewContext, string viewName, object model = null)
        {
            var controllerContext = new ControllerContext
            {
                RouteData = viewContext.RouteData,
                HttpContext = viewContext.HttpContext,
            };

            viewContext.ViewData.Model = model;

            var result = ViewEngines.Engines.FindPartialView(controllerContext, viewName);

            if (result != null && result.View != null)
            {
                using (var writer = new StringWriter())
                {
                    result.View.Render(viewContext, writer);
                    return writer.ToString();
                }
            }

            return null;
        }

        #endregion Helpers
    }
}
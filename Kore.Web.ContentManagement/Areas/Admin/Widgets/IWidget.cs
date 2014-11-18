using System;

namespace Kore.Web.ContentManagement.Areas.Admin.Widgets
{
    public interface IWidget
    {
        Guid Id { get; set; }

        string Name { get; }

        string Title { get; set; }

        bool ShowTitleOnPage { get; set; }

        bool HasTitle { get; }

        Guid ZoneId { get; set; }

        Guid? PageId { get; set; }

        int Order { get; set; }

        bool Enabled { get; set; }

        bool Localized { get; set; }

        string DisplayCondition { get; set; }

        string CssClass { get; set; }

        bool IsMoveable { get; set; }

        string CultureCode { get; set; }

        Guid? RefId { get; set; }

        string DisplayTemplatePath { get; }

        string EditorTemplatePath { get; }

        //void RegisterResources(ScriptRegister scriptRegister, StyleRegister styleRegister);

        //string BuildDisplay(ViewContext viewContext);

        //ActionResult DisplayCallback(Controller controller);

        //string GetDisplayCallbackUrl(UrlHelper urlHelper);

        //ActionResult BuildEditor(Controller controller, RoboUIFormResult<IWidget> form);

        //ActionResult EditorCallback(Controller controller);

        //void OnSaving();

        //IWidget ShallowCopy();
    }
}
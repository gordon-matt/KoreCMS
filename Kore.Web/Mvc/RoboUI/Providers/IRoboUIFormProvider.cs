using System.Collections.Generic;
using System.Web.Mvc;

namespace Kore.Web.Mvc.RoboUI.Providers
{
    public interface IRoboUIFormProvider
    {
        string GetButtonSizeCssClass(ButtonSize buttonSize);

        string GetButtonStyleCssClass(ButtonStyle buttonStyle);

        string RenderActions(HtmlHelper htmlHelper, IEnumerable<RoboUIFormAction> actions);

        string RenderAction(HtmlHelper htmlHelper, RoboUIFormAction action);

        string RenderForm<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm) where TModel : class;

        string RenderControl<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboControlAttribute roboAttribute) where TModel : class;

        string RenderAutoCompleteAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboAutoCompleteAttribute roboAttribute) where TModel : class;

        string RenderButtonAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboButtonAttribute roboAttribute) where TModel : class;

        string RenderCaptchaAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCaptchaAttribute roboAttribute) where TModel : class;

        string RenderCascadingCheckBoxListAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCascadingCheckBoxListAttribute roboAttribute) where TModel : class;

        string RenderCascadingDropDownAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCascadingDropDownAttribute roboAttribute) where TModel : class;

        string RenderChoiceAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboChoiceAttribute roboAttribute) where TModel : class;

        string RenderColorPickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboColorPickerAttribute roboAttribute) where TModel : class;

        string RenderComplexAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboComplexAttribute roboAttribute) where TModel : class;

        string RenderDatePickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboDatePickerAttribute roboAttribute) where TModel : class;

        string RenderDivAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboDivAttribute roboAttribute) where TModel : class;

        string RenderFileUploadAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboFileUploadAttribute roboAttribute) where TModel : class;

        string RenderGridAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboGridAttribute roboAttribute) where TModel : class;

        string RenderHiddenFieldAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboHiddenAttribute roboAttribute) where TModel : class;

        string RenderHtmlViewAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboHtmlViewAttribute roboAttribute) where TModel : class;

        string RenderIconFontPickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboIconFontPickerAttribute roboAttribute) where TModel : class;

        string RenderImageAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboImageAttribute roboAttribute) where TModel : class;

        string RenderLabelAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboLabelAttribute roboAttribute) where TModel : class;

        string RenderNumericAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboNumericAttribute roboAttribute) where TModel : class;

        string RenderSlugAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboSlugAttribute roboAttribute) where TModel : class;

        string RenderTextAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboTextAttribute roboAttribute) where TModel : class;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Kore.Web.Mvc.RoboUI.Providers
{
    public abstract class BaseRoboUIFormProvider : IRoboUIFormProvider
    {
        #region IRoboUIFormProvider Members

        public virtual string GetButtonSizeCssClass(ButtonSize buttonSize)
        {
            throw new NotImplementedException();
        }

        public virtual string GetButtonStyleCssClass(ButtonStyle buttonStyle)
        {
            throw new NotImplementedException();
        }

        public virtual string RenderAction(HtmlHelper htmlHelper, RoboUIFormAction action)
        {
            throw new NotImplementedException();
        }

        public virtual string RenderActions(HtmlHelper htmlHelper, IEnumerable<RoboUIFormAction> actions)
        {
            throw new NotImplementedException();
        }

        public virtual string RenderAutoCompleteAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboAutoCompleteAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderButtonAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboButtonAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderCaptchaAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCaptchaAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderCascadingCheckBoxListAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCascadingCheckBoxListAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderCascadingDropDownAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboCascadingDropDownAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderChoiceAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboChoiceAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderColorPickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboColorPickerAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderComplexAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboComplexAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderControl<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboControlAttribute roboAttribute) where TModel : class
        {
            if (roboAttribute is RoboIconFontPickerAttribute)
            {
                return RenderIconFontPickerAttribute(htmlHelper, roboForm, roboAttribute as RoboIconFontPickerAttribute);
            }

            if (roboAttribute is RoboAutoCompleteAttribute)
            {
                return RenderAutoCompleteAttribute(htmlHelper, roboForm, roboAttribute as RoboAutoCompleteAttribute);
            }

            if (roboAttribute is RoboButtonAttribute)
            {
                return RenderButtonAttribute(htmlHelper, roboForm, roboAttribute as RoboButtonAttribute);
            }

            if (roboAttribute is RoboCaptchaAttribute)
            {
                return RenderCaptchaAttribute(htmlHelper, roboForm, roboAttribute as RoboCaptchaAttribute);
            }

            if (roboAttribute is RoboCascadingCheckBoxListAttribute)
            {
                return RenderCascadingCheckBoxListAttribute(htmlHelper, roboForm, roboAttribute as RoboCascadingCheckBoxListAttribute);
            }

            if (roboAttribute is RoboCascadingDropDownAttribute)
            {
                return RenderCascadingDropDownAttribute(htmlHelper, roboForm, roboAttribute as RoboCascadingDropDownAttribute);
            }

            if (roboAttribute is RoboColorPickerAttribute)
            {
                return RenderColorPickerAttribute(htmlHelper, roboForm, roboAttribute as RoboColorPickerAttribute);
            }

            if (roboAttribute is RoboChoiceAttribute)
            {
                return RenderChoiceAttribute(htmlHelper, roboForm, roboAttribute as RoboChoiceAttribute);
            }

            if (roboAttribute is RoboComplexAttribute)
            {
                return RenderComplexAttribute(htmlHelper, roboForm, roboAttribute as RoboComplexAttribute);
            }

            if (roboAttribute is RoboDatePickerAttribute)
            {
                return RenderDatePickerAttribute(htmlHelper, roboForm, roboAttribute as RoboDatePickerAttribute);
            }

            if (roboAttribute is RoboDivAttribute)
            {
                return RenderDivAttribute(htmlHelper, roboForm, roboAttribute as RoboDivAttribute);
            }

            if (roboAttribute is RoboFileUploadAttribute)
            {
                return RenderFileUploadAttribute(htmlHelper, roboForm, roboAttribute as RoboFileUploadAttribute);
            }

            if (roboAttribute is RoboGridAttribute)
            {
                return RenderGridAttribute(htmlHelper, roboForm, roboAttribute as RoboGridAttribute);
            }

            if (roboAttribute is RoboHiddenAttribute)
            {
                return RenderHiddenFieldAttribute(htmlHelper, roboForm, roboAttribute as RoboHiddenAttribute);
            }

            if (roboAttribute is RoboHtmlViewAttribute)
            {
                return RenderHtmlViewAttribute(htmlHelper, roboForm, roboAttribute as RoboHtmlViewAttribute);
            }

            if (roboAttribute is RoboImageAttribute)
            {
                return RenderImageAttribute(htmlHelper, roboForm, roboAttribute as RoboImageAttribute);
            }

            if (roboAttribute is RoboLabelAttribute)
            {
                return RenderLabelAttribute(htmlHelper, roboForm, roboAttribute as RoboLabelAttribute);
            }

            if (roboAttribute is RoboNumericAttribute)
            {
                return RenderNumericAttribute(htmlHelper, roboForm, roboAttribute as RoboNumericAttribute);
            }

            if (roboAttribute is RoboSlugAttribute)
            {
                return RenderSlugAttribute(htmlHelper, roboForm, roboAttribute as RoboSlugAttribute);
            }

            if (roboAttribute is RoboTextAttribute)
            {
                return RenderTextAttribute(htmlHelper, roboForm, roboAttribute as RoboTextAttribute);
            }

            throw new ArgumentOutOfRangeException();
        }

        public virtual string RenderDatePickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboDatePickerAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderDivAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboDivAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderFileUploadAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboFileUploadAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderForm<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderGridAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboGridAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderHiddenFieldAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboHiddenAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderHtmlViewAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboHtmlViewAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderIconFontPickerAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboIconFontPickerAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderImageAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboImageAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderLabelAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboLabelAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderNumericAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboNumericAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderSlugAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboSlugAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        public virtual string RenderTextAttribute<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, RoboTextAttribute roboAttribute) where TModel : class
        {
            throw new NotImplementedException();
        }

        #endregion IRoboUIFormProvider Members

        protected static string ConvertDateFormat(string format)
        {
            /*
             *  .NET    JQueryUI        Output      Comment
             *  --------------------------------------------------------------
             *  d       d               5           day of month(No leading zero)
             *  dd      dd              05          day of month(two digit)
             *  ddd     D               Thu         day short name
             *  dddd    DD              Thursday    day long name
             *  M       m               11          month of year(No leading zero)
             *  MM      mm              11          month of year(two digit)
             *  MMM     M               Nov         month name short
             *  MMMM    MM              November    month name long.
             *  yy      y               09          Year(two digit)
             *  yyyy    yy              2009        Year(four digit)
             */

            var currentFormat = format;

            // Convert the date
            currentFormat = currentFormat.Replace("dddd", "DD");
            currentFormat = currentFormat.Replace("ddd", "D");

            // Convert month
            if (currentFormat.Contains("MMMM"))
            {
                currentFormat = currentFormat.Replace("MMMM", "MM");
            }
            else if (currentFormat.Contains("MMM"))
            {
                currentFormat = currentFormat.Replace("MMM", "M");
            }
            else if (currentFormat.Contains("MM"))
            {
                currentFormat = currentFormat.Replace("MM", "mm");
            }
            else
            {
                currentFormat = currentFormat.Replace("M", "m");
            }

            // Convert year
            currentFormat = currentFormat.Contains("yyyy") ? currentFormat.Replace("yyyy", "yy") : currentFormat.Replace("yy", "y");

            return currentFormat;
        }

        protected static bool DetectSpanCells(GridLayout gridLayout, int row, int column)
        {
            if (gridLayout.Column == 0 && gridLayout.Row == 0)
            {
                return false;
            }

            if (gridLayout.ColumnSpan == 1 && gridLayout.RowSpan == 1)
            {
                return false;
            }

            if (gridLayout.Column == column && gridLayout.Row == row)
            {
                return false;
            }

            if (gridLayout.Row > row)
            {
                return false;
            }

            if (gridLayout.Column > column)
            {
                return false;
            }

            if (gridLayout.Row + gridLayout.RowSpan > row && gridLayout.Column + gridLayout.ColumnSpan > column)
            {
                return true;
            }

            return false;
        }

        protected static string GenerateStyleAttribute(string name, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                return string.Concat(name, ":", value, ";");
            }
            return null;
        }

        protected static string GetEnumValueDescription(Type type, object value)
        {
            var field = type.GetField(Convert.ToString(value));
            var attrs = field.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs.Length > 0)
            {
                return ((DisplayAttribute)attrs[0]).GetName();
            }

            return Convert.ToString(value);
        }

        protected static void MergeHtmlAttribute(IDictionary<string, object> htmlAttributes, string key, string value)
        {
            if (htmlAttributes.ContainsKey(key))
            {
                if (key == "class")
                {
                    if (!htmlAttributes[key].ToString().Contains(value))
                    {
                        htmlAttributes[key] += " " + value;
                    }
                }
                else if (key == "style")
                {
                    htmlAttributes[key] += ";" + value;
                }
                else
                {
                    htmlAttributes[key] = value;
                }
            }
            else
            {
                htmlAttributes.Add(key, value);
            }
        }

        protected virtual string BeginForm<TModel>(HtmlHelper htmlHelper, RoboUIFormResult<TModel> roboForm, string formId) where TModel : class
        {
            if (roboForm.Layout == RoboUIFormLayout.Wizard)
            {
                roboForm.FormActionUrl = null;
            }

            string formActionUrl = string.IsNullOrEmpty(roboForm.FormActionUrl)
                ? htmlHelper.ViewContext.HttpContext.Request.RawUrl
                : roboForm.FormActionUrl;

            var form = new FluentTagBuilder("form", TagRenderMode.StartTag)
                .MergeAttribute("action", formActionUrl)
                .MergeAttribute("id", formId)
                .MergeAttribute("method", roboForm.FormMethod.ToString().ToLowerInvariant());

            if (roboForm.Layout == RoboUIFormLayout.Tab)
            {
                form = form.MergeAttribute("data-ajax-begin", formId.Replace("-", "_") + "_ValidateTabs");
            }

            if (!roboForm.AjaxEnabled)
            {
                form = form.MergeAttribute("enctype", "multipart/form-data");
            }
            else
            {
                form = form.MergeAttribute("data-ajax", "true");
            }

            form = form.MergeAttribute("method", roboForm.FormMethod.ToString().ToLowerInvariant());
            form = form.MergeAttributes(roboForm.HtmlAttributes);

            return form.ToString();
        }

        protected virtual string BeginForm<TModel>(HtmlHelper htmlHelper, RoboUIGridResult<TModel> roboGrid) where TModel : class
        {
            if (string.IsNullOrEmpty(roboGrid.FormActionUrl))
            {
                return string.Empty;
            }

            var form = new FluentTagBuilder("form", TagRenderMode.StartTag)
                .MergeAttribute("action", roboGrid.FormActionUrl)
                .MergeAttribute("method", "post");

            if (roboGrid.IsAjaxSupported)
            {
                form = form.MergeAttribute("data-ajax", "true");
            }

            return form.ToString();
        }
    }
}
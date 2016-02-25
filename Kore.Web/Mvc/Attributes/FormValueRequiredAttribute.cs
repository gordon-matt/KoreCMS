using System;
using System.Diagnostics;
using System.Reflection;
using System.Web.Mvc;

namespace Kore.Web.Mvc
{
    public class FormValueRequiredAttribute : ActionMethodSelectorAttribute
    {
        private readonly string[] submitButtonNames;
        private readonly FormValueRequirement requirement;

        public FormValueRequiredAttribute(params string[] submitButtonNames) :
            this(FormValueRequirement.Equals, submitButtonNames)
        {
        }

        public FormValueRequiredAttribute(FormValueRequirement requirement, params string[] submitButtonNames)
        {
            // At least one submit button should be found
            this.submitButtonNames = submitButtonNames;
            this.requirement = requirement;
        }

        public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
        {
            foreach (string buttonName in submitButtonNames)
            {
                try
                {
                    string value = string.Empty;
                    switch (this.requirement)
                    {
                        case FormValueRequirement.Equals:
                            {
                                // Don't iterate because an "Invalid request" exception can be thrown
                                value = controllerContext.HttpContext.Request.Form[buttonName];
                            }
                            break;

                        case FormValueRequirement.StartsWith:
                            {
                                foreach (var formValue in controllerContext.HttpContext.Request.Form.AllKeys)
                                {
                                    if (formValue.StartsWith(buttonName, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        value = controllerContext.HttpContext.Request.Form[formValue];
                                        break;
                                    }
                                }
                            }
                            break;
                    }
                    if (!String.IsNullOrEmpty(value))
                        return true;
                }
                catch (Exception exc)
                {
                    // Try-catch to ensure that
                    Debug.WriteLine(exc.Message);
                }
            }
            return false;
        }
    }

    public enum FormValueRequirement : byte
    {
        Equals,
        StartsWith
    }
}
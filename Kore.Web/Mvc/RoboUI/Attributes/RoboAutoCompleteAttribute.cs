using System;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboAutoCompleteAttribute : RoboControlAttribute
    {
        public RoboAutoCompleteAttribute()
        {
            MinLength = 2;
        }

        public int MinLength { get; set; }

        public bool MustMatch { get; set; }

        public RoboAutoCompleteOptions Options { get; set; }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            scriptRegister.IncludeBundle("jquery-ui");
            styleRegister.IncludeBundle("jquery-ui");
        }
    }

    public class RoboAutoCompleteOptions
    {
        public string SourceUrl { get; set; }

        public virtual bool HasTextSelector { get { return false; } }

        public virtual string GetText(object model)
        {
            return null;
        }
    }

    public class RoboAutoCompleteOptions<TModel> : RoboAutoCompleteOptions
    {
        public override bool HasTextSelector
        {
            get { return TextSelector != null; }
        }

        public Func<TModel, string> TextSelector { get; set; }

        public override string GetText(object model)
        {
            return TextSelector != null ? TextSelector((TModel)model) : null;
        }
    }
}
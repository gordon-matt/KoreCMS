using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.Fakes;
using Kore.Web.Mvc.Resources;
using Kore.Web.Mvc.RoboUI.Providers;

namespace Kore.Web.Mvc.RoboUI
{
    public abstract class RoboUIResult : ViewResult
    {
        private readonly IDictionary<string, string> hiddenValues;

        protected RoboUIResult()
        {
            hiddenValues = new Dictionary<string, string>();
        }

        public IDictionary<string, string> HiddenValues
        {
            get { return hiddenValues; }
        }

        public IRoboUIFormProvider RoboUIFormProvider { get; set; }

        public string Title { get; set; }

        public void AddHiddenValue(string name, string value)
        {
            hiddenValues.Add(name, value);
        }

        public override void ExecuteResult(ControllerContext controllerContext)
        {
            if (OverrideExecuteResult())
            {
                return;
            }

            if (!string.IsNullOrEmpty(Title))
            {
                controllerContext.Controller.ViewBag.Title = Title;
            }

            // Generate Robo Form content
            var roboFormContent = GenerateView();

            //TODO: Merge FakeHttpContext2 with FakeHttpContext
            var fakeHttpContext = new FakeHttpContext2(controllerContext.HttpContext);
            var fakeContext = new ControllerContext(fakeHttpContext, controllerContext.RouteData, controllerContext.Controller);
            ViewData = controllerContext.Controller.ViewData;
            base.ExecuteResult(fakeContext);

            using (var reader = new StreamReader(fakeHttpContext.Response.OutputStream))
            {
                var str = reader.ReadToEnd();
                str = str.Replace("[ROBO_UI_PLACEHOLDER]", roboFormContent);
                controllerContext.HttpContext.Response.Write(str);
            }
        }

        public abstract string GenerateView();

        public virtual void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
        }

        public virtual bool OverrideExecuteResult()
        {
            return false;
        }

        protected override ViewEngineResult FindView(ControllerContext controllerContext)
        {
            var result = ViewEngineCollection.FindView(controllerContext, "RoboFormResult_", null);

            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var scriptRegister = new ScriptRegister(workContext);
            var styleRegister = new StyleRegister(workContext);
            GetAdditionalResources(scriptRegister, styleRegister);

            return result;
        }
    }
}
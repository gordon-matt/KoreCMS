using System.IO;
using System.Text;
using System.Web.Mvc;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUIViewResult : RoboUIResult
    {
        private readonly ControllerBase controllerBase;

        public RoboUIViewResult(string viewName, ControllerBase controllerBase, object model = null)
        {
            this.controllerBase = controllerBase;
            ViewName = viewName;
            ViewModel = model;
        }

        public object ViewModel { get; set; }

        public override string GenerateView()
        {
            var result = ViewEngines.Engines.FindPartialView(controllerBase.ControllerContext, ViewName);

            if (result != null && result.View != null)
            {
                var viewData = new ViewDataDictionary();

                if (ViewModel != null)
                {
                    viewData.Model = ViewModel;
                }

                var sb = new StringBuilder();
                using (var textWriter = new StringWriter(sb))
                {
                    var viewContext = new ViewContext(controllerBase.ControllerContext, result.View, viewData, new TempDataDictionary(), textWriter)
                    {
                        ViewData = controllerBase.ViewData
                    };
                    result.View.Render(viewContext, textWriter);
                    return sb.ToString();
                }
            }

            return null;
        }
    }
}
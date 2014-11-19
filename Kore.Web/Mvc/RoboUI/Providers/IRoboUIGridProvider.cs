using System.Web.Mvc;
using Kore.Web.Mvc.Resources;

namespace Kore.Web.Mvc.RoboUI.Providers
{
    public interface IRoboUIGridProvider
    {
        string Render<TModel>(RoboUIGridResult<TModel> roboUIGrid, HtmlHelper htmlHelper) where TModel : class;

        void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister);

        RoboUIGridRequest CreateGridRequest(ControllerContext controllerContext);

        void ExecuteGridRequest<TModel>(RoboUIGridResult<TModel> roboUIGrid, RoboUIGridRequest request, ControllerContext controllerContext) where TModel : class;
    }
}
using System;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Castle.Core.Logging;
using Kore.Infrastructure;
using Kore.Localization;
using Kore.Logging;
//using Kore.Web.Mvc.Controls.Grid;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Mvc
{
    public abstract class KoreController : Controller
    {
        private HtmlHelper html = null;

        public IWebWorkContext WorkContext { get; set; }

        public Localizer T { get; set; }

        public ILogger Logger { get; set; }

        public HtmlHelper Html
        {
            get
            {
                if (html == null)
                {
                    html = new HtmlHelper(new ViewContext(), new ViewDataContainer(ViewData));
                }
                return html;
            }
        }

        protected string ClientIPAddress
        {
            get
            {
                var ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (ipAddress == null || ipAddress.ToLower() == "unknown")
                {
                    ipAddress = Request.ServerVariables["REMOTE_ADDR"];
                }

                return ipAddress;
            }
        }

        protected KoreController()
        {
            WorkContext = EngineContext.Current.Resolve<IWebWorkContext>();
            T = LocalizationUtilities.Resolve();
            Logger = LoggingUtilities.Resolve();
            //Logger = NullLogger.Instance;
        }

        protected virtual bool CheckPermission(Permission permission)
        {
            //TODO: if (membershipService.SupportsRolePermissions)
            var authorizationService = EngineContext.Current.Resolve<IAuthorizationService>();
            return authorizationService.TryCheckAccess(permission, WorkContext.CurrentUser);
        }

        //#region Update Model

        //protected void TryUpdateModel(object model, Type modelType, IValueProvider valueProvider = null)
        //{
        //    var binder = new DefaultModelBinder();
        //    var bindingContext = new ModelBindingContext
        //    {
        //        ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, modelType),
        //        ModelState = ViewData.ModelState,
        //        ValueProvider = valueProvider ?? ValueProvider //provider
        //    };

        //    binder.BindModel(ControllerContext, bindingContext);
        //}

        //#endregion Update Model

        //public string RenderRazorViewToString(string viewName, string masterName, object model)
        //{
        //    ViewData.Model = model;
        //    using (var sw = new StringWriter())
        //    {
        //        var viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ControllerContext, viewName, masterName);
        //        var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}

        public virtual ActionResult RedirectToHomePage()
        {
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        public virtual string RenderRazorPartialViewToString(string viewName, object model, dynamic viewBag = null)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data
            };
        }

        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                Data = data,
                JsonRequestBehavior = behavior
            };
        }
    }
}
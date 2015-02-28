using System.IO;
using System.Text;
using System.Web.Mvc;
using ElFinder;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Media.Services;
using Kore.Web.Mvc;
using Kore.Web.Mvc.Resources;
using Kore.Web.Mvc.RoboUI;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Media)]
    [RoutePrefix("media-library")]
    public class MediaController : KoreController
    {
        [Route("")]
        public ActionResult Index()
        {
            if (!CheckPermission(MediaPermissions.ManageMedia))
            {
                return new HttpUnauthorizedResult();
            }

            WorkContext.Breadcrumbs.Add(T("Media"));

            ViewBag.Title = T("Manage Media");

            return View("Kore.Web.ContentManagement.Areas.Admin.Media.Views.Media.Index");
        }

        [Route("browse")]
        public ActionResult Browse()
        {
            return View("Kore.Web.ContentManagement.Areas.Admin.Media.Views.Media.Browse");
        }

        //[HttpPost]
        [Route("connector")]
        public ActionResult Connector()
        {
            return ConnectorInstance.Process(HttpContext.Request);
        }

        private Connector connectorInstance;

        private Connector ConnectorInstance
        {
            get
            {
                if (connectorInstance == null)
                {
                    var driver = EngineContext.Current.Resolve<IDriver>();
                    var pathProvider = EngineContext.Current.Resolve<IMediaPathProvider>();

                    if (driver is FileSystemDriver)
                    {
                        var fileSystemDriver = (driver as FileSystemDriver);

                        string publicPath = pathProvider.PublicPath.TrimEnd(new[] { '\\' });

                        var thumbsStorage = new DirectoryInfo(Server.MapPath(publicPath));
                        fileSystemDriver.AddRoot(new Root(new DirectoryInfo(Server.MapPath(publicPath)), "/Media/")
                        {
                            Alias = "My Documents",
                            StartPath = new DirectoryInfo(Server.MapPath(Path.Combine(publicPath, "Default"))),
                            ThumbnailsStorage = thumbsStorage,
                            //MaxUploadSizeInMb = 2.2,
                            ThumbnailsUrl = "/admin/media/media-library/thumbnails/"
                        });
                        connectorInstance = new Connector(fileSystemDriver);
                    }
                    else
                    {
                        connectorInstance = new Connector(driver);
                    }
                }
                return connectorInstance;
            }
        }

        public ActionResult SelectFile(string target)
        {
            return Json(ConnectorInstance.GetFileByHash(target).FullName);
        }

        [Route("thumbnails/{thumbHash}")]
        public ActionResult Thumbs(string thumbHash)
        {
            return ConnectorInstance.GetThumbnail(Request, Response, thumbHash);
        }
    }
}
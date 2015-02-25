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

            var sb = new StringBuilder();
            sb.Append("<div id=\"elfinder\"></div>");

            var scriptRegister = new ScriptRegister(WorkContext);
            var styleRegister = new StyleRegister(WorkContext);

            scriptRegister.IncludeInline(string.Format(@"
	                var myCommands = elFinder.prototype._options.commands;
	                var disabled = ['extract', 'archive', 'resize', 'help', 'select'];
	                $.each(disabled, function (i, cmd) {{
	                    var idx;
	                    (idx = $.inArray(cmd, myCommands)) !== -1 && myCommands.splice(idx, 1);
	                }});
	                var options = {{
	                    url: '{0}',
	                    requestType: 'post',
	                    commands: myCommands,
	                    lang: 'en',
	                    ui: ['toolbar'],
	                    rememberLastDir: false,
	                    height: 500,
	                    resizable: false,
                        defaultView: 'list',
	                    uiOptions: {{
	                        toolbar: [
                                ['home', 'up'],
                                ['mkdir', 'upload'],
                                ['info'],
                                ['quicklook'],
                                ['cut', 'paste'],
                                ['rm'],
                                ['view', 'sort']
	                        ],
	                        tree: {{
	                            openRootOnLoad: false,
	                        }},
	                        cwd: {{
	                            oldSchool: true
	                        }}
	                    }},
	                    contextmenu: {{
	                        navbar: ['open', '|', 'cut', 'paste', '|', 'rm', '|', 'info'],

	                        cwd: ['reload', 'back', '|', 'upload', 'paste', '|', 'info'],

	                        files: [
                                'getfile', '|', 'open', 'quicklook', '|', 'download', '|', 'cut', 'paste', '|',
                                'rm', '|', 'edit', 'rename', 'resize', '|', 'archive', 'extract', '|', 'info'
	                        ]
	                    }},
	                    handlers: {{
	                        upload: function (event, instance) {{
	                            var uploadedFiles = event.data.added;
	                            var archives = ['application/x-gzip', 'application/x-tar', 'application/x-bzip2'];
	                            for (i in uploadedFiles) {{
	                                var file = uploadedFiles[i];
	                                if (jQuery.inArray(file.mime, archives) >= 0) {{
	                                    instance.exec('extract', file.hash);
	                                }}
	                            }}
	                        }},
	                    }},
	                    dialog: {{ width: 900, modal: true, title: ""Files"" }}, // open in dialog window
	                    commandsOptions: {{
	                        getfile: {{
	                            onlyURL: true,
	                            multiple: false,
	                            folders: false,
	                            oncomplete: ''
	                        }},
	                    }}
	                }};
	                $('#elfinder').elfinder(options).elfinder('instance');", @Url.Action("Connector", "Media", RouteData.Values)));

            scriptRegister.IncludeBundle("jquery-ui");
            scriptRegister.IncludeBundle("jquery-migrate");
            scriptRegister.IncludeBundle("elfinder");

            styleRegister.IncludeBundle("jquery-ui");
            styleRegister.IncludeBundle("elfinder");

            var result = new RoboUIContentResult(sb.ToString());
            return result;
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
                        var thumbsStorage = new DirectoryInfo(Server.MapPath(pathProvider.PublicPath));
                        fileSystemDriver.AddRoot(new Root(new DirectoryInfo(Server.MapPath(pathProvider.PublicPath)), "/Media/")
                        {
                            Alias = "My Documents",
                            StartPath = new DirectoryInfo(Server.MapPath(pathProvider.PublicPath)),
                            ThumbnailsStorage = thumbsStorage,
                            //MaxUploadSizeInMb = 2.2,
                            ThumbnailsUrl = "Thumbnails/"
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

        //public ActionResult Index()
        //{
        //    return ConnectorInstance.Process(this.HttpContext.Request);
        //}

        public ActionResult SelectFile(string target)
        {
            return Json(ConnectorInstance.GetFileByHash(target).FullName);
        }

        [Route("Thumbnails/{tmb}")]
        public ActionResult Thumbs(string tmb)
        {
            return ConnectorInstance.GetThumbnail(Request, Response, tmb);
        }

        //#region File Tree Connector

        //[HttpPost]
        //[Route("file-tree-connector")]
        //public ActionResult FileTreeConnector(string dir)
        //{
        //    var path = string.IsNullOrEmpty(dir) ? null : Server.UrlDecode(dir.Trim('/'));

        //    var mediaService = EngineContext.Current.Resolve<IMediaService>();
        //    var folders = mediaService.GetMediaFolders(path);
        //    var files = mediaService.GetMediaFiles(path);

        //    var sb = new StringBuilder();

        //    sb.Append("<ul class=\"jqueryFileTree\" style=\"display: none;\">");

        //    foreach (var folder in folders)
        //    {
        //        if (string.IsNullOrEmpty(dir) && folder.Name == "UploadFiles")
        //        {
        //            continue;
        //        }
        //        sb.AppendFormat("<li class=\"directory collapsed\"><a href=\"#\" rel=\"{0}/\">{1}</a></li>", folder.MediaPath.Replace('\\', '/'), folder.Name);
        //    }

        //    foreach (var file in files)
        //    {
        //        sb.AppendFormat("<li class=\"file ext_{0}\"><a href=\"#\" rel=\"{1}\">{2}</a></li>", Path.GetExtension(file.Name), file.MediaPath, file.Name);
        //    }

        //    sb.Append("</ul>");

        //    return Content(sb.ToString());
        //}

        //[HttpPost]
        //[Route("file-tree-connector/new-folder")]
        //public ActionResult NewFolder(string folder)
        //{
        //    var path = string.IsNullOrEmpty(folder) ? null : folder.Trim('/');
        //    if (string.IsNullOrEmpty(path))
        //    {
        //        return null;
        //    }

        //    var parentPath = Path.GetDirectoryName(path);
        //    var folderName = Path.GetFileName(path);
        //    var mediaService = EngineContext.Current.Resolve<IMediaService>();
        //    mediaService.CreateFolder(parentPath, folderName);

        //    return Content(folder + "/");
        //}

        //#endregion File Tree Connector
    }
}
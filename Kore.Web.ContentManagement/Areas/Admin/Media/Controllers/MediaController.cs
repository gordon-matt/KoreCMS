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
            var target = Request.QueryString["target"];

            var sb = new StringBuilder();

            if (target != null)
            {
                sb.Append("<div class=\"box media-browse\">");
                sb.Append("<div class=\"box-header\"><span class=\"title\">File Browse</span><div class=\"buttons\"><a class=\"btn btn-close\" href=\"javascript:void(0)\" onclick=\"parent.jQuery.fancybox.close();\"><i class=\"kore-icon kore-icon-close\"></i></a></div></div>");
                sb.Append("<div class=\"box-content nopadding\" style=\"border-bottom: none;\">");
            }

            sb.Append("<div id=\"fileContainer\" style=\"height: 190px; padding-left: 5px;\"></div>");

            if (target != null)
            {
                sb.Append("</div>");
                sb.Append("</div>");
            }

            var scriptRegister = new ScriptRegister(WorkContext);
            var styleRegister = new StyleRegister(WorkContext);

            if (target != null)
            {
                scriptRegister.IncludeInline(string.Format("$('#fileContainer').fileTree({{ root: '/', script: '{0}', multiFolder: false }}, function(file){{ try{{parent.document.getElementById('{1}').value = file; parent.jQuery.fancybox.close(); }}catch(e){{}} }});",
                    Url.Action("FileTreeConnector", "Media", new { area = Constants.Areas.Media }), target));
            }
            else
            {
                scriptRegister.IncludeInline(string.Format("$('#fileContainer').fileTree({{ root: '/', script: '{0}', multiFolder: false }}, function(file){{ try{{parent.fileclick(file);}}catch(e){{}} }});",
                    Url.Action("FileTreeConnector", "Media", new { area = Constants.Areas.Media })));
            }

            scriptRegister.IncludeBundle("jquery-filetree");
            styleRegister.IncludeBundle("jquery-filetree");

            var result = new RoboUIContentResult(sb.ToString());

            return result;
        }

        [HttpPost]
        [Route("connector")]
        public ActionResult Connector()
        {
            var driver = EngineContext.Current.Resolve<IDriver>();
            var connector = new Connector(driver);
            return connector.Process(HttpContext.Request);
        }

        #region File Tree Connector

        [HttpPost]
        [Route("file-tree-connector")]
        public ActionResult FileTreeConnector(string dir)
        {
            var path = string.IsNullOrEmpty(dir) ? null : Server.UrlDecode(dir.Trim('/'));

            var mediaService = EngineContext.Current.Resolve<IMediaService>();
            var folders = mediaService.GetMediaFolders(path);
            var files = mediaService.GetMediaFiles(path);

            var sb = new StringBuilder();

            sb.Append("<ul class=\"jqueryFileTree\" style=\"display: none;\">");

            foreach (var folder in folders)
            {
                if (string.IsNullOrEmpty(dir) && folder.Name == "UploadFiles")
                {
                    continue;
                }
                sb.AppendFormat("<li class=\"directory collapsed\"><a href=\"#\" rel=\"{0}/\">{1}</a></li>", folder.MediaPath.Replace('\\', '/'), folder.Name);
            }

            foreach (var file in files)
            {
                sb.AppendFormat("<li class=\"file ext_{0}\"><a href=\"#\" rel=\"{1}\">{2}</a></li>", Path.GetExtension(file.Name), file.MediaPath, file.Name);
            }

            sb.Append("</ul>");

            return Content(sb.ToString());
        }

        [HttpPost]
        [Route("file-tree-connector/new-folder")]
        public ActionResult NewFolder(string folder)
        {
            var path = string.IsNullOrEmpty(folder) ? null : folder.Trim('/');
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            var parentPath = Path.GetDirectoryName(path);
            var folderName = Path.GetFileName(path);
            var mediaService = EngineContext.Current.Resolve<IMediaService>();
            mediaService.CreateFolder(parentPath, folderName);

            return Content(folder + "/");
        }

        #endregion File Tree Connector
    }
}
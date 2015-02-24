using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using Kore.Infrastructure;
using Kore.Web.ContentManagement.Areas.Admin.Media.Services;
using Kore.Web.ContentManagement.FileSystems.Media;
using Kore.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Controllers
{
    [Authorize]
    [RouteArea(Constants.Areas.Media)]
    [RoutePrefix("media-library/plugo-browser")]
    public class PlugoBrowserController : KoreController
    {
        private static readonly List<string> imageExtensions = new List<string> { "png", "jpg", "gif" };

        private readonly Lazy<IMediaService> mediaService;

        public PlugoBrowserController(Lazy<IMediaService> mediaService)
        {
            this.mediaService = mediaService;
        }

        [Route("dialog")]
        public ActionResult Dialog()
        {
            return View("PlugoBrowserDialog");
        }

        [Route("test")]
        public ActionResult Test()
        {
            var settings = new JObject
            {
                {"upload_dir", Url.Action("UploadFiles", "UploadFiles")},
                {"allowed_extensions", "jpg,png,doc"},
                {"date_format", string.Format(
                    "{{0:{0}}}",
                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern)},
                {"datetime_format", string.Format(
                    "{{0:{0} {1}}}",
                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern,
                    Thread.CurrentThread.CurrentCulture.DateTimeFormat.LongTimePattern)},
                //{"date_format", WorkContext.ShortDatePattern},
                //{"datetime_format", WorkContext.FullDateTimePattern},
                {"per_page", 10},
                {"disable_settings", 1},
                {"session_upload_dir", "FilesUpload"},
            };

            var result = new JObject { { "success", true }, { "settings", settings } };
            return Content(result.ToString(), "application/json");
        }

        [Route("dirList")]
        public ActionResult DirList(string path)
        {
            var dir = string.IsNullOrEmpty(path) ? null : Server.UrlDecode(path.Trim('/'));
            var storageProvider = EngineContext.Current.Resolve<IStorageProvider>();
            var folders = storageProvider.ListFolders(dir);
            var files = storageProvider.ListFiles(dir);

            var result = new JObject { { "path", path }, { "relativePath", storageProvider.GetPublicUrl(dir) }, { "pathSplit", new JArray() } };

            result["dirList"] = new JArray { folders.Select(x => new JObject
            {
                { "name", x.Name },
                { "size", ""},
                { "last_modified", x.LastUpdated }
            }).ToArray() };

            result["fileList"] = new JArray { files.Select(ConvertMediaFileToJson).ToArray() };
            return Content(result.ToString(), "application/json");
        }

        private static JObject ConvertMediaFileToJson(IStorageFile x)
        {
            var obj = new JObject
            {
                {"name", x.GetName()},
                {"size", x.GetSize().ToFileSize()},
                {"last_modified", x.GetLastUpdated()},
                {"icon", x.GetFileType().SafeTrim('.')}
            };

            if (imageExtensions.Contains(obj["icon"].ToString()))
            {
                using (var stream = x.OpenRead())
                {
                    using (var sourceImage = Image.FromStream(stream, false, false))
                    {
                        obj["width"] = sourceImage.Width;
                        obj["height"] = sourceImage.Height;
                    }
                }
            }

            return obj;
        }

        [Route("dirTree")]
        public ActionResult DirTree(string path)
        {
            var dir = string.IsNullOrEmpty(path) ? null : Server.UrlDecode(path.Trim('/'));
            var folders = mediaService.Value.GetMediaFolders(dir);

            var result = new JObject();
            result["/"] = new JArray { folders.Select(x => x.Name).ToArray() };
            return Content(result.ToString(), "application/json");
        }

        [Route("getFileInfo")]
        public ActionResult GetFileInfo(string path)
        {
            var split = path.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var folderPath = split.Length > 1 ? string.Join("/", split, 0, split.Length - 1) : string.Empty;
            var file = mediaService.Value.GetMediaFile(folderPath, split[split.Length - 1]);
            if (file == null)
            {
                return null;
            }

            var fileInfo = new JObject();
            fileInfo["relativePath"] = mediaService.Value.GetMediaPublicUrl(folderPath, file.Name);
            fileInfo["path"] = path;
            fileInfo["baseName"] = file.Name;
            fileInfo["fileName"] = Path.GetFileNameWithoutExtension(file.Name);
            fileInfo["extension"] = fileInfo["icon"] = Path.GetExtension(file.Name).SafeTrim('.');
            fileInfo["size"] = file.Size.ToFileSize();
            fileInfo["modified"] = file.LastUpdated.ToString("O");
            fileInfo["width"] = 100;
            fileInfo["height"] = 100;

            var result = new JObject { { "path", path }, { "fileInfo", fileInfo } };
            return Content(result.ToString(), "application/json");
        }

        [Route("getDirTreeOptions")]
        public ActionResult GetDirTreeOptions()
        {
            var folders = mediaService.Value.GetMediaFolders(string.Empty);
            var result = new JObject();
            result["dirTree"] = "<option value=\"/\">Root</option>" + string.Join("", folders.Select(x => string.Format("<option value=\"{0}\">{1}</option>", x.Name, x.Name)));
            return Content(result.ToString(), "application/json");
        }

        [Route("createDir")]
        public ActionResult CreateDir(string name, string parent)
        {
            try
            {
                var dir = string.IsNullOrEmpty(parent) ? null : Server.UrlDecode(parent.Trim('/'));
                mediaService.Value.CreateFolder(dir, name);
                return Json(new { result = true, path = parent }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("removeDir")]
        public ActionResult RemoveDir(string path)
        {
            try
            {
                mediaService.Value.DeleteFolder(path);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("removeFile")]
        public ActionResult RemoveFile(string path)
        {
            try
            {
                mediaService.Value.DeleteFile(path);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
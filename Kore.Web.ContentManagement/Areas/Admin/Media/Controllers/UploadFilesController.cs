using System.IO;
using System.Web.Mvc;
using ElFinder;
using Kore.Web.ContentManagement.Areas.Admin.Media.Models;
using Kore.Web.ContentManagement.Areas.Admin.Media.Services;
using Newtonsoft.Json.Linq;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Controllers
{
    [Authorize]
    [RouteArea(CmsConstants.Areas.Media)]
    [RoutePrefix("media-library/upload-files")]
    public class UploadFilesController : Controller
    {
        private readonly IMediaService mediaService;

        public UploadFilesController(IMediaService mediaService)
        {
            this.mediaService = mediaService;
        }

        [Route("")]
        public ActionResult UploadFiles(FileUpload fileUpload)
        {
            var fileName = fileUpload.Filename;
            var folder = Request.Form["folder"] ?? Request.Form["path"];
            if (string.IsNullOrEmpty(folder))
            {
                folder = "UploadFiles";
            }

            if (mediaService.FileExists(folder + "\\" + fileName))
            {
                fileName = mediaService.GetUniqueFilename(folder, fileName);
            }

            var mediaUrl = mediaService.UploadMediaFile(folder, fileName, fileUpload.InputStream);
            var newUuid = Helper.EncodePath(Path.Combine(folder, fileName));
            var result = new { mediaUrl, newUuid, id = newUuid };

            return new FileUploaderResult(true, result);
        }

        [AcceptVerbs(HttpVerbs.Delete | HttpVerbs.Post)]
        [Route("delete-file/{id}")]
        public ActionResult DeleteUploadFile(string id)
        {
            var path = Helper.DecodePath(id);
            mediaService.DeleteFile(path);
            return new FileUploaderResult(true);
        }

        private class FileUploaderResult : ActionResult
        {
            private const string ResponseContentType = "text/plain";

            private readonly bool success;
            private readonly string error;
            private readonly bool? preventRetry;
            private readonly JObject otherData;

            public FileUploaderResult(bool success, object otherData = null, string error = null, bool? preventRetry = null)
            {
                this.success = success;
                this.error = error;
                this.preventRetry = preventRetry;

                if (otherData != null)
                    this.otherData = JObject.FromObject(otherData);
            }

            public override void ExecuteResult(ControllerContext context)
            {
                var response = context.HttpContext.Response;
                response.ContentType = ResponseContentType;
                response.Write(BuildResponse());
            }

            private string BuildResponse()
            {
                var response = otherData ?? new JObject();
                response["success"] = success;
                response["result"] = success;

                if (!string.IsNullOrWhiteSpace(error))
                    response["error"] = error;

                if (preventRetry.HasValue)
                    response["preventRetry"] = preventRetry.Value;

                return response.ToString();
            }
        }
    }
}
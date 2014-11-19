using System;
using System.IO;
using System.Web.Mvc;

namespace Kore.Web.ContentManagement.Areas.Admin.Media.Models
{
    [ModelBinder(typeof(ModelBinder))]
    public class FileUpload
    {
        public Guid Id { get; set; }

        public string Filename { get; set; }

        public Stream InputStream { get; set; }

        public string ContentType { get; set; }

        public class ModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var request = controllerContext.RequestContext.HttpContext.Request;
                var formUpload = request.Files.Count > 0;

                var uploadFile = formUpload ? request.Files[0] : null;

                // find filename
                var xFileName = request.Headers["X-File-Name"];
                var qqFile = request["qqfile"];
                var qquuid = request["qquuid"];

                var formFilename = formUpload ? uploadFile.FileName : null;

                var contentType = formUpload ? uploadFile.ContentType : request["Content-Type"];

                var upload = new FileUpload
                {
                    Id = !string.IsNullOrEmpty(qquuid) ? new Guid(qquuid) : Guid.NewGuid(),
                    Filename = xFileName ?? qqFile ?? formFilename,
                    InputStream = formUpload ? uploadFile.InputStream : request.InputStream,
                    ContentType = contentType
                };

                return upload;
            }
        }
    }
}
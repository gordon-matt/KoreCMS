using System;
using Kore.Web.Mvc.Resources;
using Newtonsoft.Json.Linq;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboFileUploadAttribute : RoboControlAttribute
    {
        public bool EnableFineUploader { get; set; }

        public string AllowedExtensions { get; set; }

        public bool ShowThumbnail { get; set; }

        public string UploadFolder { get; set; }

        public bool EnableBrowse { get; set; }

        public static JArray GetAllowedExtensions(string allowedExtensions)
        {
            var result = new JArray();
            if (!string.IsNullOrEmpty(allowedExtensions))
            {
                var split = allowedExtensions.Split(new[] { ",", " " }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var str in split)
                {
                    result.Add(str);
                }
            }

            return result;
        }

        public override void GetAdditionalResources(ScriptRegister scriptRegister, StyleRegister styleRegister)
        {
            if (EnableFineUploader)
            {
                scriptRegister.IncludeBundle("fine-uploader");
                styleRegister.IncludeBundle("fine-uploader");
            }

            if (EnableBrowse)
            {
                scriptRegister.IncludeBundle("fancybox");
                styleRegister.IncludeBundle("fancybox");
            }
        }
    }

    public class RoboFileUploadOptions
    {
        public string UploadUrl { get; set; }

        public string AllowedExtensions { get; set; }

        public int SizeLimit { get; set; }

        public string UploadFolder { get; set; }
    }
}
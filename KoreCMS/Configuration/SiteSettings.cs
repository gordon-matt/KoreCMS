//using System;
//using System.ComponentModel.DataAnnotations;
//using Kore.Web.Configuration;

//namespace KoreCMS.Configuration
//{
//    public class SiteSettings : ISettings
//    {
//        public SiteSettings()
//        {
//            SiteName = "Kore CMS";
//        }

//        #region ISettings Members

//        [ScaffoldColumn(false)]
//        public string Name
//        {
//            get { return "Site Settings"; }
//        }

//        [ScaffoldColumn(false)]
//        public string EditorTemplatePath
//        {
//            get { throw new NotImplementedException(); }
//        }

//        #endregion ISettings Members

//        [Display(Name = "Site Name")]
//        public string SiteName { get; set; }
//    }
//}
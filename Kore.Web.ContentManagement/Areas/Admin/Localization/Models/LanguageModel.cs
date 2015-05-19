//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;
//using Kore.Data;
//using Kore.Localization.Domain;

//namespace Kore.Web.ContentManagement.Areas.Admin.Localization.Models
//{
//    public class LanguageModel : IEntity
//    {
//        [HiddenInput]
//        [ScaffoldColumn(false)]
//        public Guid Id { get; set; }

//        [Required]
//        [StringLength(255)]
//        public string Name { get; set; }

//        [Display(Name = "Culture Code")]
//        [Required]
//        [StringLength(10)]
//        public string CultureCode { get; set; }

//        [Display(Name = "Unique SEO Code")]
//        [Required]
//        [StringLength(10)]
//        public string UniqueSeoCode { get; set; }

//        [Display(Name = "Flag Image File Name")]
//        [StringLength(255)]
//        public string FlagImageFileName { get; set; }

//        [Display(Name = "Is Right-to-Left")]
//        [Required]
//        public bool IsRTL { get; set; }

//        [Display(Name = "Is Enabled")]
//        [Required]
//        public bool IsEnabled { get; set; }

//        [Display(Name = "Sort Order")]
//        [Required]
//        public int SortOrder { get; set; }

//        #region IEntity Members

//        public object[] KeyValues
//        {
//            get { return new object[] { Id }; }
//        }

//        #endregion IEntity Members

//        public static implicit operator LanguageModel(Language entity)
//        {
//            if (entity == null)
//            {
//                return null;
//            }

//            return new LanguageModel
//            {
//                Id = entity.Id,
//                Name = entity.Name,
//                CultureCode = entity.CultureCode,
//                UniqueSeoCode = entity.UniqueSeoCode,
//                FlagImageFileName = entity.FlagImageFileName,
//                IsRTL = entity.IsRTL,
//                IsEnabled = entity.IsEnabled,
//                SortOrder = entity.SortOrder
//            };
//        }
//    }
//}
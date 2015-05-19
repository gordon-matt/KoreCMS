//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;
//using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;

//namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Models
//{
//    public class MenuItemModel
//    {
//        [HiddenInput]
//        [ScaffoldColumn(false)]
//        public Guid Id { get; set; }

//        [HiddenInput]
//        [ScaffoldColumn(false)]
//        public Guid MenuId { get; set; }

//        [Required]
//        [StringLength(255)]
//        public string Text { get; set; }

//        [Required]
//        [StringLength(255)]
//        public string Url { get; set; }

//        public bool IsExternalUrl { get; set; }

//        [StringLength(255)]
//        public string Description { get; set; }

//        public Guid? ParentId { get; set; }

//        [Display(Name = "Css Class")]
//        [StringLength(255)]
//        public string CssClass { get; set; }

//        [Required]
//        public int Position { get; set; }

//        public bool Enabled { get; set; }

//        public int Rank { get; set; }

//        public static implicit operator MenuItemModel(MenuItem other)
//        {
//            return new MenuItemModel
//            {
//                Id = other.Id,
//                MenuId = other.MenuId,
//                Position = other.Position,
//                Text = other.Text,
//                Description = other.Description,
//                Url = other.Url,
//                IsExternalUrl = other.IsExternalUrl,
//                CssClass = other.CssClass,
//                ParentId = other.ParentId,
//                Enabled = other.Enabled
//            };
//        }
//    }
//}
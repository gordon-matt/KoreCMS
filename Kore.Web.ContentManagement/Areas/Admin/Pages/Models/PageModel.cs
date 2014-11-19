using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Pages.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Pages.Models
{
    public class PageModel
    {
        [HiddenInput]
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Title { get; set; }

        [Required]
        [StringLength(255)]
        public string Slug { get; set; }

        [Display(Name = "Meta Keywords")]
        [Required]
        [StringLength(255)]
        public string MetaKeywords { get; set; }

        [Display(Name = "Meta Description")]
        [Required]
        [StringLength(255)]
        public string MetaDescription { get; set; }

        [Display(Name = "Show On Menu")]
        public Guid? ShowOnMenuId { get; set; }

        [Display(Name = "Css Class")]
        [StringLength(255)]
        public string CssClass { get; set; }

        [Display(Name = "Enabled")]
        public bool IsEnabled { get; set; }

        [AllowHtml]
        [Display(Name = "Body Content")]
        [UIHint("RichTextEditorFull")]
        public string BodyContent { get; set; }

        [HiddenInput]
        [ScaffoldColumn(false)]
        public string CultureCode { get; set; }

        [HiddenInput]
        [ScaffoldColumn(false)]
        public Guid? RefId { get; set; }

        public static implicit operator PageModel(Page other)
        {
            if (other == null)
            {
                return null;
            }

            return new PageModel
            {
                Id = other.Id,
                Title = other.Title,
                Slug = other.Slug,
                MetaKeywords = other.MetaKeywords,
                MetaDescription = other.MetaDescription,
                IsEnabled = other.IsEnabled,
                BodyContent = other.BodyContent,
                CultureCode = other.CultureCode,
                RefId = other.RefId,
                CssClass = other.CssClass
            };
        }
    }
}
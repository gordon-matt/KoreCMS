using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;

namespace Kore.Web.ContentManagement.Areas.Admin.Menus.Models
{
    public class MenuModel
    {
        [HiddenInput]
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string UrlFilter { get; set; }

        public static implicit operator MenuModel(Menu other)
        {
            return new MenuModel
            {
                Id = other.Id,
                Name = other.Name,
                UrlFilter = other.UrlFilter
            };
        }
    }
}
using System.Collections.Generic;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public interface IAutoMenuProvider
    {
        string RootUrlSlug { get; }

        IEnumerable<MenuItem> GetMainMenuItems();

        IEnumerable<MenuItem> GetSubMenuItems(string currentUrlSlug);
    }
}
using System.Collections.Generic;
using System.Web.Mvc;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public interface IAutoMenuProvider
    {
        string RootUrlSlug { get; }

        MenuItem GetRootMenuItem();

        IEnumerable<MenuItem> GetSubMenuItems(string currentUrlSlug);
    }
}
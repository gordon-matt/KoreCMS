using System.Collections.Generic;
using System.Security.Principal;
using Kore.Web.ContentManagement.Areas.Admin.Menus.Domain;

namespace Kore.Web.ContentManagement.Infrastructure
{
    public interface IAutoMenuProvider
    {
        string RootUrlSlug { get; }

        IEnumerable<MenuItem> GetMainMenuItems(IPrincipal user);

        IEnumerable<MenuItem> GetSubMenuItems(string currentUrlSlug, IPrincipal user);
    }
}
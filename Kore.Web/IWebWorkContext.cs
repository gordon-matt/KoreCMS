using System.Collections.Generic;
using System.Web;
using Kore.Web.Mvc.Notify;
using Kore.Web.Navigation;

namespace Kore.Web
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWebWorkContext : IWorkContext
    {
        HttpContextBase HttpContext { get; }

        BreadcrumbCollection Breadcrumbs { get; set; }

        ICollection<NotifyEntry> Notifications { get; set; }

        string CurrentDesktopTheme { get; }

        string CurrentMobileTheme { get; }
    }
}
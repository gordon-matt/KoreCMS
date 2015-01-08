using System.Collections.Generic;
using System.Web;
using Kore.Security.Membership;
using Kore.Web.Mvc.Notify;
using Kore.Web.Navigation;

namespace Kore.Web
{
    /// <summary>
    /// Work context
    /// </summary>
    public interface IWebWorkContext
    {
        T GetState<T>(string name);

        void SetState<T>(string name, T value);

        HttpContextBase HttpContext { get; }

        BreadcrumbCollection Breadcrumbs { get; set; }

        ICollection<NotifyEntry> Notifications { get; set; }

        string CurrentDesktopTheme { get; }

        string CurrentMobileTheme { get; }

        string CurrentCultureCode { get; }

        KoreUser CurrentUser { get; }

        string ShortDatePattern { get; }

        string FullDateTimePattern { get; }
    }
}
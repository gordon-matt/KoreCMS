using System.Web.Mvc;

namespace Kore.Web.Mvc.Controls
{
    public class ExtendedSelectListItem : SelectListItem
    {
        public object HtmlAttributes { get; set; }

        public string Category { get; set; }
    }
}
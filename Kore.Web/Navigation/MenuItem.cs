using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using Kore.Web.Security.Membership.Permissions;

namespace Kore.Web.Navigation
{
    public class MenuItem
    {
        public MenuItem()
        {
            Permissions = Enumerable.Empty<Permission>();
        }

        public string Text { get; set; }

        public string Description { get; set; }

        public string Url { get; set; }

        public string Href { get; set; }

        public string Position { get; set; }

        public bool Selected { get; set; }

        public RouteValueDictionary RouteValues { get; set; }

        public IEnumerable<MenuItem> Items { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }

        public string CssClass { get; set; }

        public string IconCssClass { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
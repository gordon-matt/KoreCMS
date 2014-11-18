using System.Collections.Generic;
using Kore.Web.Mvc.RoboUI.Filters;

namespace Kore.Web.Mvc.RoboUI
{
    public class RoboUIGridRequest
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public IList<SortDescriptor> Sorts { get; set; }

        public IList<IFilterDescriptor> Filters { get; set; }

        public string NodeId { get; set; }

        public int NodeLevel { get; set; }
    }
}
using System.Collections.Generic;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public static class FilterDescriptorFactory
    {
        public static IList<IFilterDescriptor> Create(string input)
        {
            IList<IFilterDescriptor> list = new List<IFilterDescriptor>();
            IFilterNode filterNode = new FilterParser(input).Parse();
            if (filterNode == null)
                return list;
            var filterNodeVisitor = new FilterNodeVisitor();
            filterNode.Accept(filterNodeVisitor);
            list.Add(filterNodeVisitor.Result);
            return list;
        }
    }
}
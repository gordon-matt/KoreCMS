using System.Collections.Generic;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class FilterNodeVisitor : IFilterNodeVisitor
    {
        private readonly Stack<IFilterDescriptor> context;

        public FilterNodeVisitor()
        {
            context = new Stack<IFilterDescriptor>();
        }

        public IFilterDescriptor Result
        {
            get { return context.Pop(); }
        }

        private IFilterDescriptor CurrentDescriptor
        {
            get
            {
                if (context.Count > 0)
                    return context.Peek();
                return null;
            }
        }

        public void StartVisit(IOperatorNode operatorNode)
        {
            var filterDescriptor1 = new FilterDescriptor
            {
                Operator = operatorNode.FilterOperator
            };
            var filterDescriptor2 = CurrentDescriptor as CompositeFilterDescriptor;
            if (filterDescriptor2 != null)
                filterDescriptor2.FilterDescriptors.Add(filterDescriptor1);
            context.Push(filterDescriptor1);
        }

        public void StartVisit(ILogicalNode logicalNode)
        {
            var filterDescriptor1 = new CompositeFilterDescriptor
            {
                LogicalOperator = logicalNode.LogicalOperator
            };
            var filterDescriptor2 = CurrentDescriptor as CompositeFilterDescriptor;
            if (filterDescriptor2 != null)
                filterDescriptor2.FilterDescriptors.Add(filterDescriptor1);
            context.Push(filterDescriptor1);
        }

        public void Visit(PropertyNode propertyNode)
        {
            ((FilterDescriptor)CurrentDescriptor).Member = propertyNode.Name;
        }

        public void EndVisit()
        {
            if (context.Count <= 1)
                return;
            context.Pop();
        }

        public void Visit(IValueNode valueNode)
        {
            ((FilterDescriptor)CurrentDescriptor).Value = valueNode.Value;
        }
    }
}
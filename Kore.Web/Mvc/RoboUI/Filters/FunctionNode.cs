using System.Collections.Generic;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class FunctionNode : IFilterNode, IOperatorNode
    {
        public FunctionNode()
        {
            Arguments = new List<IFilterNode>();
        }

        public IList<IFilterNode> Arguments { get; private set; }

        public void Accept(IFilterNodeVisitor visitor)
        {
            visitor.StartVisit(this);
            foreach (IFilterNode filterNode in Arguments)
                filterNode.Accept(visitor);
            visitor.EndVisit();
        }

        public FilterOperator FilterOperator { get; set; }
    }
}
namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class OrNode : IFilterNode, ILogicalNode
    {
        public IFilterNode First { get; set; }

        public IFilterNode Second { get; set; }

        public void Accept(IFilterNodeVisitor visitor)
        {
            visitor.StartVisit(this);
            First.Accept(visitor);
            Second.Accept(visitor);
            visitor.EndVisit();
        }

        public FilterCompositionLogicalOperator LogicalOperator
        {
            get { return FilterCompositionLogicalOperator.Or; }
        }
    }
}
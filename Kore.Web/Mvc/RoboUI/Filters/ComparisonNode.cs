namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class ComparisonNode : IFilterNode, IOperatorNode
    {
        public virtual IFilterNode First { get; set; }

        public virtual IFilterNode Second { get; set; }

        public void Accept(IFilterNodeVisitor visitor)
        {
            visitor.StartVisit(this);
            First.Accept(visitor);
            Second.Accept(visitor);
            visitor.EndVisit();
        }

        public FilterOperator FilterOperator { get; set; }
    }
}
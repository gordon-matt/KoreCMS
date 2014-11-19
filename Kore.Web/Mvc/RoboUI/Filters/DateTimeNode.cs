namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class DateTimeNode : IFilterNode, IValueNode
    {
        public void Accept(IFilterNodeVisitor visitor)
        {
            visitor.Visit(this);
        }

        public object Value { get; set; }
    }
}
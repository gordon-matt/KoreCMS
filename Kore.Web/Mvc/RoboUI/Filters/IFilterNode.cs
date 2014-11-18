namespace Kore.Web.Mvc.RoboUI.Filters
{
    public interface IFilterNode
    {
        void Accept(IFilterNodeVisitor visitor);
    }
}
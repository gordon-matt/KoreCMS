﻿namespace Kore.Web.Mvc.RoboUI.Filters
{
    public interface IFilterNodeVisitor
    {
        void Visit(PropertyNode propertyNode);

        void Visit(IValueNode valueNode);

        void StartVisit(ILogicalNode logicalNode);

        void StartVisit(IOperatorNode operatorNode);

        void EndVisit();
    }
}
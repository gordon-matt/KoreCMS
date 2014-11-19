using System;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    public class PredicateFilterDescription : FilterDescription
    {
        private readonly Delegate predicate;

        public PredicateFilterDescription(Delegate predicate)
        {
            this.predicate = predicate;
        }

        public override bool SatisfiesFilter(object dataItem)
        {
            return (bool)predicate.DynamicInvoke(new[]
            {
                dataItem
            });
        }

        public override object Clone()
        {
            return MemberwiseClone();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Kore.Web.Mvc.RoboUI.Filters
{
    /// <summary>
    ///     Represents a filtering descriptor which serves as a container for one or more child filtering descriptors.
    /// </summary>
    public class CompositeFilterDescriptor : FilterDescriptorBase
    {
        private FilterDescriptorCollection filterDescriptors;

        /// <summary>
        ///     Gets or sets the logical operator used for composing of
        ///     <see cref="P:Kore.Web.Mvc.RoboUI.Filters.CompositeFilterDescriptor.FilterDescriptors" />.
        /// </summary>
        /// <value>
        ///     The logical operator used for composition.
        /// </value>
        public FilterCompositionLogicalOperator LogicalOperator { get; set; }

        /// <summary>
        ///     Gets or sets the filter descriptors that will be used for composition.
        /// </summary>
        /// <value>
        ///     The filter descriptors used for composition.
        /// </value>
        public FilterDescriptorCollection FilterDescriptors
        {
            get
            {
                if (filterDescriptors == null)
                    SetFilterDescriptors(new FilterDescriptorCollection());
                return filterDescriptors;
            }
            set
            {
                if (filterDescriptors == value)
                    return;
                SetFilterDescriptors(value);
            }
        }

        /// <summary>
        ///     Creates a predicate filter expression combining
        ///     <see cref="P:Kore.Web.Mvc.RoboUI.Filters.CompositeFilterDescriptor.FilterDescriptors" />
        ///     expressions with <see cref="P:Kore.Web.Mvc.RoboUI.Filters.CompositeFilterDescriptor.LogicalOperator" />.
        /// </summary>
        /// <param name="parameterExpression">The parameter expression, which will be used for filtering.</param>
        /// <returns>
        ///     A predicate filter expression.
        /// </returns>
        protected override Expression CreateFilterExpression(ParameterExpression parameterExpression)
        {
            var expressionBuilder = new FilterDescriptorCollectionExpressionBuilder(parameterExpression,
                FilterDescriptors, LogicalOperator);
            expressionBuilder.Options.CopyFrom(ExpressionBuilderOptions);
            return expressionBuilder.CreateBodyExpression();
        }

        private void SetFilterDescriptors(FilterDescriptorCollection value)
        {
            filterDescriptors = value;
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            base.Serialize(json);
            json["logic"] = ((object)LogicalOperator).ToString().ToLowerInvariant();
            if (!FilterDescriptors.Any())
                return;
            json["filters"] =
                JsonObjectExtensions.ToJson(FilterDescriptors.OfType<JsonObject>());
        }

        public override object Clone()
        {
            var instance = (CompositeFilterDescriptor)MemberwiseClone();
            instance.FilterDescriptors = new FilterDescriptorCollection();
            foreach (var filterDescriptor in FilterDescriptors)
            {
                instance.FilterDescriptors.Add((IFilterDescriptor)filterDescriptor.Clone());
            }
            return instance;
        }
    }
}
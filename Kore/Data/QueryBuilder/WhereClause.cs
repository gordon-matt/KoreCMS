// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Kore.Data.QueryBuilder
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a WHERE clause on 1 database column, containing 1 or more comparisons on
    /// that column, chained together by logic operators: eg (UserID=1 or UserID=2 or UserID>100)
    /// This can be achieved by doing this:
    /// WhereClause myWhereClause = new WhereClause("UserID", Comparison.Equals, 1);
    /// myWhereClause.AddClause(LogicOperator.Or, Comparison.Equals, 2);
    /// myWhereClause.AddClause(LogicOperator.Or, Comparison.GreaterThan, 100);
    /// </summary>
    public struct WhereClause
    {
        private string fieldName;
        private ComparisonOperator comparisonOperator;
        private object value;

        internal struct SubClause
        {
            public LogicOperator LogicOperator;
            public ComparisonOperator ComparisonOperator;
            public object Value;

            public SubClause(LogicOperator logic, ComparisonOperator compareOperator, object compareValue)
            {
                LogicOperator = logic;
                ComparisonOperator = compareOperator;
                Value = compareValue;
            }
        }

        internal ICollection<SubClause> SubClauses;

        /// <summary>
        /// Gets/sets the name of the database column this WHERE clause should operate on
        /// </summary>
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        /// <summary>
        /// Gets/sets the comparison method
        /// </summary>
        public ComparisonOperator ComparisonOperator
        {
            get { return comparisonOperator; }
            set { comparisonOperator = value; }
        }

        /// <summary>
        /// Gets/sets the value that was set for comparison
        /// </summary>
        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }

        public WhereClause(string field, ComparisonOperator firstCompareOperator, object firstCompareValue)
        {
            fieldName = field;
            comparisonOperator = firstCompareOperator;
            value = firstCompareValue;
            SubClauses = new List<SubClause>();
        }

        public void AddClause(LogicOperator logic, ComparisonOperator compareOperator, object compareValue)
        {
            var subClause = new SubClause(logic, compareOperator, compareValue);
            SubClauses.Add(subClause);
        }
    }
}
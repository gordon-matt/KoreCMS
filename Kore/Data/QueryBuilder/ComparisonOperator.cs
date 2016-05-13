// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Kore.Data.QueryBuilder
{
    /// <summary>
    /// Represents comparison operators for WHERE, HAVING and JOIN clauses
    /// </summary>
    public enum ComparisonOperator : byte
    {
        EqualTo,
        NotEqualTo,
        Like,
        NotLike,
        GreaterThan,
        GreaterThanOrEqualTo,
        LessThan,
        LessThanOrEqualTo,
        In,
        Contains,
        NotContains,
        StartsWith,
        EndsWith,
    }
}
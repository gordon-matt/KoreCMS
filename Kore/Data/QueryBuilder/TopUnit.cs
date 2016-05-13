// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Kore.Data.QueryBuilder
{
    /// <summary>
    /// Represents a unit for TOP clauses in SELECT statements
    /// </summary>
    public enum TopUnit : byte
    {
        Records,
        Percent
    }
}
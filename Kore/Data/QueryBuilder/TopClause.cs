// Based on code by Ewout Stortenbeker
// CodeEngine Framework (http://www.code-engine.com/)
// Email: 4ewout@gmail.com
// The version used in here has been heavily modified from the original

namespace Kore.Data.QueryBuilder
{
    public struct TopClause
    {
        public int Quantity;
        public TopUnit Unit;

        public TopClause(int quantity)
        {
            Quantity = quantity;
            Unit = TopUnit.Records;
        }

        public TopClause(int nr, TopUnit aUnit)
        {
            Quantity = nr;
            Unit = aUnit;
        }
    }
}
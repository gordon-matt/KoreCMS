namespace Kore.Web.Mvc.RoboUI
{
    public class GridLayout
    {
        public GridLayout(int column, int row, int columnSpan = 1, int rowSpan = 1)
        {
            Column = column;
            Row = row;
            ColumnSpan = columnSpan;
            RowSpan = rowSpan;
        }

        public int Column { get; private set; }

        public int ColumnSpan { get; private set; }

        public int Row { get; private set; }

        public int RowSpan { get; private set; }
    }
}
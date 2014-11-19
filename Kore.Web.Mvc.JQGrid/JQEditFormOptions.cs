namespace Kore.Web.Mvc.JQGrid
{
    public class JQEditFormOptions
    {
        public string ElmPrefix { get; set; }

        public string ElmSuffix { get; set; }

        public string Label { get; set; }

        public int? RowPos { get; set; }

        public int? ColPos { get; set; }

        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
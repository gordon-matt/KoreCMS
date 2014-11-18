namespace Kore.Web.Mvc.JQGrid.DataReaders
{
    public class JsonReader
    {
        public JsonReader()
        {
            RepeatItems = false;
        }

        public string Id { get; set; }

        public bool RepeatItems { get; set; }
    }
}
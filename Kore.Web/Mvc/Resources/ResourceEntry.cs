namespace Kore.Web.Mvc.Resources
{
    public class ResourceEntry
    {
        public ResourceEntry(string type, string path)
        {
            Type = type;
            Path = path;
            Order = 9999;
        }

        public string Type { get; private set; }

        public string Path { get; private set; }

        public int Order { get; set; }

        public object HtmlAttributes { get; set; }
    }
}
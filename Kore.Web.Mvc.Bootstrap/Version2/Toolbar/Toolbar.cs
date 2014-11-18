namespace Kore.Web.Mvc.Bootstrap.Version2
{
    public class Toolbar : HtmlElement
    {
        public Toolbar()
            : this(null)
        {
        }

        public Toolbar(object htmlAttributes)
            : base("div", htmlAttributes)
        {
            EnsureClass("btn-toolbar");
        }
    }
}
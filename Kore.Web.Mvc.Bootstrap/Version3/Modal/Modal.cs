namespace Kore.Web.Mvc.Bootstrap.Version3
{
    public class Modal : HtmlElement
    {
        public Modal()
            : this(null)
        {
        }

        public Modal(object htmlAttributes)
            : base("div", htmlAttributes)
        {
            EnsureClass("modal fade");
        }
    }
}
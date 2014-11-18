namespace Kore.Web.Mvc.Bootstrap.Version2
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
            EnsureClass("modal hide");
        }
    }
}
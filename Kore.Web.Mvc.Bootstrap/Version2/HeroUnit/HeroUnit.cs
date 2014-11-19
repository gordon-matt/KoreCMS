namespace Kore.Web.Mvc.Bootstrap.Version2
{
    public class HeroUnit : HtmlElement
    {
        public string Heading { get; set; }

        public string Tagline { get; set; }

        public HeroUnit(string heading, string tagline)
            : this(heading, tagline, null)
        {
        }

        public HeroUnit(string heading, string tagline, object htmlAttributes)
            : base("div", htmlAttributes)
        {
            this.Heading = heading;
            this.Tagline = tagline;
            EnsureClass("hero-unit");
        }
    }
}
namespace Kore.Web.Mvc.EmbeddedViews
{
    public interface IEmbeddedResourceResolver
    {
        EmbeddedResourceTable Scripts { get; }

        EmbeddedResourceTable Views { get; }
    }
}
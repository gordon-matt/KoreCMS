namespace Kore.Web.Mvc.EmbeddedResources
{
    public interface IEmbeddedResourceResolver
    {
        EmbeddedResourceTable Scripts { get; }

        EmbeddedResourceTable Styles { get; }

        EmbeddedResourceTable Views { get; }
    }
}
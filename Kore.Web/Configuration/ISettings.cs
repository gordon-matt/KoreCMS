namespace Kore.Web.Configuration
{
    public interface ISettings
    {
        string Name { get; }

        string EditorTemplatePath { get; }
    }
}
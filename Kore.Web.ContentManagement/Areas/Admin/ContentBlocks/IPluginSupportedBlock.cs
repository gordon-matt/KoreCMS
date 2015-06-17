namespace Kore.Web.ContentManagement.Areas.Admin.ContentBlocks
{
    public interface IPluginSupportedBlock
    {
        string PluginDisplayTemplatePath { get; }

        string PluginEditorTemplatePath { get; }
    }
}
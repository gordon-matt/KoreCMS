namespace Kore.Web.Common.Areas.Admin.Regions
{
    public interface IRegionSettings
    {
        string Name { get; }

        string EditorTemplatePath { get; }
    }
}
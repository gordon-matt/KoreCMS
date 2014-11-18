namespace Kore.Localization
{
    public interface ILocalizedStringManager
    {
        string GetLocalizedString(string text, string cultureCode);
    }

    //public class LocalizedStringManager : ILocalizedStringManager
    //{
    //    public string GetLocalizedString(string text, string cultureCode)
    //    {
    //        return text;
    //    }
    //}
}
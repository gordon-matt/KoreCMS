using System.Collections.Generic;

namespace Kore.Localization
{
    public interface ILanguageManager
    {
        IEnumerable<Language> GetActiveLanguages();
    }

    //public class DefaultLanguageManager : ILanguageManager
    //{
    //    public IEnumerable<Language> GetActiveLanguages()
    //    {
    //        yield return new Language
    //        {
    //            Name = "English",
    //            CultureCode = "en-US"
    //        };
    //    }
    //}
}
//using Kore.DI;

namespace Kore.Localization
{
    public interface ICultureManager //: IDependency
    {
        string GetCurrentCulture();

        bool IsValidCulture(string cultureName);
    }
}
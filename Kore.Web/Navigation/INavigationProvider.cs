namespace Kore.Web.Navigation
{
    public interface INavigationProvider
    {
        void GetNavigation(NavigationBuilder builder);
    }
}
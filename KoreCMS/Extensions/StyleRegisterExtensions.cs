using Kore.Web.Mvc.Resources;

namespace KoreCMS.Extensions
{
    public static class StyleRegisterExtensions
    {
        public static void IncludeBootstrap(this StyleRegister register)
        {
            register.Include("bootstrap.min.css");
            register.Include("bootstrap-theme.min.css");
        }

        public static void IncludeChosen(this StyleRegister register)
        {
            register.Include("bootstrap-chosen.css");
        }

        public static void IncludeFontAwesome(this StyleRegister register)
        {
            register.Include("font-awesome.min.css");
        }

        public static void IncludeJQueryUI(this StyleRegister register)
        {
            register.Include("themes/base/all.css");
            register.Include("themes/jquery-ui-bootstrap.css");
        }
    }
}
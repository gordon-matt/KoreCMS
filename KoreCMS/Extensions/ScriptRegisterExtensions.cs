using Kore.Web.Mvc.Resources;

namespace KoreCMS.Extensions
{
    public static class ScriptRegisterExtensions
    {
        private const int DefaultOrder = 10;

        public static void IncludeBootstrap(this ScriptRegister register)
        {
            register.Include("bootstrap.min.js").HasOrder(DefaultOrder);
        }

        public static void IncludeChosen(this ScriptRegister register)
        {
            register.Include("chosen.jquery.min.js").HasOrder(DefaultOrder);
        }

        public static void IncludeFancyBox(this ScriptRegister register)
        {
            register.Include("jquery.fancybox.pack.js").HasOrder(DefaultOrder);
        }

        public static void IncludeJQGrid(this ScriptRegister register, string cultureCode = "en")
        {
            if (cultureCode == "en" || string.IsNullOrEmpty(cultureCode))
            {
                register.Include("i18n/grid.locale-en.js").HasOrder(DefaultOrder);
            }
            else
            {
                string localizedFile = string.Format("i18n/grid.locale-{0}.js", cultureCode);
                register.Include(localizedFile).HasOrder(DefaultOrder);
            }

            register.Include("jquery.jqGrid.min.js").HasOrder(DefaultOrder);
        }

        public static void IncludeJQuery(this ScriptRegister register)
        {
            register.Include("jquery-2.1.1.min.js").HasOrder(0);
            register.Include("jquery-migrate-1.2.1.min.js").HasOrder(1);
        }

        public static void IncludeJQueryUI(this ScriptRegister register)
        {
            register.Include("jquery-ui.min-1.11.1.js").HasOrder(DefaultOrder);
            register.Include("jquery-ui.unobtrusive-2.2.0.min.js").HasOrder(DefaultOrder);
        }

        public static void IncludeJQueryValidate(this ScriptRegister register)
        {
            register.IncludeExternal("/Scripts/jquery.validate.min.js", DefaultOrder);
            register.IncludeExternal("/Scripts/jquery.validate.unobtrusive.min.js", DefaultOrder);
            register.IncludeExternal("/Scripts/jquery.unobtrusive-ajax.min.js", DefaultOrder);
        }

        public static void IncludeKnockout(this ScriptRegister register)
        {
            register.Include("knockout-3.2.0.js").HasOrder(DefaultOrder);
        }

        public static void IncludeModernizr(this ScriptRegister register)
        {
            register.Include("modernizr-2.8.3.js").HasOrder(DefaultOrder);
        }
    }
}
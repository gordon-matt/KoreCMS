using Kore.Web.Mvc.Resources;

namespace KoreCMS.Extensions
{
    public static class ScriptRegisterExtensions
    {
        private const int DefaultOrder = 10;

        public static void IncludeBootstrap(this ScriptRegister register)
        {
            register.IncludeBundle("bootstrap", order: DefaultOrder);
        }

        public static void IncludeChosen(this ScriptRegister register)
        {
            register.Include("chosen.jquery.min.js", order: DefaultOrder);
        }

        public static void IncludeJQuery(this ScriptRegister register)
        {
            register.Include("jquery-3.3.1.min.js", order: DefaultOrder);
            //register.Include("jquery-migrate-1.2.1.min.js", order: DefaultOrder);
        }

        public static void IncludeJQueryUI(this ScriptRegister register)
        {
            register.IncludeBundle("jquery-ui", order: DefaultOrder);
        }

        public static void IncludeJQueryValidate(this ScriptRegister register)
        {
            register.IncludeExternal("/Scripts/jquery.validate.js", order: DefaultOrder);
            register.IncludeExternal("/Scripts/jquery.validate.unobtrusive.min.js", order: DefaultOrder);
            register.IncludeExternal("/Scripts/jquery.unobtrusive-ajax.min.js", order: DefaultOrder);
        }

        public static void IncludeKnockout(this ScriptRegister register)
        {
            register.IncludeBundle("knockout", order: DefaultOrder);
        }

        public static void IncludeModernizr(this ScriptRegister register)
        {
            register.IncludeBundle("modernizr", order: DefaultOrder);
        }
    }
}
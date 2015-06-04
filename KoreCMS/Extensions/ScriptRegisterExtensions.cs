using Kore.Web.Mvc.Resources;

namespace KoreCMS.Extensions
{
    public static class ScriptRegisterExtensions
    {
        private const int DefaultOrder = 10;

        public static void IncludeBootstrap(this ScriptRegister register)
        {
            register.IncludeBundle("bootstrap", order: DefaultOrder);
            //register.Include("bootstrap.min.js", order: DefaultOrder);
        }

        public static void IncludeChosen(this ScriptRegister register)
        {
            register.Include("chosen.jquery.min.js", order: DefaultOrder);
        }

        //public static void IncludeFancyBox(this ScriptRegister register)
        //{
        //    register.Include("jquery.fancybox.pack.js", order: DefaultOrder);
        //}

        //public static void IncludeJQGrid(this ScriptRegister register, string cultureCode = "en")
        //{
        //    if (cultureCode == "en" || string.IsNullOrEmpty(cultureCode))
        //    {
        //        register.Include("i18n/grid.locale-en.js", order: DefaultOrder);
        //    }
        //    else
        //    {
        //        string localizedFile = string.Format("i18n/grid.locale-{0}.js", cultureCode);
        //        register.Include(localizedFile, order: DefaultOrder);
        //    }

        //    register.Include("jquery.jqGrid.min.js", order: DefaultOrder);
        //}

        public static void IncludeJQuery(this ScriptRegister register)
        {
            //register.IncludeBundle("jquery", order: DefaultOrder);
            //register.IncludeBundle("jquery-migrate", order: DefaultOrder);
            register.Include("jquery-2.1.1.min.js", order: DefaultOrder);
            register.Include("jquery-migrate-1.2.1.min.js", order: DefaultOrder);
        }

        public static void IncludeJQueryUI(this ScriptRegister register)
        {
            register.IncludeBundle("jquery-ui", order: DefaultOrder);
            //register.Include("jquery-ui.min-1.11.1.js", order: DefaultOrder);
            //register.Include("jquery-ui.unobtrusive-2.2.0.min.js", order: DefaultOrder);
        }

        public static void IncludeJQueryValidate(this ScriptRegister register)
        {
            //register.IncludeBundle("jqueryval", order: DefaultOrder);
            register.IncludeExternal("/Scripts/jquery.validate.js", order: DefaultOrder);
            register.IncludeExternal("/Scripts/jquery.validate.unobtrusive.min.js", order: DefaultOrder);
            register.IncludeExternal("/Scripts/jquery.unobtrusive-ajax.min.js", order: DefaultOrder);
        }

        public static void IncludeKnockout(this ScriptRegister register)
        {
            register.IncludeBundle("knockout", order: DefaultOrder);
            //register.Include("knockout-3.2.0.js", order: DefaultOrder);
        }

        public static void IncludeModernizr(this ScriptRegister register)
        {
            register.IncludeBundle("modernizr", order: DefaultOrder);
            //register.Include("modernizr-2.8.3.js", order: DefaultOrder);
        }
    }
}
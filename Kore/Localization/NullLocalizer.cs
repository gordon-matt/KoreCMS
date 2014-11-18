namespace Kore.Localization
{
    public static class NullLocalizer
    {
        private static readonly Localizer instance;

        static NullLocalizer()
        {
            instance =
                (format, args) =>
                new LocalizedString((args == null || args.Length == 0) ? format : string.Format(format, args));
        }

        public static Localizer Instance
        {
            get { return instance; }
        }
    }
}
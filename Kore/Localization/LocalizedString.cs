using System;

namespace Kore.Localization
{
    public class LocalizedString : MarshalByRefObject
    {
        private readonly object[] args;
        private readonly string localized;
        private readonly string textHint;

        public LocalizedString(string languageNeutral)
        {
            localized = languageNeutral;
            textHint = languageNeutral;
        }

        public LocalizedString(string localized, string textHint, object[] args)
        {
            this.localized = localized;
            this.textHint = textHint;
            this.args = args;
        }

        public string TextHint
        {
            get { return textHint; }
        }

        public object[] Args
        {
            get { return args; }
        }

        public string Text
        {
            get { return localized; }
        }

        public static LocalizedString TextOrDefault(string text, LocalizedString defaultValue)
        {
            if (string.IsNullOrEmpty(text))
                return defaultValue;
            return new LocalizedString(text);
        }

        public override string ToString()
        {
            return localized;
        }

        public override int GetHashCode()
        {
            int hashCode = 0;
            if (localized != null)
                hashCode ^= localized.GetHashCode();
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var that = (LocalizedString)obj;
            return string.Equals(localized, that.localized);
        }

        public static implicit operator string(LocalizedString localized)
        {
            return localized.Text;
        }
    }
}
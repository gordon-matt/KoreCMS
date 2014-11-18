using System;
using System.Collections;
using System.Text;
using System.Web;

namespace Kore.Web
{
    public class UrlBuilder
    {
        private readonly Hashtable keyValues;
        private readonly string path;

        private UrlBuilder()
        {
            keyValues = new Hashtable(StringComparer.InvariantCultureIgnoreCase);
        }

        public UrlBuilder(Uri uri)
            : this()
        {
            path = uri.GetLeftPart(UriPartial.Path);
            AppendQueryString(uri.Query);
        }

        public UrlBuilder(string path)
            : this()
        {
            var indexOf = path.IndexOf('?');
            if (indexOf > -1)
            {
                this.path = path.Substring(0, indexOf);
                AppendQueryString(path.Substring(indexOf + 1));
            }
            else
            {
                this.path = path;
            }
        }

        public void AppendQueryString(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                return;
            }

            var queryStrings = queryString.TrimStart('?').Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var str in queryStrings)
            {
                if (string.IsNullOrEmpty(str))
                {
                    continue;
                }
                var split = str.Split('=');
                var value = split.Length == 2 ? HttpUtility.UrlDecode(split[1]) : string.Empty;
                keyValues[split[0]] = value;
            }
        }

        public void AddQueryString(string key, string value)
        {
            keyValues[key] = value;
        }

        public void RemoveQueryString(string key)
        {
            if (keyValues.ContainsKey(key))
            {
                keyValues.Remove(key);
            }
        }

        public void ClearQueryString()
        {
            keyValues.Clear();
        }

        public T GetQueryStringValue<T>(string key)
        {
            var value = keyValues[key];
            if (value == null || value.ToString() == string.Empty)
            {
                return default(T);
            }

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (FormatException)
            {
                return default(T);
            }
        }

        public override string ToString()
        {
            return ToString(path, keyValues);
        }

        private static string ToString(string urlPath, Hashtable queryStrings)
        {
            var sb = new StringBuilder();
            sb.Append(urlPath);

            if (queryStrings.Count > 0)
            {
                var hasQueryString = false;
                sb.Append("?");
                foreach (DictionaryEntry entry in queryStrings)
                {
                    if (hasQueryString)
                    {
                        sb.Append("&");
                    }
                    sb.Append(entry.Key);
                    sb.Append("=");
                    sb.Append(HttpUtility.UrlEncode(Convert.ToString(entry.Value)));
                    hasQueryString = true;
                }
            }

            return sb.ToString();
        }
    }
}
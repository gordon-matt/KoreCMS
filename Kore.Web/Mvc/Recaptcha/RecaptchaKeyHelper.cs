/* ============================================================================================================================
 * THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * =========================================================================================================================== */

namespace Kore.Web.Mvc.Recaptcha
{
    internal class RecaptchaKeyHelper
    {
        internal static string ParseKey(string key)
        {
            if (key.StartsWith("{") && key.EndsWith("}"))
            {
                return System.Configuration.ConfigurationManager.AppSettings[key.Trim().Substring(1, key.Length - 2)];
            }

            return key;
        }
    }
}
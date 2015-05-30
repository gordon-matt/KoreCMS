using System;
using System.Collections.Generic;
using System.Web;

namespace Kore.Web.ContentManagement.Areas.Admin.Messaging.Services
{
    public interface ITokenizer
    {
        /// <summary>
        /// Replace all of the token key occurences inside the specified template text with corresponded token values
        /// </summary>
        /// <param name="template">The template with token keys inside</param>
        /// <param name="tokens">The sequence of tokens to use</param>
        /// <param name="htmlEncode">The value indicating whether tokens should be HTML encoded</param>
        /// <returns>Text with all token keys replaces by token value</returns>
        string Replace(string template, IEnumerable<Token> tokens, bool htmlEncode);
    }

    public class Tokenizer : ITokenizer
    {
        /// <summary>
        /// Replace all of the token key occurences inside the specified template text with corresponded token values
        /// </summary>
        /// <param name="template">The template with token keys inside</param>
        /// <param name="tokens">The sequence of tokens to use</param>
        /// <param name="htmlEncode">The value indicating whether tokens should be HTML encoded</param>
        /// <returns>Text with all token keys replaces by token value</returns>
        public string Replace(string template, IEnumerable<Token> tokens, bool htmlEncode)
        {
            if (string.IsNullOrWhiteSpace(template))
                return template;

            if (tokens == null)
                throw new ArgumentNullException("tokens");

            foreach (var token in tokens)
            {
                var tokenValue = token.Value;

                //do not encode URLs
                if (htmlEncode && token.HtmlEncoded)
                    tokenValue = HttpUtility.HtmlEncode(tokenValue);
                template = Replace(template, token.Key, tokenValue);
            }
            return template;
        }

        private static string Replace(string original, string pattern, string replacement)
        {
            return original.Replace(pattern, replacement);
        }
    }
}
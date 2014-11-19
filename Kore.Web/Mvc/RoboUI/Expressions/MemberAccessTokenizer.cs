using System;
using System.Collections.Generic;
using System.Linq;

namespace Kore.Web.Mvc.RoboUI.Expressions
{
    public static class MemberAccessTokenizer
    {
        public static IEnumerable<IMemberAccessToken> GetTokens(string memberPath)
        {
            var members = memberPath.Split(new[] { '.', '[' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string str in members)
            {
                IndexerToken indexerToken;
                if (TryParseIndexerToken(str, out indexerToken))
                    yield return indexerToken;
                else
                    yield return new PropertyToken(str);
            }
        }

        private static bool TryParseIndexerToken(string member, out IndexerToken token)
        {
            token = null;
            if (!IsValidIndexer(member))
                return false;
            var list = new List<object>();
            list.AddRange(ExtractIndexerArguments(member).Select(ConvertIndexerArgument));
            token = new IndexerToken(list);
            return true;
        }

        private static bool IsValidIndexer(string member)
        {
            return member.EndsWith("]", StringComparison.Ordinal);
        }

        private static IEnumerable<string> ExtractIndexerArguments(string member)
        {
            var args = member.TrimEnd(new[] { ']' });
            var strArray = args.Split(new[] { ',' });
            return strArray;
        }

        private static object ConvertIndexerArgument(string argument)
        {
            int result;
            if (int.TryParse(argument, out result))
                return result;
            if (argument.StartsWith("\"", StringComparison.Ordinal))
            {
                return argument.Trim(new[]
                {
                    '"'
                });
            }
            if (!argument.StartsWith("'", StringComparison.Ordinal))
                return argument;
            var str = argument.Trim(new[]
            {
                '\''
            });
            if (str.Length == 1)
                return str[0];
            return str;
        }
    }
}
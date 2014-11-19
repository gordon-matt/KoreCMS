using System;
using System.Diagnostics;
using System.Text;

namespace Kore
{
    public static class ExceptionExtensions
    {
        public static int GetLineNumber(this Exception x)
        {
            return new StackTrace(x).GetFrame(0).GetFileLineNumber();
        }

        public static string GetMessageStack(this Exception x)
        {
            if (x == null)
            {
                return null;
            }

            var sb = new StringBuilder();
            sb.AppendLine(x.Message);

            while (x.InnerException != null)
            {
                x = x.InnerException;
                sb.Append("--> ");
                sb.AppendLine(x.Message);
            }

            return sb.ToString();
        }
    }
}
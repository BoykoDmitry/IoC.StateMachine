using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine
{
    public static class Affirm
    {
        [DebuggerHidden]
        public static void ArgumentNotNull(object s, string argName)
        {
            if (s == null)
                Exceptions.ArgNull(argName);
        }

        [DebuggerHidden]
        public static void IsNotNull(object s, string message)
        {
            if (s == null)
                Exceptions.CommonException(message);
        }

        [DebuggerHidden]
        public static void NotNullOrEmpty(string arg, string argName)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw Exceptions.ArgNullOrEmpty(argName);
            }
        }

        public static void With(bool condition, string msgFrmt, params object[] args)
        {
            if (!condition)
            {
                throw Exceptions.CommonException(Exceptions.FormatMessage(msgFrmt, args));
            }
        }

        public static string FormIt(this string text, params object[] args)
        {
            if (args == null || args.Length == 0) return text;

            return string.Format(text, args);
        }
    }
}

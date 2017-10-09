using IoC.StateMachine.Exceptions;
using System.Diagnostics;

namespace IoC.StateMachine
{
    public static class Affirm
    {
        [DebuggerHidden]
        public static void ArgumentNotNull(object s, string argName)
        {
            if (s == null)
                throw Excp.ArgNull(argName);
        }

        [DebuggerHidden]
        public static void IsNotNull(object s, string message)
        {
            if (s == null)
                throw Excp.CommonException(message);
        }

        [DebuggerHidden]
        public static void NotNullOrEmpty(string arg, string argName)
        {
            if (string.IsNullOrEmpty(arg))
            {
                throw Excp.ArgNullOrEmpty(argName);
            }
        }

        public static void With(bool condition, string msgFrmt, params object[] args)
        {
            if (!condition)
            {
                throw Excp.CommonException(msgFrmt.FormIt(args));
            }
        }

        public static string FormIt(this string text, params object[] args)
        {
            if (args == null || args.Length == 0) return text;

            return string.Format(text, args);
        }
    }
}

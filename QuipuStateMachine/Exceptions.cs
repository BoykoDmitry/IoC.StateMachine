using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine
{
    public static class Exceptions
    {
        static Exceptions()
        {

        }

        [DebuggerHidden]
        public static string FormatMessage(string messageFormat, params object[] args)
        {
            if (args != null && args.Length > 0)
            {
                messageFormat = string.Format(messageFormat, args);
            }
            return messageFormat;
        }


        [DebuggerHidden]
        public static ArgumentNullException ArgNull(string argName)
        {
            return new ArgumentNullException(argName);
        }


        [DebuggerHidden]
        public static ArgumentException ArgNullOrEmpty(string argumentName)
        {
            return new ArgumentException(
                FormatMessage("String '{0}' should be neither null nor empty", argumentName), argumentName);
        }

        [DebuggerHidden]
        public static Exception CommonException(string message)
        {
            return new Exception(message);
        }
    }
}

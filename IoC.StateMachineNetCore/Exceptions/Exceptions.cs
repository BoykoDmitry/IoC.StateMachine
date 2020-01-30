using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Exceptions
{
    public static class Excp
    {
        [DebuggerHidden]
        public static ArgumentNullException ArgNull(string argName)
        {
            return new ArgumentNullException(argName);
        }


        [DebuggerHidden]
        public static ArgumentException ArgNullOrEmpty(string argumentName)
        {
            return new ArgumentException(
                "String '{0}' should be neither null nor empty".FormIt(argumentName), argumentName);
        }

        [DebuggerHidden]
        public static Exception CommonException(string message)
        {
            return new Exception(message);
        }
    }
}

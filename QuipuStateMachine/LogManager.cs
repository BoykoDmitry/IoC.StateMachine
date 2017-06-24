using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine
{
    public static class LogManager
    {
        static readonly ILog NullLogInstance = new EmptyLog();

        /// <summary>
        /// Creates an <see cref="ILog"/> for the provided type.
        /// </summary>
        public static Func<Type, ILog> GetLog = type => NullLogInstance;

        class EmptyLog : ILog
        {
            public void Debug(string format, params object[] args) { }
            public void Error(string format, params object[] args) { }
            public void Info(string format, params object[] args) { }
        }
    }
}

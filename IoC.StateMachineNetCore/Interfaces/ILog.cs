using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    /// <summary>
    /// Encapsulates logic for logging
    /// </summary>
    public interface ILog
    {
        void Debug(string format, params object[] args);
        void Error(string format, params object[] args);
        void Info(string format, params object[] args);
    }
}

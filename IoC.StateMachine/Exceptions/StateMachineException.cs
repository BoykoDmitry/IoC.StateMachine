using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Exceptions
{
    public class StateMachineException : Exception
    {
        public StateMachineException()
        {

        }
        public StateMachineException(Guid stateMachindId, string message) : base(message)
        {
            StateMachindId = stateMachindId;
        }

        public StateMachineException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StateMachineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public Guid StateMachindId { get; set; }
    }
}

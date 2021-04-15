using IoC.StateMachine.Interfaces;
using IoC.StateMachine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Exceptions
{
    [Serializable]
    public class TooManyTriggersException : StateMachineException
    {
        private readonly IList<ITriggerHolder> _triggers;
        public IList<ITriggerHolder> Triggers { get { return _triggers; } }

        public TooManyTriggersException()
        {
        }

        public TooManyTriggersException(Guid stateMachindId, string message) : base(stateMachindId, message)
        {
        }

        public TooManyTriggersException(IList<ITriggerHolder> triggers, Guid stateMachindId, string message) : base(stateMachindId, message)
        {
            _triggers = triggers;
        }

        public TooManyTriggersException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public TooManyTriggersException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }

    [Serializable]
    public class NoTrueTriggerException : StateMachineException
    {
        public NoTrueTriggerException()
        {
        }

        public NoTrueTriggerException(Guid stateMachindId, string message) : base(stateMachindId, message)
        {
        }

        public NoTrueTriggerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public NoTrueTriggerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

    }

}

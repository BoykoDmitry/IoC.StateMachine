using System;
using System.Runtime.Serialization;

namespace IoC.StateMachine.Core
{
    /// <summary>
    /// Base for context of state machine
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class ContextBase
    {
        [DataMember]
        public string Status { get; set; }
    }
}

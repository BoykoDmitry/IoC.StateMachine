using System;
using System.Runtime.Serialization;

namespace IoC.StateMachine.Core
{
    [Serializable]
    [DataContract]
    public abstract class ContextBase
    {
        [DataMember]
        public string Status { get; set; }
    }
}

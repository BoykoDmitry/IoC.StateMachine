using IoC.StateMachine.Abstractions;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    /// <summary>
    /// Represents transition between two states <see cref="ITransition"/>
    /// </summary>
    [Serializable]
    [DataContract]
    public class Transition : SMBaseElement, ITransition
    {
        /// <summary>
        /// State from 
        /// </summary>
        [DataMember]
        public string SourceStateId { get; set; }
        
        /// <summary>
        /// State to 
        /// </summary>
        [DataMember]
        public string TargetStateId { get; set; }

        /// <summary>
        /// Trigger wrapper wich defines condition for transition
        /// </summary>
        [DataMember]
        public ITriggerHolder Trigger { get; set; }

        /// <summary>
        /// For designer, curviness of the line 
        /// </summary>
        [DataMember]
        public double Curviness { get; set; }
    }
}

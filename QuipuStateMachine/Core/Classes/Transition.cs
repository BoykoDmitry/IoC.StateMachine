using IoC.StateMachine.Interfaces;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    [Serializable]
    [DataContract]
    public class Transition : SMBaseElement, ITransition
    {
        [DataMember]
        public string SourceStateId { get; set; }
        [DataMember]
        public string TargetStateId { get; set; }
        [XmlIgnore]
        public IState ParentState { get; set; }
        [DataMember]
        public ITriggerHolder Trigger { get; set; }
        [DataMember]
        public double Curviness { get; set; }
    }
}

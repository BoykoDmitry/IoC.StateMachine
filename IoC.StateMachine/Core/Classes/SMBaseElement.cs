using IoC.StateMachine.Interfaces;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    [Serializable]
    [DataContract]
    public abstract class SMBaseElement
    {
        [XmlIgnore]
        public IStateMachine StateMachine { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string Points { get; set; }
        [DataMember]
        public string Text { get; set; }
    }
}

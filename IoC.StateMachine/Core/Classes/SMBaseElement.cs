using IoC.StateMachine.Interfaces;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    /// <summary>
    /// Base class with common properties for state machine definition classes
    /// </summary>
    [Serializable]
    [DataContract]
    public abstract class SMBaseElement
    {
        /// <summary>
        /// Parent state machine
        /// </summary>
        [XmlIgnore]
        public IStateMachine StateMachine { get; set; }

        /// <summary>
        /// id of the element, like transition or state
        /// </summary>
        [DataMember]
        public string id { get; set; }

        /// <summary>
        /// can be used to store coordinates of the element in the screen, for designer
        /// </summary>
        [DataMember]
        public string Points { get; set; }

        /// <summary>
        /// can be used to store comments and etc, for designer
        /// </summary>
        [DataMember]
        public string Text { get; set; }
    }
}

using IoC.StateMachine.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    /// <summary>
    /// Represents collection of history moves of the state machine instance
    /// </summary>
    [Serializable]
    [CollectionDataContract]
    public class Moves : List<Move>
    {
        [NonSerialized]
        object _itemsLock = new object();

        public IMove Add(string source, string target)
        {
            if (_itemsLock == null)
                _itemsLock = new object();
            lock (_itemsLock)
            {
                var m = new Move(source, target, Count);
                Add(m);
                return m;
            }
        }
    }

    /// <summary>
    /// Represents one move of the state machine
    /// </summary>
    [Serializable]
    [DataContract]
    public class Move : IMove
    {
        public Move() { }

        public Move(string source, string target, int order)
        {
            SourceStateId = source;
            TargetStateId = target;
            Order = order;
        }

        [XmlAttribute]
        [DataMember]
        public string SourceStateId { get; set; }

        [XmlAttribute]
        [DataMember]
        public string TargetStateId { get; set; }

        [XmlAttribute]
        [DataMember]
        public int Order { get; set; }
    }
}

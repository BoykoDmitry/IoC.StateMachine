using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    [DataContract]
    [Serializable]
    public class State : SMBaseElement, IState
    {
        private IList<IActionHolder> _enterActions;
        [DataMember]
        public IList<IActionHolder> EnterActions
        {
            get
            {
                _enterActions = _enterActions ?? new List<IActionHolder>();
                return _enterActions;
            }
            set
            {
                _enterActions = value;
            }
        }
        [DataMember]
        public bool StartPoint { get; set; }

        [DataMember]
        public bool EndPoint { get; set; }

        private IList<IActionHolder> _exitActions;
        [DataMember]
        public IList<IActionHolder> ExitActions
        {
            get
            {
                _exitActions = _exitActions ?? new List<IActionHolder>();
                return _exitActions;
            }
            set
            {
                _exitActions = value;
            }
        }
    }
}

using IoC.StateMachine.Interfaces;
using IoC.StateMachine.Abstractions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    /// <summary>
    /// State of state machine <see cref="IState"/>
    /// </summary>
    [DataContract]
    [Serializable]
    public class State : SMBaseElement, IState
    {
        private IList<IActionHolder> _enterActions;
        /// <summary>
        /// Actions to be executed when state machine enters in this state <see cref="ISMService.Push(IStateMachine, ISMParameters)"/>
        /// </summary>
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

        /// <summary>
        /// if state is a starting one. 
        /// </summary>
        [DataMember]
        public bool StartPoint { get; set; }

        /// <summary>
        /// if state is a finish state
        /// </summary>
        [DataMember]
        public bool EndPoint { get; set; }

        private IList<IActionHolder> _exitActions;
        /// <summary>
        /// Actions to be executed when state machine is moved from the state. Executed before next trigger is evaulated <see cref="ISMService.Push(IStateMachine, ISMParameters)
        /// </summary>
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

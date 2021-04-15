using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using IoC.StateMachine.Abstractions;

namespace IoC.StateMachine.Core
{
    /// <summary>
    /// Implementation of <see cref="IStateMachineDefinition"/>
    /// </summary>
    [Serializable]
    [DataContract]
    public sealed class StateMachineDefinition : IStateMachineDefinition
    {
        private IList<IState> _states;
        [DataMember]
        public IList<IState> States
        {
            get
            {
                _states = _states ?? new List<IState>();
                return _states;
            }
            set { _states = value; }
        }

        private IList<ITransition> _transitions;
        [DataMember]
        public IList<ITransition> Transitions
        {
            get
            {
                _transitions = _transitions ?? new List<ITransition>();
                return _transitions;
            }
            set { _transitions = value; }
        }

        public bool Validate()
        {
            if (States == null)
                throw new ArgumentException("StateMachine: States are not defined");

            if (!States.Any(_ => _.StartPoint))
                throw new ArgumentException("StateMachine: initial state is not defined");

            if (States.Count(_ => _.StartPoint) > 1)
                throw new ArgumentException("StateMachine: too many initial states");

            if (!States.Any(_ => _.EndPoint))
                throw new ArgumentException("StateMachine: final state is not defined");

            return true;
        }
    }
}

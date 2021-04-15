using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Abstractions
{
    /// <summary>
    /// Represents definition for state machine
    /// possible transisionts and states
    /// </summary>
    public interface IStateMachineDefinition
    {
        /// <summary>
        /// List of possible states 
        /// </summary>
        IList<IState> States { get; }

        /// <summary>
        /// List of possible transitions
        /// </summary>
        IList<ITransition> Transitions { get; }

        /// <summary>
        /// Validates state machine definition
        /// </summary>
        /// <returns>true if definition is correct</returns>
        bool Validate();
    }

}

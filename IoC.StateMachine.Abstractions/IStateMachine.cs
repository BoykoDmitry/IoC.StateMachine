using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Abstractions
{
    /// <summary>
    /// Represents instance of state machine 
    /// </summary>
    public interface IStateMachine : IDisposable
    {
        /// <summary>
        /// Unique ID of state machine
        /// </summary>
        Guid SmId { get; set; }

        /// <summary>
        /// Definition of state machine 
        /// </summary>
        IStateMachineDefinition Definition { get; }

        /// <summary>
        /// Current state of state machine
        /// </summary>
        IState CurrentState { get; set; }

        /// <summary>
        /// Current state id of state machine
        /// </summary>
        string CurrentStateId { get; set; }

        /// <summary>
        /// Previous state of state machine
        /// </summary>
        string PreviousStateId { get; set; }

        /// <summary>
        /// Sets current state of state machine
        /// </summary>
        /// <param name="state">State to set <see cref="IState"/></param>
		void SetCurrentState(IState state);

        /// <summary>
        /// Sets current state of state machine
        /// </summary>
        /// <param name="state">state id to be set></param>
        void SetCurrentState(string stateId);

        /// <summary>
        /// Defines if state machine was finished
        /// </summary>
        bool Finished { get; set; }

        /// <summary>
        /// Sets definition of state machine
        /// </summary>
        /// <param name="definition">Definition to set <see cref="IStateMachineDefinition"/></param>
        void SetDefinition(IStateMachineDefinition definition);

        /// <summary>
        /// List with state machine moves
        /// </summary>
		IEnumerable<IMove> StateMoves { get; set; }

        IServiceScope Container { get; set; }
    }
}

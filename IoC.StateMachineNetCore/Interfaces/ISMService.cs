
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    /// <summary>
    /// Encapsulates logic for starting and moving state machines
    /// </summary>
    public interface ISMService
    {
        /// <summary>
        /// Pushes state machine to the next state with given parameters
        /// executes exit actions, searches for next state and move state machine
        /// </summary>
        /// <param name="sm">State machine to push</param>
        /// <param name="parameters">Parameters for push</param>
        /// <returns></returns>
        IStateMachine Push(IStateMachine sm, ISMParameters parameters);

        /// <summary>
        /// Starts new state machine with given definition and parameters
        /// </summary>
        /// <param name="parameters">Parameters to start</param>
        /// <param name="definition">Definition of state machine to be started</param>
        /// <returns>Instance of <see cref="IStateMachine"/></returns>
        T Start<T>(ISMParameters parameters, IStateMachineDefinition definition) where T : class, IStateMachine;

        /// <summary>
        /// Starts new state machine with given definition and parameters
        /// </summary>
        /// <param name="def">Definition of state machine to be started</param>
        /// <param name="paramters">Parameters to start</param>
        /// <param name="smType">type of state machine <see cref="IStateMachine"/></param>
        /// <returns></returns>
		IStateMachine Start(IStateMachineDefinition def, ISMParameters paramters, Type smType);

        /// <summary>
        /// Starts new state machine with given definition and parameters
        /// </summary>
        /// <param name="def">Definition of state machine to be started</param>
        /// <param name="paramters">Parameters to start</param>
        /// <param name="key">Key to resolve type from container <see cref="IStateMachine"/></param>
        /// <returns></returns>
		IStateMachine Start(IStateMachineDefinition def, ISMParameters paramters, string key);

        /// <summary>
        /// Gets list of possible transitions
        /// </summary>
        /// <param name="sm">state machine</param>
        /// <returns></returns>
        IList<ITransition> GetTransitions(IStateMachine sm);      
    }

    /// <summary>
    /// Encapsulates logic for action exection for state 
    /// </summary>
    public interface IStateProcessor
    {
        /// <summary>
        /// Executes enter actions 
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="parameters">Parameters for execution</param>
        void ProcessState(IState state, ISMParameters parameters);

        /// <summary>
        /// Excutes exit actions 
        /// </summary>
        /// <param name="state">State</param>
        /// <param name="parameters">Parameters for execution</param>
        void ExitState(IState state, ISMParameters parameters);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    /// <summary>
    /// Base for holder classes
    /// </summary>
    public interface IHolderBase : IRoot
    {
        /// <summary>
        /// Parameters for execution
        /// </summary>
        ISMParameters Parameters { get; }

        /// <summary>
        /// Key which will be used to resolve action class from container
        /// </summary>
        string Code { get; set; }

        void SetParameters(ISMParameters parameters);

        /// <summary>
        /// Order of execution, among other actions in the list
        /// </summary>
        int Order { get; set; }
    }

    /// <summary>
    /// Generic wrapper for certain action
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IHolder<T> : IHolderBase
    {
        /// <summary>
        /// Nested action
        /// </summary>
        T NestedAction { get; set; }
    }

    /// <summary>
    /// Wrapper for trigger <see cref="ISMTrigger"/>
    /// </summary>
    public interface ITriggerHolder : IHolder<ISMTrigger>
    {
        /// <summary>
        /// Execution of the action with parameters from state machine push
        /// </summary>
        /// <param name="transition">Transition to which trigger belongs <see cref="ITransition"/></param>
        /// <param name="TransitionParameters">Parameters from state machine push <see cref="ISMService.Push(IStateMachine, ISMParameters)"/></param>
        bool Invoke(ITransition transition, ISMParameters TransitionParameters);

        /// <summary>
        /// Defines if result of the trigger must be inverted
        /// </summary>
        bool Inverted { get; set; }
    }

    /// <summary>
    /// Wrapper for action <see cref="ISMAction"/>
    /// </summary>
    public interface IActionHolder : IHolder<ISMAction>
    {
        /// <summary>
        /// Execution of the action with parameters from state machine push
        /// </summary>
        /// <param name="TransitionParameters">Parameters from state machine push <see cref="ISMService.Push(IStateMachine, ISMParameters)"/></param>
        void Invoke(ISMParameters TransitionParameters);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Abstractions
{
    /// <summary>
    /// Represents action for state(exit or enter)
    /// </summary>
    public interface ISMAction : IHaveStateMachine
    {
        /// <summary>
        /// Invokes the action
        /// </summary>
        /// <param name="Parameters">Parameters from the action wrapper</param>
        /// <param name="TransitionParameters">Parameters from push <see cref="ISMService.Push(IStateMachine, ISMParameters)"/></param>
        void Invoke(ISMParameters Parameters, ISMParameters TransitionParameters);
    }
}

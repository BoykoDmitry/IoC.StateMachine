using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    /// <summary>
    /// Represents trigger for transition
    /// </summary>
    public interface ISMTrigger : IHaveStateMachine
    {
        /// <summary>
        /// Invokes trigger 
        /// </summary>
        /// <param name="transition">Transition to which trigger belongs</param>
        /// <param name="Parameters">Parameters from trigger wrapper <see cref="ITriggerHolder"/></param>
        /// <param name="TransitionParameters">Parameters from push <see cref="ISMService.Push(IStateMachine, ISMParameters)"/></param>
        bool Invoke(ITransition transition, ISMParameters Parameters, ISMParameters TransitionParameters);
    }
}

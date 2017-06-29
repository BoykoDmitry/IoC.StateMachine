using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    /// <summary>
    /// State for state machine
    /// </summary>
    public interface IState : ISMBaseElement
    {
        /// <summary>
        /// Actions to be executed when state machine enters in this state <see cref="ISMService.Push(IStateMachine, ISMParameters)"/>
        /// </summary>
        IList<IActionHolder> EnterActions { get; }

        /// <summary>
        /// Actions to be executed when state machine is moved from the state. Executed before next trigger is evaulated <see cref="ISMService.Push(IStateMachine, ISMParameters)
        /// </summary>
        IList<IActionHolder> ExitActions { get; }

        /// <summary>
        /// if state is a starting one. 
        /// </summary>
        bool StartPoint { get; set; }

        /// <summary>
        /// if state is a finish state
        /// </summary>
        bool EndPoint { get; set; }
    }   
}

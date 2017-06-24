using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface IStateMachineDefinition
    {
        IList<IState> States { get; }
        IList<ITransition> Transitions { get; }
        bool Validate();
    }

}

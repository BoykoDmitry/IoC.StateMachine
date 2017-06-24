using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface IStateMachine
    {
        Guid SmId { get; set; }
        IStateMachineDefinition Definition { get; }
        IState CurrentState { get; set; }
        bool WasInit { get; set; }
        string CurrentStateId { get; set; }
		string PreviousStateId { get; set; }
		void SetCurrentState(IState state);
        void SetCurrentState(string stateId);
        bool Finished { get; set; }
        void SetDefinition(IStateMachineDefinition definition);
		IEnumerable<IMove> StateMoves { get; set; }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface ISMService
    {
        IStateMachine Push(IStateMachine sm, ISMParameters parameters);
        T Start<T>(ISMParameters parameters, IStateMachineDefinition definition) where T : class, IStateMachine;
		IStateMachine Start(IStateMachineDefinition def, ISMParameters paramters, Type smType);
		IStateMachine Start(IStateMachineDefinition def, ISMParameters paramters, string key);
        IList<ITransition> GetTransitions(IStateMachine sm);      
    }

    public interface IStateProcessor
    {
        void ProcessState(IState state, ISMParameters parameters);
    }
}

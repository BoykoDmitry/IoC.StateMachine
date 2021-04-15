using IoC.StateMachine.Abstractions;
using IoC.StateMachine.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Interfaces
{
    public interface IStateMachineDefinitionBuilder
    {
        IStateBuilder AddStartState(string id);
        IStateBuilder AddEndState(string id);
        IStateBuilder AddState(string id);
        IStateBuilder AddState(Action<IState> setup = null);
        ITransitionBuilder AddTransition(string id, string from, string to);
        ITransitionBuilder AddTransition(Action<ITransition> setup = null);

        StateMachineDefinition Build();
        IServiceProvider ServiceProvider { get; }
    }
}

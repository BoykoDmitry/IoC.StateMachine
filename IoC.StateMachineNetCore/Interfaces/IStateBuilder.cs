using IoC.StateMachine.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Interfaces
{
    public interface IStateBuilder 
    {
        IStateBuilder WithStartPoint();
        IStateBuilder WithEndPoint();
        IStateBuilder WithId(string id);

        IStateBuilder AddEnterAction(Action<IActionBuilder> actionBuilderAction);
        IStateBuilder AddExitAction(Action<IActionBuilder> actionBuilderAction);

        IState Build();
    }
}

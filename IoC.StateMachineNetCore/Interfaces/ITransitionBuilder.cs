using IoC.StateMachine.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Interfaces
{
    public interface ITransitionBuilder
    {        
        ITransitionBuilder WithSourceStateId(string sourceStateId);
        ITransitionBuilder WithTargetStateId(string targetStateId);

        ITransitionBuilder SetTrigger(Action<ITriggerBuilder> triggerBuilderAction);
        ITransition Build();
    }
}

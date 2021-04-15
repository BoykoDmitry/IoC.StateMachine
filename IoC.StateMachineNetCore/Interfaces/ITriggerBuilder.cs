using IoC.StateMachine.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Interfaces
{
    public interface ITriggerBuilder
    {
        ITriggerBuilder WithCode(string code);
        ITriggerBuilder Inverted();
        ITriggerBuilder WithParameter(string key, object value);

        ITriggerHolder Build();
    }
}

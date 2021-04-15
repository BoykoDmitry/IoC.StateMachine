using IoC.StateMachine.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Interfaces
{
    public interface IActionBuilder
    {
        IActionBuilder WithCode(string code);
        IActionBuilder WithOrder(int order);
        IActionBuilder WithParameter(string key, object value);

        IActionHolder Build(); 
    }
}

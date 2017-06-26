using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface IHolderBase
    {
        ISMParameters Parameters { get; }
        string Code { get; set; }

        void SetParameters(ISMParameters parameters);
        int Order { get; set; }
    }
    public interface IHolder<T> : IHolderBase
    {
        T NestedAction { get; set; }
    }

    public interface ITriggerHolder : IHolder<ISMTrigger>
    {
        bool Invoke(ITransition transition, ISMParameters TransitionParameters);
        bool Inverted { get; set; }
    }

    public interface IActionHolder : IHolder<ISMAction>
    {
        void Invoke(ISMParameters TransitionParameters);
    }
}

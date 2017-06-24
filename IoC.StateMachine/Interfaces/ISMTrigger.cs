using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface ISMTrigger
    {
        bool Invoke(ITransition transition, ISMParameters Parameters, ISMParameters TransitionParameters);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface ISMAction
    {
        void Invoke(ISMParameters Parameters, ISMParameters TransitionParameters);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface IFabric<T> 
    {
        T Get(string key);        
    }

    public interface IActionFabric : IFabric<ISMAction>
    {

    }

    public interface ITriggerFabric : IFabric<ISMTrigger>
    {

    }

    public interface ISMFactory : IFabric<IStateMachine>
    {

    }
}

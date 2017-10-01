using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    /// <summary>
    /// Abstract definition of container
    /// </summary>
    public interface IAmContainer : IDisposable
    {      
        void BuildUp(object target);
        IAmContainer GetChildContainer();
        void RegisterInstance(Type service, object instance);
        void Register(Type fromT, Type toT);
        void Register(Type fromT);
        T Get<T>(string key = null);
        object Get(Type service, string key = null);
        IEnumerable<object> GetAll(Type service);
    }
}

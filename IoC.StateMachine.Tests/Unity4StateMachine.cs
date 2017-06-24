using Microsoft.Practices.Unity;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Tests
{
    public class Unity4StateMachine : IAmContainer
    {
        private readonly IUnityContainer _container;
        public Unity4StateMachine(IUnityContainer container)
        {
            Affirm.ArgumentNotNull(container, "container");
            _container = container;
        }

        public void BuildUp(object target)
        {
            _container.BuildUp(target);
        }

        public object Get(Type service, string key = null)
        {
            return _container.Resolve(service, key);
        }

        public T Get<T>(string key = null)
        {
            return _container.Resolve<T>(key);
        }

        public IEnumerable<object> GetAll(Type service)
        {
            return _container.ResolveAll(service);
        }

        public IAmContainer GetChildContainer()
        {
            return new Unity4StateMachine(_container.CreateChildContainer());
        }

        public void Register(Type fromT)
        {
            _container.RegisterType(fromT);
        }

        public void Register(Type fromT, Type toT)
        {
            _container.RegisterType(fromT, toT);
        }

        public void RegisterInstance(Type service, object instance)
        {
            _container.RegisterInstance(service, instance);
        }
    }
}

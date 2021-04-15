using IoC.StateMachine.Abstractions;
using Lamar;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Lamar
{
    public class Factory<T> : IFabric<T>
    {
        private readonly IServiceContext _serviceProvider;
        public Factory(IServiceContext serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public T Get(string key)
        {
            return _serviceProvider.GetInstance<T>(key);
        }
    }

    public class SMFactory : Factory<IStateMachine>, ISMFactory
    {
        public SMFactory(IServiceContext serviceProvider) : base(serviceProvider)
        {
        }
    }

    public class ActionFabric : Factory<ISMAction>, IActionFabric
    {
        public ActionFabric(IServiceContext serviceProvider) : base(serviceProvider)
        {
        }
    }

    public class TriggerFabric : Factory<ISMTrigger>, ITriggerFabric
    {
        public TriggerFabric(IServiceContext serviceProvider) : base(serviceProvider)
        {
        }
    }
}

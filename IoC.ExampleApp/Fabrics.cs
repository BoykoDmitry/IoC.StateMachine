using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.ExampleApp
{
    public abstract class Factory<T> : IFabric<T>
    {
        protected readonly IServiceProvider _serviceProvider;
        protected Factory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public abstract T Get(string key);      
    }

    public class SMFactory : Factory<IStateMachine>, ISMFactory
    {
        public SMFactory(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override IStateMachine Get(string key)
        {
            return _serviceProvider.GetRequiredService<GuessStateMachine>();
        }
    }

    public class ActionFabric : Factory<ISMAction>, IActionFabric
    {
        public ActionFabric(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ISMAction Get(string key)
        {
            switch (key)
            {
                case "InitContext":
                    return _serviceProvider.GetRequiredService<InitContext>();
                case "CheckNumber":
                    return _serviceProvider.GetRequiredService<CheckNumber>();                    
            }

            throw new KeyNotFoundException();
        }
    }

    public class TriggerFabric : Factory<ISMTrigger>, ITriggerFabric
    {
        public TriggerFabric(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override ISMTrigger Get(string key)
        {
            switch (key)
            {
                case "GuessOKTrigger":
                    return _serviceProvider.GetRequiredService<GuessOKTrigger>();                    
            }

            throw new KeyNotFoundException();
        }
    }
}

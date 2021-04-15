using IoC.StateMachine.Abstractions;
using Lamar;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Lamar
{
    public abstract class Factory<T> : IFabric<T>
    {
        private readonly IServiceContext _serviceProvider;
        private readonly ILogger _logger;
        protected Factory(IServiceContext serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public virtual T Get(string key)
        {
            return _serviceProvider.GetInstance<T>(key);
        }
    }

    public class SMFactory : Factory<IStateMachine>, ISMFactory
    {
        public SMFactory(IServiceContext serviceProvider, ILogger<SMFactory> logger) : base(serviceProvider, logger)
        {
        }
    }

    public class ActionFabric : Factory<ISMAction>, IActionFabric
    {
        public ActionFabric(IServiceContext serviceProvider, ILogger<ActionFabric> logger) : base(serviceProvider, logger)
        {
        }
    }

    public class TriggerFabric : Factory<ISMTrigger>, ITriggerFabric
    {
        public TriggerFabric(IServiceContext serviceProvider, ILogger<TriggerFabric> logger) : base(serviceProvider, logger)
        {
        }
    }
}

using IoC.StateMachine.Abstractions;
using IoC.StateMachine.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Core.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSMCore(
           this IServiceCollection services,
           Action<SMConfigurationBuilder> configure = null)
        {
            var configuration = new SMConfigurationBuilder(services);
            services.AddSingleton<ISMService, SMService>()
                    .AddTransient<IStateProcessor, StateProcessor>();
            configure?.Invoke(configuration);
            EnsurePersistence(configuration);
            CheckFactories(configuration);

            return services;
        }

        private static void EnsurePersistence(SMConfigurationBuilder configuration)
        {
            var hasPersistanceService = configuration.HasService<IPersistenceService>();

            if (!hasPersistanceService)
                configuration.WithDefaultPersistance();
        }
        private static void CheckFactories(SMConfigurationBuilder configuration)
        {
            var hasSMFactory = configuration.HasService<ISMFactory>();
            var hasActionFabric = configuration.HasService<IActionFabric>();
            var hasTriggerFabric = configuration.HasService<ITriggerFabric>();

            if (!hasSMFactory)
                throw new Exception("ISMFactory is not implemented. Configuration failed!");

            if (!hasActionFabric)
                throw new Exception("IActionFabric is not implemented. Configuration failed!");

            if (!hasTriggerFabric)
                throw new Exception("ITriggerFabric is not implemented. Configuration failed!");
        }
    }
}

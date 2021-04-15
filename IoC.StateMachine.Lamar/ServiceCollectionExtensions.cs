using IoC.StateMachine.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IoC.StateMachine.Lamar
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLamarFactories(
         this IServiceCollection services
        )
        {
             services.AddSingleton<ISMFactory, SMFactory>()
                     .AddSingleton<IActionFabric, ActionFabric>()
                     .AddSingleton<ITriggerFabric, TriggerFabric>();

            return services;
        }
    }
}

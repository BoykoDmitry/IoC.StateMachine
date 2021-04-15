using IoC.StateMachine.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System;

namespace IoC.StateMachine.Json
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJsonPersistance(
           this IServiceCollection services
          )
        {
            if (services.HasService<IPersistenceService>())
                services.RemoveAll<IPersistenceService>();
          
            services.AddSingleton<IPersistenceService>(s => new JsonPersistenceService(s, 
                                                                s.GetRequiredService<ILoggerFactory>().CreateLogger<JsonPersistenceService>()
                                                                )
            );

            return services;
        }
    }
}

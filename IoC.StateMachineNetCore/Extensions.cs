using IoC.StateMachine.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine
{
    public static class StringExtensions 
    {
        public static bool IsEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }
    }

    public static class ServiceCollectionHelper
    {
        /// <summary>
        /// Registers the specified service only if none already exists for the specified provider type.
        /// </summary>
        public static IServiceCollection TryAddProvider<TService, TProvider>(
            this IServiceCollection services,
            ServiceLifetime lifetime)
        {
            return services.TryAddProvider(typeof(TService), typeof(TProvider), lifetime);
        }

        /// <summary>
        /// Registers the specified service only if none already exists for the specified provider type.
        /// </summary>
        public static IServiceCollection TryAddProvider(
            this IServiceCollection services,
            Type serviceType,
            Type providerType,
            ServiceLifetime lifetime)
        {
            var descriptor = services.FirstOrDefault(
                x => x.ServiceType == serviceType && x.ImplementationType == providerType
            );

            if (descriptor == null)
            {
                descriptor = new ServiceDescriptor(serviceType, providerType, lifetime);
                services.Add(descriptor);
            }

            return services;
        }

        public static IServiceCollection Replace<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime)
        {
            return services.Replace(new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime));
        }

        public static bool HasService<T>(this IServiceCollection services) =>
            services.Any(x => x.ServiceType == typeof(T));

        public static bool HasService<T>(this SMConfigurationBuilder configuration) =>
            configuration.Services.HasService<T>();
    }
}


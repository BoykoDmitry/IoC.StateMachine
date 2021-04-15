using IoC.StateMachine.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using IoC.StateMachine.Abstractions;
using Microsoft.Extensions.Logging;

namespace IoC.StateMachine.Serialization
{
    public static class ServiceCollectionExtensions
    {
        public static XMLSerializerBuilder WithDefaultPersistance(this SMConfigurationBuilder builder)
        {
            return new XMLSerializerBuilder(builder.Services)
                    .WithXMLDataContractPersistance();
        }

        public static XMLSerializerBuilder WithXMLDataContractPersistance(this XMLSerializerBuilder builder)
        {
            builder.Services.AddSingleton<IPersistenceService>(s => new DataContractPersistenceService(new string[] { "IoC.StateMachine" }, s, s.GetRequiredService<ILoggerFactory>().CreateLogger<DataContractPersistenceService>()));

            return builder;
        }
    }
}

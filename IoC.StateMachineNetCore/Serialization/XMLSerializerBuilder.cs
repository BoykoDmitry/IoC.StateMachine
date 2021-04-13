using IoC.StateMachine.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Serialization
{
    public class XMLSerializerBuilder : SMConfigurationBuilder
    {
        public XMLSerializerBuilder(IServiceCollection services) : base(services)
        {
        }
    }
}

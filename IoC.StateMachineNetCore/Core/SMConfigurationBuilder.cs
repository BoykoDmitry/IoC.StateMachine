using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Core
{
    public class SMConfigurationBuilder
    {
        public SMConfigurationBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}

using IoC.StateMachine.Abstractions;
using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Builder
{
    public class ActionBuilder : IActionBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IStateBuilder _stateBuilder;
        public ActionBuilder(IServiceProvider serviceProvider, IStateBuilder stateBuilder)
        {
            Affirm.ArgumentNotNull(serviceProvider, nameof(serviceProvider));
            Affirm.ArgumentNotNull(stateBuilder, nameof(stateBuilder));

            _serviceProvider = serviceProvider;
            _stateBuilder = stateBuilder;
        }
        private SMParametersCollection parametersCollection = new SMParametersCollection();
        public IActionBuilder WithParameter(string key, object value)
        {
            parametersCollection.Add(key, value);

            return this;
        }

        public IActionHolder Build()
        {
            Affirm.With(String.IsNullOrEmpty(Code), $"Code must be defined! {_stateBuilder}");

            var logger = _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<SMActionHolder>();
            var actionHolder = new SMActionHolder(logger)
            {
                Code = this.Code,
                Order = this.Order
            };

            actionHolder.Parameters = parametersCollection;

            return actionHolder;
        }

        private string Code { get; set; } = string.Empty;
        public IActionBuilder WithCode(string code)
        {
            Code = code;

            return this;
        }

        private int Order { get; set; } = -1;
        public IActionBuilder WithOrder(int order)
        {
            Order = order;

            return this;
        }
    }
}

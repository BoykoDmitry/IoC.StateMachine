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
    public class TriggerBuilder : ITriggerBuilder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ITransitionBuilder _transitionBuilder;
        public TriggerBuilder(IServiceProvider serviceProvider, ITransitionBuilder transitionBuilder)
        {
            Affirm.ArgumentNotNull(serviceProvider, nameof(serviceProvider));
            Affirm.ArgumentNotNull(transitionBuilder, nameof(transitionBuilder));

            _serviceProvider = serviceProvider;
            _transitionBuilder = transitionBuilder;
        }

        public ITriggerHolder Build()
        {
            Affirm.With(String.IsNullOrEmpty(Code), $"Code must be defined! {_transitionBuilder}");

            var logger = _serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<SMATriggerHolder>();
            var triggerHolder = new SMATriggerHolder(logger)
            {
                Code = this.Code,
                Inverted = this._inverted
            };

            triggerHolder.Parameters = parametersCollection;

            return triggerHolder;
        }

        private bool _inverted { get; set; } = false;
        public ITriggerBuilder Inverted()
        {
            _inverted = true;

            return this;
        }

        private string Code { get; set; } = string.Empty;
        public ITriggerBuilder WithCode(string code)
        {
            Code = code;

            return this;
        }

        private SMParametersCollection parametersCollection = new SMParametersCollection();
        public ITriggerBuilder WithParameter(string key, object value)
        {
            parametersCollection.Add(key, value);

            return this;
        }
    }
}

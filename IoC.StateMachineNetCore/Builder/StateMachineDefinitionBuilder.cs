using IoC.StateMachine.Abstractions;
using IoC.StateMachine.Core;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoC.StateMachine.Builder
{
    public class StateMachineDefinitionBuilder : IStateMachineDefinitionBuilder
    {
        private IList<IStateBuilder> stateBuilders = new List<IStateBuilder>();
        private IList<ITransitionBuilder> transitionBuilders = new List<ITransitionBuilder>();
        private readonly IServiceProvider _serviceProvider;

        public IServiceProvider ServiceProvider => _serviceProvider;

        public StateMachineDefinitionBuilder(IServiceProvider serviceProvider)
        {
            Affirm.ArgumentNotNull(serviceProvider, nameof(serviceProvider));

            _serviceProvider = serviceProvider;
        }

        public IStateBuilder AddStartState(string id)
        {
            return AddState(_ => 
                { 
                    _.StartPoint = true; 
                    _.id = id; 
                });
        }

        public IStateBuilder AddEndState(string id)
        {
            return AddState(_ => 
                { 
                    _.EndPoint = true; 
                    _.id = id; 
                });
        }

        public IStateBuilder AddState(string id)
        {
            return AddState(_ => _.id = id);
        }
        public IStateBuilder AddState(Action<IState> setup = null)
        {
            var stateBuilder = new StateBuilder(_serviceProvider, setup);

            stateBuilders.Add(stateBuilder);

            return stateBuilder;
        }

        public ITransitionBuilder AddTransition(Action<ITransition> setup = null)
        {
            var transitionBuilder = new TransitionBuilder(_serviceProvider, setup);

            transitionBuilders.Add(transitionBuilder);

            return transitionBuilder;
        }

        public StateMachineDefinition Build()
        {
            var def = new StateMachineDefinition()
            {
                States = stateBuilders.Select(_ => _.Build()).ToList(),
                Transitions = transitionBuilders.Select(_ => _.Build()).ToList()
            };

            def.Validate();

            return def;
        }

        public ITransitionBuilder AddTransition(string id, string from, string to)
        {
            return AddTransition(_ => 
                {
                    _.id = id; 
                    _.SourceStateId = from; 
                    _.TargetStateId = to; 
                });
        }
    }
}

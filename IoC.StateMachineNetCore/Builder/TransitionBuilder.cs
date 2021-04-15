using IoC.StateMachine.Abstractions;
using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IoC.StateMachine.Builder
{
    public class TransitionBuilder : ITransitionBuilder
    {
        private readonly Action<ITransition> _setup = null;
        private readonly IServiceProvider _serviceProvider;
        public TransitionBuilder(IServiceProvider serviceProvider, Action<ITransition> setup = null)
        {
            Affirm.ArgumentNotNull(serviceProvider, nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _setup = setup;
        }
        public ITransition Build()
        {
            var transition = new Transition();
           
            transition.id = this.Id;
            transition.SourceStateId = this.SourceStateId;
            transition.TargetStateId = this.TargetStateId;

            _setup?.Invoke(transition);

            Affirm.With(string.IsNullOrEmpty(transition.id), $"id of transition must be defined {transition.SourceStateId} -> {transition.TargetStateId}");
            Affirm.With(string.IsNullOrEmpty(transition.SourceStateId), $"SourceStateId of transition must be defined id: {transition.id}, TargetStateId: {transition.TargetStateId}");
            Affirm.With(string.IsNullOrEmpty(transition.TargetStateId), $"TargetStateId of transition must be defined id: {transition.id}, SourceStateId: {transition.SourceStateId}");
           
            Affirm.With(TriggerBuilder == null, $"Trigger must be defined for transition with id {transition.id}, {transition.SourceStateId} -> {transition.TargetStateId}");

            transition.Trigger = TriggerBuilder.Build();

            return transition;
        }

        private ITriggerBuilder TriggerBuilder { get; set; }
        public ITransitionBuilder SetTrigger(Action<ITriggerBuilder> triggerBuilderAction)
        {
            Affirm.ArgumentNotNull(triggerBuilderAction, nameof(triggerBuilderAction));

            TriggerBuilder = new TriggerBuilder(_serviceProvider, this);

            triggerBuilderAction.Invoke(TriggerBuilder);
            return this;
        }

        private string SourceStateId { get; set; } = string.Empty;
        public ITransitionBuilder WithSourceStateId(string sourceStateId)
        {
            SourceStateId = sourceStateId;

            return this;
        }

        private string TargetStateId { get; set; } = string.Empty;
        public ITransitionBuilder WithTargetStateId(string targetStateId)
        {
            TargetStateId = targetStateId;

            return this;
        }
        private string Id { get; set; } = string.Empty;
        public ITransitionBuilder WithId(string id)
        {
            Id = id;

            return this;
        }

        public override string ToString()
        {
            return $"Id:{Id}, {SourceStateId} -> {TargetStateId}";
        }
    }
}

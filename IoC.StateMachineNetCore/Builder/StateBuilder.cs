using IoC.StateMachine.Abstractions;
using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IoC.StateMachine.Builder
{
    public class StateBuilder : IStateBuilder
    {
        private readonly Action<IState> _setup;
        private readonly IServiceProvider _serviceProvider;
        public StateBuilder(IServiceProvider serviceProvider, Action<IState> setup = null)
        {
            Affirm.ArgumentNotNull(serviceProvider, nameof(serviceProvider));

            _serviceProvider = serviceProvider;
            _setup = setup;
        }

        private bool EndPoint { get; set; } = false;
        private bool StartPoint { get; set; } = false;
        private string Id { get; set; } = string.Empty;

        IList<IActionBuilder> EnterActions = new List<IActionBuilder>();
        public IStateBuilder AddEnterAction(Action<IActionBuilder> actionBuilderAction)
        {
            Affirm.ArgumentNotNull(actionBuilderAction, nameof(actionBuilderAction));

            var actionBuilder = new ActionBuilder(_serviceProvider, this);

            actionBuilderAction.Invoke(actionBuilder);

            EnterActions.Add(actionBuilder);

            return this;
        }

        IList<IActionBuilder> ExitActions = new List<IActionBuilder>();
        public IStateBuilder AddExitAction(Action<IActionBuilder> actionBuilderAction)
        {
            Affirm.ArgumentNotNull(actionBuilderAction, nameof(actionBuilderAction));

            var actionBuilder = new ActionBuilder(_serviceProvider, this);

            actionBuilderAction.Invoke(actionBuilder);

            ExitActions.Add(actionBuilder);

            return this;
        }

        public override string ToString()
        {
            return $"Id:{Id}";
        }

        public IState Build()
        {
            var state = new State();

            state.EndPoint = this.EndPoint;
            state.StartPoint = this.StartPoint;
            state.id = this.Id;

            _setup?.Invoke(state);

            Affirm.With(string.IsNullOrEmpty(state.id), "Id must be defined for state!");
            Affirm.With(EndPoint && EndPoint, $"State with Id {state.id} is endpoint and startpoint in the same time!");

            state.ExitActions = ExitActions.Select(_ => _.Build()).ToList();
            state.EnterActions = EnterActions.Select(_ => _.Build()).ToList();           

            return state;
        }

        public IStateBuilder WithEndPoint()
        {
            EndPoint = true;

            return this;
        }

        public IStateBuilder WithStartPoint()
        {
            StartPoint = true;

            return this;
        }

        public IStateBuilder WithId(string id)
        {
            Id = id;

            return this;
        }
    }
}

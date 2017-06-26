using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoC.StateMachine.Interfaces;
using IoC.StateMachine;
using IoC.StateMachine.Core.Extension;

namespace IoC.StateMachine.Core
{   
    public class SMService : ISMService
    {
        static readonly ILog Log = LogManager.GetLog(typeof(SMService));

        private readonly IStateProcessor _stateProcessor;
        private readonly IPersistenceService _persistenceService;
        public SMService(IStateProcessor stateProcessor, IPersistenceService persistenceService)
        {
            Affirm.ArgumentNotNull(stateProcessor, "stateProcessor");
            Affirm.ArgumentNotNull(persistenceService, "persistenceService");

            _stateProcessor = stateProcessor;
            _persistenceService = persistenceService;
        }

        public IList<ITransition> GetTransitions(IStateMachine sm)
        {
            return sm.Definition.GetTransitions(sm.CurrentState);
        }

        private IStateMachine MoveToState(IStateMachine sm, IState state, ISMParameters parameters)
        {
            Affirm.ArgumentNotNull(sm, "sm");
            Affirm.ArgumentNotNull(state, "state");

            Log.Debug("{0} is going to be moved to state {1}".FormIt(sm, state.id));

            _stateProcessor.ProcessState(state, parameters);

            sm.SetCurrentState(state);           

            return sm;
        }

        #region Init

        private ITransition GetNextTransition(IStateMachine sm, ISMParameters parameters)
        {
            Affirm.ArgumentNotNull(sm, "sm");

            var possibleTransitions = sm.Definition.GetTransitions(sm.CurrentState);

            if (possibleTransitions == null)
                throw new InvalidOperationException("{0}: no transitions found {1}".FormIt(sm));

            var triggers = possibleTransitions.Select(_ => new { trigger = _.Trigger, tran = _ }).ToList();

            var theOne = triggers.Where(_ => _.trigger.Invoke(_.tran, parameters)).ToList();

            if (!theOne.Any())
                throw new InvalidOperationException("{0}: no true trigger result found for state {1}".FormIt(sm, sm.CurrentStateId));

            if (theOne.Count()>2)
                throw new InvalidOperationException("{0}: too many triggers are true {1}".FormIt(sm, sm.CurrentStateId));

            return theOne.First().tran;
        }
        #endregion

        #region Push

        public IStateMachine Push(IStateMachine sm, ISMParameters parameters)
        {
            Affirm.ArgumentNotNull(sm, "sm");

            Log.Debug("{0} going to be pushed".FormIt(sm));

            if (sm.Finished)
                throw new InvalidOperationException("{0} is finished already! can not push!".FormIt(sm));

            _stateProcessor.ExitState(sm.CurrentState, parameters);

            var nextState = GetNextTransition(sm, parameters).TargetStateId;

            var state = sm.Definition.GetStateById(nextState);

            if (state == null)
                throw new InvalidOperationException("{0}: State {1} not found!".FormIt(sm, nextState));

            return MoveToState(sm, state, parameters);
        }

        #endregion

        #region Start

		public IStateMachine Start(IStateMachineDefinition def, ISMParameters parameters, string key)
		{
            Affirm.ArgumentNotNull(key, "key");
			var sm = IoC.Get<IStateMachine>(key);
			return Start(def, parameters, sm); 
		}

		public IStateMachine Start(IStateMachineDefinition def, ISMParameters parameters, Type smType)
		{
            Affirm.ArgumentNotNull(smType, "smType");
			var sm = IoC.Get(smType) as IStateMachine;
            if (sm == null)
                throw new ArgumentNullException("Given type {0} is not assingable from IStateMachine".FormIt(smType));
			return Start(def, parameters, sm);
		}
		
		IStateMachine Start(IStateMachineDefinition def, ISMParameters paramters, IStateMachine sm)
        {
            Affirm.ArgumentNotNull(def, "definition");
            Affirm.ArgumentNotNull(sm, "machine");

            def.Validate();

            Log.Debug("SM with {0} is going to be started".FormIt(sm.GetType()));

            sm.SmId = Guid.NewGuid();
            _persistenceService.BuildUpDefinition(sm, def);

            var initialState = sm.Definition.GetInitialState();

            if (initialState == null)
                throw new InvalidOperationException("{0}: can not find intitial state".FormIt(sm));

            return MoveToState(sm, initialState, paramters);
        }

        public T Start<T>(ISMParameters parameters, IStateMachineDefinition definition) where T : class,IStateMachine
        {
            return Start(definition, parameters, typeof(T)) as T;
        }
        #endregion
      
    }

    public class StateProcessor : IStateProcessor
    {
        IEnumerable<IActionHolder> GetOrdered(IEnumerable<IActionHolder> source)
        {
            return source.OrderBy(_ => _.Order);
        }

        private void InvokeActionList(IState state, ISMParameters parameters, IEnumerable<IActionHolder> source)
        {
            Affirm.ArgumentNotNull(state, "state");

            if (source != null)
                foreach (var act in GetOrdered(source))
                    act.Invoke(parameters);

        }

        public void ProcessState(IState state, ISMParameters parameters)
        {
            Affirm.ArgumentNotNull(state, "state");

            if (state.EnterActions != null)
                InvokeActionList(state, parameters, state.EnterActions);

        }

        public void ExitState(IState state, ISMParameters parameters)
        {
            Affirm.ArgumentNotNull(state, "state");

            if (state.ExitActions != null)
                InvokeActionList(state, parameters, state.ExitActions);
        }
    }
}
    
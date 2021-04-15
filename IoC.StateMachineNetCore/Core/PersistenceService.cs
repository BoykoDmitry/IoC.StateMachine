using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using IoC.StateMachine.Core.Extension;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using IoC.StateMachine.Exceptions;
using IoC.StateMachine.Abstractions;
using Microsoft.Extensions.Logging;

namespace IoC.StateMachine.Core
{
    /// <summary>
    /// Base class for persistence 
    /// </summary>
    public abstract class BasePersistenceService : IPersistenceService
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly ILogger _logger;
        protected BasePersistenceService(IServiceProvider serviceProvider, ILogger logger )
        {
            Affirm.ArgumentNotNull(serviceProvider, nameof(serviceProvider));
            Affirm.ArgumentNotNull(logger, nameof(logger));

            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public abstract string To<T>(T obj);

        public abstract string To(IStateMachine sm);

        public abstract T FromSource<T>(string source) where T : class, IStateMachine;

        public abstract T ObjectFromSource<T>(string source) where T : class;

        public abstract object ObjectFromSource(string source, Type t);

        public abstract object FromSource(string source, Type type);

        public virtual void BuildUpDefinition(IStateMachine sm, IStateMachineDefinition def)
        {
            Affirm.ArgumentNotNull(sm, "sm");
            Affirm.ArgumentNotNull(def, "def");

            def.Validate();

            sm.CurrentState = def.GetStateById(sm.CurrentStateId);

            var childContainer = _serviceProvider.CreateScope();
            //childContainer.RegisterInstance(typeof(IStateMachine), sm);
            //childContainer.RegisterInstance(sm.GetType(), sm);
            sm.Container = childContainer;

            var actionFabric = sm.Container.ServiceProvider.GetRequiredService<IActionFabric>();
            var triggerFabric = sm.Container.ServiceProvider.GetRequiredService<ITriggerFabric>();

            Action<IEnumerable<IActionHolder>> buildUpActions = (s) =>
             {
                 foreach (var act in s)
                 {
                     act.NestedAction = actionFabric.Get(act.Code);
                     if (act.NestedAction == null)
                         throw new StateMachineException(sm.SmId, $"Can not resolve {act.Code} from ation fabric");
                     act.NestedAction.StateMachine = sm;
                 }
             };

            
            foreach (var state in def.States)
            {
                state.StateMachine = sm;

                if (state.EnterActions != null)
                    buildUpActions(state.EnterActions);

                if (state.ExitActions != null)
                    buildUpActions(state.ExitActions);
            }

            foreach (var tran in def.Transitions)
            {
                tran.StateMachine = sm;

                if (tran.Trigger != null
                    && !string.IsNullOrEmpty(tran.Trigger.Code))
                {
                    var trigger = triggerFabric.Get(tran.Trigger.Code);

                    if (trigger == null)
                        throw new StateMachineException(sm.SmId, $"Can not resolve {tran.Trigger.Code} from trigger fabric");

                    trigger.StateMachine = sm;
                    tran.Trigger.NestedAction = trigger;
                }
            }

            sm.SetDefinition(def);
        }

        public T Load<T>(string xml, IStateMachineDefinition def) where T : class, IStateMachine
        {
            Affirm.NotNullOrEmpty(xml, "xml");
            Affirm.ArgumentNotNull(def, "def");

            var sm = ObjectFromSource<T>(xml);

            BuildUpDefinition(sm, def);

            return sm;
        }
    }
}

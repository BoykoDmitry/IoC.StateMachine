using IoC.StateMachine.Interfaces;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using IoC.StateMachine.Core.Extension;
using System.Collections.Generic;

namespace IoC.StateMachine.Core
{
    /// <summary>
    /// Base class for persistence 
    /// </summary>
    public abstract class BasePersistenceService : IPersistenceService
    {
        public BasePersistenceService()
        {
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

            var childContainer = IoC.CreateChildContainer();
            childContainer.RegisterInstance(typeof(IStateMachine), sm);
            childContainer.RegisterInstance(sm.GetType(), sm);

            Action<IAmContainer, IEnumerable<IActionHolder>> buildUpActions = (c, s) =>
             {
                 foreach (var act in s)
                 {
                     c.BuildUp(act);
                     act.NestedAction = c.Get<ISMAction>(act.Code);
                 }
             };

            childContainer.BuildUp(sm);
            
            foreach (var state in def.States)
            {
                state.StateMachine = sm;
                childContainer.BuildUp(state);

                if (state.EnterActions != null)
                    buildUpActions(childContainer, state.EnterActions);

                if (state.ExitActions != null)
                    buildUpActions(childContainer, state.ExitActions);
            }

            foreach (var tran in def.Transitions)
            {
                tran.StateMachine = sm;             
                childContainer.BuildUp(tran);

                if (tran.Trigger != null
                    && !string.IsNullOrEmpty(tran.Trigger.Code))
                    tran.Trigger.NestedAction = childContainer.Get<ISMTrigger>(tran.Trigger.Code);
            }

            sm.SetDefinition(def);
        }

        public T Load<T>(string xml, string definition) where T : class, IStateMachine
        {
            var def = ObjectFromSource<StateMachineDefinition>(definition);
            var sm = ObjectFromSource<T>(xml);

            BuildUpDefinition(sm, def);

            return sm;
        }
    }
}

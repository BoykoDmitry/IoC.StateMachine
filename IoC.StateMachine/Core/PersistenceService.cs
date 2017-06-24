using IoC.StateMachine.Interfaces;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using IoC.StateMachine.Core.Extension;

namespace IoC.StateMachine.Core
{
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

            childContainer.BuildUp(sm);
            
            foreach (var state in def.States)
            {
                state.StateMachine = sm;
                childContainer.BuildUp(state);
                
                if (state.EnterActions != null)
                    foreach (var act in state.EnterActions)
                    {
                        childContainer.BuildUp(act);
                        act.NestedAction = childContainer.Get<ISMAction>(act.Code);
                    }
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

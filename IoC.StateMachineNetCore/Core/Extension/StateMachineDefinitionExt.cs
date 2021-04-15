using IoC.StateMachine.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Core.Extension
{
    /// <summary>
    /// Various helper extensions
    /// </summary>
    public static class StateMachineDefinitionExt
    {
        public static IState GetStateById(this IStateMachineDefinition def, string id)
        {
            if (def.States == null)
                return null;

            return def.States.FirstOrDefault(_ => _.id == id);
        }

        public static void CheckState(this IStateMachineDefinition def,IState state)
        {
            Affirm.ArgumentNotNull(state, "state");

            def.CheckState(state.id);
        }

        public static void CheckState(this IStateMachineDefinition def, string stateId)
        {
            if (def.GetStateById(stateId) == null)
                throw new ArgumentException("StateMachine: State was not found!");
        }

        public static IState GetInitialState(this IStateMachineDefinition def)
        {
            return def.States.FirstOrDefault(_ => _.StartPoint);
        }

        public static IList<ITransition> GetTransitions(this IStateMachineDefinition def, IState state)
        {
            Affirm.ArgumentNotNull(state, "state");

            if (def.Transitions == null)
                return null;
             
            return GetTransitions(def, state.id);
        }

        public static IList<ITransition> GetTransitions(this IStateMachineDefinition def, string stateId)
        {
            Affirm.NotNullOrEmpty(stateId, "stateId");

            return def.Transitions.Where(_ => _.SourceStateId == stateId).ToList();
        }

    }
}

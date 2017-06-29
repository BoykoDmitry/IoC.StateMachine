using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Core.Extension
{
    /// <summary>
    /// Extensions for state machine definition
    /// </summary>
    public static class StateMachineBuilder
    {
        /// <summary>
        /// Creates or returns transition 
        /// </summary>
        /// <param name="def">Definition</param>
        /// <param name="tranId">id of transition</param>
        /// <param name="stateFrom">state from</param>
        /// <param name="stateTo">state to</param>
        /// <returns><see cref="ITransition"/></returns>
        public static ITransition GetOrCreateTran(this IStateMachineDefinition def, string tranId, string stateFrom, string stateTo)
        {
            var tran = def.Transitions.FirstOrDefault(_ => _.SourceStateId == stateFrom && _.TargetStateId == stateTo);

            if (tran == null)
            {
                tran = new Transition() { SourceStateId = stateFrom, TargetStateId = stateTo, id = tranId };
                def.Transitions.Add(tran);
            }

            return tran;

        }

        /// <summary>
        /// Executes action on instance of <see cref="ISMBaseElement"/>
        /// </summary>
        public static T Setup<T>(this T t, Action<T> action) where T: ISMBaseElement
        {
            Affirm.ArgumentNotNull(action, "action");

            action(t);

            return t;
        }

        /// <summary>
        /// Creates or returns state 
        /// </summary>
        /// <param name="def">Definition</param>
        /// <param name="id">id of state machine</param>
        /// <param name="action">action to be executed on given state</param>
        /// <returns></returns>
        public static IState GetOrCreateState(this IStateMachineDefinition def, string id, Action<IState> action = null)
        {
            var state = def.GetStateById(id);
            if (state == null)
            {
                state = new State() { id = id };
                def.States.Add(state);
            }

            if (action != null)
                action(state);

            return state;
        }

        private static IActionHolder GetAction(IList<IActionHolder> source, string code, Action<IActionHolder> action = null)
        {
            var act = source.FirstOrDefault(_ => _.Code == code);

            if (act == null)
            {
                act = new SMActionHolder() { Code = code };
                source.Add(act);
            }

            if (action != null)
                action(act);

            return act;
        }

        /// <summary>
        /// Gets action from the state
        /// </summary>
        /// <param name="def">Definition</param>
        /// <param name="code">Action key</param>
        /// <returns></returns>
        public static IActionHolder Action(this IState def, string code, Action<IActionHolder> action = null)
        {
            return GetAction(def.EnterActions, code, action);
        }

        /// <summary>
        /// Gets exit action from the state
        /// </summary>
        /// <param name="def">Definition</param>
        /// <param name="code">Action key</param>
        /// <returns></returns>
        public static IActionHolder ExitAction(this IState def, string code, Action<IActionHolder> action = null)
        {
            return GetAction(def.ExitActions, code, action);
        }

        /// <summary>
        /// Adds parameter for action holder 
        /// </summary>
        /// <returns></returns>
        public static IHolderBase SetParameter<T>(this IHolderBase act, string key, T value)
        {
            act.Parameters.Add(key, value);

            return act;
        }

        /// <summary>
        /// Gets or creates trigger from the transition
        /// </summary>
        /// <param name="def">transition</param>
        /// <param name="code">trigger key</param>
        /// <returns></returns>
        public static ITriggerHolder Trigger(this ITransition def, string code, Action<ITriggerHolder> action = null)
        {
            var act = def.Trigger ?? new SMATriggerHolder() { Code = code };

            if (action != null)
                action(act);

            def.Trigger = act;

            return act;
        }      

        public static object GetValue(this ISMParameter p)
        {
            if (p == null)
                return null;

            return p.Value;
        }
    }
}

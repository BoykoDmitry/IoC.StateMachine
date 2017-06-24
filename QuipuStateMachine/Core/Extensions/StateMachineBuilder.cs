using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Core.Extension
{
    public static class StateMachineBuilder
    {
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

        public static IActionHolder Action(this IState def, string code, Action<IActionHolder> action = null)
        {
            var act = def.EnterActions.FirstOrDefault(_ => _.Code == code);

            if (act == null)
            {
                act = new SMActionHolder() { Code = code };
                def.EnterActions.Add(act);
            }

            if (action != null)
                action(act);

            return act;
        }

        public static IActionHolder SetParameter<T>(this IActionHolder act, string key, T value)
        {
            act.Parameters.Add(key, value);

            return act;
        }

        public static ITriggerHolder Trigger(this ITransition def, string code, Action<ITriggerHolder> action = null)
        {
            var act = def.Trigger ?? new SMATriggerHolder() { Code = code };

            if (action != null)
                action(act);

            def.Trigger = act;

            return act;
        }

        public static ITriggerHolder SetParameter<T>(this ITriggerHolder act, string key, T value)
        {
            act.Parameters.Add(key, value);

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

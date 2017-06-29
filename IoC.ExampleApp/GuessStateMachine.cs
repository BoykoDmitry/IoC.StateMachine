using IoC.StateMachine.Core;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoC.StateMachine.Core.Extension;
using System.Runtime.Serialization;

namespace IoC.ExampleApp
{
    public enum GuessResult {
        Correct,
        More,
        Less,
        NA
    };

    [DataContract]
    public class GuessContext : ContextBase
    {
        public GuessContext()
        {
            Number = -1;
            Result = GuessResult.NA;
        }
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public GuessResult Result { get; set; }
    }

    public abstract class BaseGuessAction
    {
        protected readonly GuessStateMachine _sm;
        public BaseGuessAction(GuessStateMachine sm)
        {
            _sm = sm;
        }    
    }

    public class InitContext : BaseGuessAction, ISMAction
    {
        private Random rnd = new Random();
        public InitContext(GuessStateMachine sm) : base(sm)
        {
        }

        public void Invoke(ISMParameters Parameters, ISMParameters TransitionParameters)
        {
            _sm.Context = new GuessContext();
            
            var maxValue = Parameters.GetParameter<int>("MaxNumber");

            _sm.Context.Number = rnd.Next(maxValue);
        }
    }

    public class CheckNumber : BaseGuessAction, ISMAction
    {
        public CheckNumber(GuessStateMachine sm) : base(sm)
        {
        }

        public void Invoke(ISMParameters Parameters, ISMParameters TransitionParameters)
        {
            if (TransitionParameters != null)
            {
                int EnteredNumber = TransitionParameters.GetParameter<int>("EnteredNumber");

                if (EnteredNumber > _sm.Context.Number)
                    _sm.Context.Result = GuessResult.More;
                else if (EnteredNumber < _sm.Context.Number)
                    _sm.Context.Result = GuessResult.Less;
                else
                    _sm.Context.Result = GuessResult.Correct;
            }
        }
    }

    public class GuessOKTrigger : BaseGuessAction, ISMTrigger
    {
        public GuessOKTrigger(GuessStateMachine sm) : base(sm)
        {
        }

        public bool Invoke(ITransition transition, ISMParameters Parameters, ISMParameters TransitionParameters)
        {          
            return (GuessResult.Correct == _sm.Context.Result);
        }
    }

    [DataContract]
    public class GuessStateMachine : StateMachineBase<GuessContext>, IStateMachine
    {
        public static IStateMachineDefinition GetDefinition()
        {
            var def = new StateMachineDefinition();

            def.GetOrCreateState("New")
                .Setup(_=>_.StartPoint = true)
                .Action("InitContext")
                .SetParameter<int>("MaxNumber", 20);

            def.GetOrCreateState("Guess")
                .ExitAction("CheckNumber");

            def.GetOrCreateState("Done")
                .Setup(_ => _.EndPoint = true);

            //def.GetOrCreateTran("NewToGuess", "New", "Guess")
            //    .Trigger("GuessOKTrigger")
            //    .Setup(_ => _.Inverted = true);

            def.GetOrCreateTran("GuessToDone", "Guess", "Done")
                .Trigger("GuessOKTrigger");

            //def.GetOrCreateTran("GuessToGues1", "Guess", "Guess")
            //    .Trigger("GuessOKTrigger")
            //    .Setup(_ => _.Inverted = true);


            return def;

        }
    }
}

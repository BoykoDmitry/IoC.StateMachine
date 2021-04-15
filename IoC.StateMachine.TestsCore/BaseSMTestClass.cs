using IoC.StateMachine.Core;
using IoC.StateMachine.Abstractions;
using Lamar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using IoC.StateMachine.Core.Extension;
using Microsoft.Extensions.DependencyInjection;
using IoC.StateMachine.Builder;

namespace IoC.StateMachine.Tests
{
    public abstract class BaseSMTestClass
    {
        protected IServiceProvider _container;
        protected StateMachineDefinition Definition;

        protected virtual void SetUpDefinition(IServiceProvider serviceProvider)
        {
            var builder = new StateMachineDefinitionBuilder(serviceProvider);

            builder.AddStartState("state1")
                   .AddEnterAction(_ => _.WithCode("TestInitAction"));

            builder.AddState("state2")
                   .AddEnterAction(_ => _.WithCode("TestActionSetPropTo2"));

            builder.AddState("state2_1");

            builder.AddEndState("state3")
                   .AddEnterAction(_ =>
                   {
                       _.WithCode("TestAction")
                        .WithOrder(0)
                        .WithParameter("int", "10");
                   });

            builder.AddTransition("tran1", "state1", "state2") //
                   .SetTrigger(_ =>
                   {
                       _.WithCode("TestTrigger")
                        .WithParameter("TargetState", "state2");
                   });

            builder.AddTransition("tran2", "state2", "state3") //
                   .SetTrigger(_ =>
                   {
                       _.WithCode("TestTrigger")
                        .WithParameter("TargetState", "state3");
                   });

            builder.AddTransition("tran2_1", "state2", "state2_1") //
                   .SetTrigger(_ =>
                   {
                       _.WithCode("TestTrigger")
                        .WithParameter("TargetState", "state2_1");
                   });

            builder.AddTransition("tran2_11", "state2_1", "state3") //
                   .SetTrigger(_ =>
                   {
                       _.WithCode("TestTrigger")
                        .WithParameter("TargetState", "state3");
                   });

            builder.AddTransition("tran3", "state3", "state1") //
                   .SetTrigger(_ =>
                   {
                       _.WithCode("TestTrigger")
                        .WithParameter("TargetState", "state1");
                   });

            Definition = builder.Build();
        }

        public BaseSMTestClass()
        {
            //Definition = new StateMachineDefinition();
            //Definition.GetOrCreateState("state1")
            //          .Setup(_ => _.StartPoint = true)
            //          .Action("TestInitAction");

            //Definition.GetOrCreateState("state2")
            //          .Action("TestActionSetPropTo2");

            //Definition.GetOrCreateState("state2_1");

            //Definition.GetOrCreateState("state3")
            //          .Setup(_ => _.EndPoint = true)
            //          .Action("TestAction")
            //          .Setup(_ => _.Order = 0)
            //          .SetParameter<string>("int", "10");

            //Definition.GetOrCreateTran("tran1", "state1", "state2") //
            //          .Trigger("TestTrigger")
            //          .SetParameter<string>("TargetState", "state2");

            //Definition.GetOrCreateTran("tran2", "state2", "state3") //
            //          .Trigger("TestTrigger")
            //          .SetParameter<string>("TargetState", "state3");

            //Definition.GetOrCreateTran("tran2_1", "state2", "state2_1") //
            //          .Trigger("TestTrigger")
            //          .SetParameter<string>("TargetState", "state2_1");

            //Definition.GetOrCreateTran("tran2_11", "state2_1", "state3") //
            //          .Trigger("TestTrigger")
            //          .SetParameter<string>("TargetState", "state3");

            //Definition.GetOrCreateTran("tran3", "state3", "state1")
            //          .Trigger("TestTrigger")
            //          .SetParameter<string>("TargetState", "state1");

        }

      

    }
}

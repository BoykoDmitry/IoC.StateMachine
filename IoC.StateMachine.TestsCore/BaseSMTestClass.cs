using IoC.StateMachine.Core;
using IoC.StateMachine.Interfaces;
using Lamar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using IoC.StateMachine.Core.Extension;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.StateMachine.Tests
{
    public class Factory<T> : IFabric<T>
    {
        private readonly IServiceContext _serviceProvider;
        public Factory(IServiceContext serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public T Get(string key)
        {
            return _serviceProvider.GetInstance<T>(key);
        }
    }

    public class SMFactory : Factory<IStateMachine>, ISMFactory
    {
        public SMFactory(IServiceContext serviceProvider) : base(serviceProvider)
        {
        }
    }

    public class ActionFabric : Factory<ISMAction>, IActionFabric
    {
        public ActionFabric(IServiceContext serviceProvider) : base(serviceProvider)
        {
        }
    }

    public class TriggerFabric : Factory<ISMTrigger>, ITriggerFabric
    {
        public TriggerFabric(IServiceContext serviceProvider) : base(serviceProvider)
        {
        }
    }

    public abstract class BaseSMTestClass
    {
        protected IServiceProvider _container;
        protected StateMachineDefinition Definition;

        public BaseSMTestClass()
        {
            Definition = new StateMachineDefinition();
            Definition.GetOrCreateState("state1")
                      .Setup(_ => _.StartPoint = true)
                      .Action("TestInitAction");

            Definition.GetOrCreateState("state2")
                      .Action("TestActionSetPropTo2");

            Definition.GetOrCreateState("state2_1");

            Definition.GetOrCreateState("state3")
                      .Setup(_ => _.EndPoint = true)
                      .Action("TestAction")
                      .Setup(_ => _.Order = 0)
                      .SetParameter<string>("int", "10");

            Definition.GetOrCreateTran("tran1", "state1", "state2")
                      .Trigger("TestTrigger")
                      .SetParameter<string>("TargetState", "state2");

            Definition.GetOrCreateTran("tran2", "state2", "state3")
                      .Trigger("TestTrigger")
                      .SetParameter<string>("TargetState", "state3");

            Definition.GetOrCreateTran("tran2_1", "state2", "state2_1")
                      .Trigger("TestTrigger")
                      .SetParameter<string>("TargetState", "state2_1");

            Definition.GetOrCreateTran("tran2_11", "state2_1", "state3")
                      .Trigger("TestTrigger")
                      .SetParameter<string>("TargetState", "state3");

            Definition.GetOrCreateTran("tran3", "state3", "state1")
                      .Trigger("TestTrigger")
                      .SetParameter<string>("TargetState", "state1");
        }

      

    }
}

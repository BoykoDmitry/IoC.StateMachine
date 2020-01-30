
using IoC.StateMachine.Core;
using IoC.StateMachine.Interfaces;
using IoC.StateMachine.Serialization;
using Lamar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoC.StateMachine.Core.Extension;
using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace IoC.StateMachine.Tests
{
    [TestClass]
    public class ExceptionsTests
    {        
        IServiceProvider _container;
        private StateMachineDefinition Definition;

        [TestInitialize]
        public void TestInitialize()
        {
            var container = new ServiceRegistry();//
            container.For<ISMAction>().Use<TestAction>().Named("TestAction").Scoped();
            container.For<ISMAction>().Use<TestInitAction>().Named("TestInitAction").Scoped();
            container.For<ISMTrigger>().Use<TestTrigger>().Named("TestTrigger").Scoped();

            container.For<IStateProcessor>().Use<StateProcessor>();
            container.For<IPersistenceService>().Use(s => new DataContractPersistenceService(new string[] { "IoC.StateMachine" }, s));

            container.For<ISMFactory>().Use<SMFactory>().Singleton();
            container.For<IActionFabric>().Use<ActionFabric>().Singleton();
            container.For<ITriggerFabric>().Use<TriggerFabric>().Singleton();

            container.For<ISMService>().Use<SMService>();

            _container = new Container(container).ServiceProvider;
        }

        public ExceptionsTests()
        {
            Definition = new StateMachineDefinition();
            Definition.GetOrCreateState("s1")
                      .Setup(_ => _.StartPoint = true);

            Definition.GetOrCreateState("s2")
                      .Setup(_ => _.EndPoint = true);

            var tran1 = new Transition() { id = "t1", SourceStateId = "s1", TargetStateId = "s2" };

            Definition.Transitions.Add(tran1);

            tran1
           .Trigger("TestTrigger")
           .SetParameter<string>("TargetState", "s2");

            var tran2 = new Transition() { id = "t2", SourceStateId = "s1", TargetStateId = "s2" };

            Definition.Transitions.Add(tran2);

            tran2
           .Trigger("TestTrigger")
           .SetParameter<string>("TargetState", "s2");
        }

        [TestMethod]
        [ExpectedException(typeof(TooManyTriggersException))]
        public void TooManyTriggersTest()
        {
            var service = _container.GetService<ISMService>();

            var sm = service.Start<StateMachine4Test>(null, Definition);

            var paramPush = new SMParametersCollection();
            paramPush.Add("TargetState", "s2");

            service.Push(sm, paramPush);
        }

        [TestMethod]
        [ExpectedException(typeof(NoTrueTriggerException))]
        public void NoTrueTriggerTest()
        {
            var service = _container.GetService<ISMService>();

            var sm = service.Start<StateMachine4Test>(null, Definition);

            var paramPush = new SMParametersCollection();
            paramPush.Add("TargetState", "s200");

            service.Push(sm, paramPush);
        }
    }


}

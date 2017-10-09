using IoC.StateMachine.Core;
using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Core.Extension;
using IoC.StateMachine.Exceptions;
using IoC.StateMachine.Interfaces;
using IoC.StateMachine.Serialization;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Tests
{
    [TestClass]
    public class ExceptionsTests
    {
        IAmContainer _container;
        private StateMachineDefinition Definition;

        [TestInitialize]
        public void TestInitialize()
        {
            var container = new UnityContainer();
            container.RegisterType<ISMAction, TestAction>("TestAction");
            container.RegisterType<ISMAction, TestInitAction>("TestInitAction");
            container.RegisterType<ISMTrigger, TestTrigger>("TestTrigger");
            container.RegisterType<IStateProcessor, StateProcessor>();
            container.RegisterInstance<IPersistenceService>(new DataContractPersistenceService(new string[] { "IoC.StateMachine" }));
            container.RegisterType<ISMService, SMService>();

            _container = new Unity4StateMachine(container);

            IoC.SetContainer(_container);
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
            var service = _container.Get<ISMService>();

            var sm = service.Start<StateMachine4Test>(null, Definition);

            var paramPush = new SMParametersCollection();
            paramPush.Add("TargetState", "s2");

            service.Push(sm, paramPush);
        }

        [TestMethod]
        [ExpectedException(typeof(NoTrueTriggerException))]
        public void NoTrueTriggerTest()
        {
            var service = _container.Get<ISMService>();

            var sm = service.Start<StateMachine4Test>(null, Definition);

            var paramPush = new SMParametersCollection();
            paramPush.Add("TargetState", "s200");

            service.Push(sm, paramPush);
        }
    }


}

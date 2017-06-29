using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using IoC.StateMachine.Interfaces;
using IoC.StateMachine.Core;
using IoC.StateMachine.Serialization;
using IoC.StateMachine.Core.Extension;
using IoC.StateMachine.Core.Classes;
using System.Linq;

namespace IoC.StateMachine.Tests
{
    [TestClass]
    public class StateMachineTest
    {
        IAmContainer _container;
        private StateMachineDefinition Definition;

        [TestInitialize]
        public void TestInitialize()
        {
            var container = new UnityContainer();

            container.RegisterType<ISMAction, TestAction>("TestAction");
            container.RegisterType<ISMAction, TestInitAction>("TestInitAction");
            container.RegisterType<ISMAction, TestActionSetPropTo2>("TestActionSetPropTo2");
            container.RegisterType<IStateProcessor, StateProcessor>();
            container.RegisterInstance<IPersistenceService>(new DataContractPersistenceService(new string[] { "IoC.StateMachine" }));
            container.RegisterType<ISMTrigger, TestTrigger>("TestTrigger");
            container.RegisterType<ISMService, SMService>();

            _container = new Unity4StateMachine(container);

            IoC.SetContainer(_container);
        }

        public StateMachineTest()
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

        [TestMethod]
        public void StartTest()
        {
            var service = _container.Get<ISMService>();

            var param = new SMParametersCollection();
            param.Add("int", "5");
            param.Add("str", "str");

            var sm = service.Start<StateMachine4Test>(param, Definition);

            Assert.IsNotNull(sm);
            Assert.AreEqual(sm.Context.TestPropInt, 5);
            Assert.AreEqual(sm.Context.TestPropStr, "str");

            var paramPush = new SMParametersCollection();
            paramPush.Add("TargetState", "state2");

            service.Push(sm, paramPush);

            Assert.IsTrue(sm.CurrentState.id == "state2");

            var paramPush2 = new SMParametersCollection();
            paramPush2.Add("TargetState", "state3");

            service.Push(sm, paramPush2);

            Assert.IsTrue(sm.CurrentState.id == "state3");
            Assert.AreEqual(sm.Context.TestPropInt, 15);            
        }

        [TestMethod]
        public void InitTest()
        {
            var service = _container.Get<IPersistenceService>();

            var sm = new StateMachine4Test();

            service.BuildUpDefinition(sm, Definition);

            var builtSM = sm.Definition.States.Select(_ => _.StateMachine).FirstOrDefault();

            Assert.IsNotNull(builtSM);
            Assert.IsInstanceOfType(builtSM, typeof(StateMachine4Test));
        }

        [TestMethod]
        public void FromXmlTest()
        {
            var service = _container.Get<ISMService>();
            var xmlService = _container.Get<IPersistenceService>();

            var sm = xmlService.FromSource<StateMachine4Test>(StateMachine4Test.xml4test);

            Assert.IsNotNull(sm);
            Assert.AreEqual(Guid.Parse("e7fc4673-0a9d-4d8e-8c66-1fa1f3eca3d0"), sm.SmId);
            Assert.IsInstanceOfType(sm, typeof(StateMachine4Test));
            Assert.IsTrue(sm.CurrentStateId == "state3");
        }

        [TestMethod]
        public void ToXmlTest()
        {
            var service = _container.Get<ISMService>();
            var xmlService = _container.Get<IPersistenceService>();

            var sm = new StateMachine4Test();
            sm.SetDefinition(Definition);
            var s = xmlService.To(sm);
        }

        [TestMethod]
        public void DefinitionToXmlTest()
        {
            var xmlService = _container.Get<IPersistenceService>();

            var defXml = xmlService.To<StateMachineDefinition>(Definition);

            Assert.IsNotNull(defXml);
        }
    }
}

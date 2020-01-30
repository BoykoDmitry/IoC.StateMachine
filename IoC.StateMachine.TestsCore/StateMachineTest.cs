using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IoC.StateMachine.Interfaces;
using IoC.StateMachine.Core;
using IoC.StateMachine.Serialization;
using IoC.StateMachine.Core.Extension;
using IoC.StateMachine.Core.Classes;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Lamar;

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

    [TestClass]
    public class StateMachineTest
    {
        IServiceProvider _container;
        private StateMachineDefinition Definition;

        [TestInitialize]
        public void TestInitialize()
        {
            var container = new ServiceRegistry();

            container.For<ISMAction>().Use<TestAction>().Named("TestAction").Scoped();
            container.For<ISMAction>().Use<TestInitAction>().Named("TestInitAction").Scoped();
            container.For<ISMTrigger>().Use<TestTrigger>().Named("TestTrigger").Scoped();
            container.For<ISMAction>().Use<TestActionSetPropTo2>().Named("TestActionSetPropTo2").Scoped();

            container.For<ISMFactory>().Use<SMFactory>().Singleton();
            container.For<IActionFabric>().Use<ActionFabric>().Singleton();
            container.For<ITriggerFabric>().Use<TriggerFabric>().Singleton();

            container.For<IStateProcessor>().Use<StateProcessor>().Transient();
            container.For<IPersistenceService>().Use(s => new DataContractPersistenceService(new string[] { "IoC.StateMachine" }, s)).Singleton();

            container.For<ISMService>().Use<SMService>().Singleton();

            _container = new Container(container).ServiceProvider;            
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
            var service = _container.GetService<ISMService>();

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
            var service = _container.GetService<IPersistenceService>();

            var sm = new StateMachine4Test();

            service.BuildUpDefinition(sm, Definition);

            var builtSM = sm.Definition.States.Select(_ => _.StateMachine).FirstOrDefault();

            Assert.IsNotNull(builtSM);
            Assert.IsInstanceOfType(builtSM, typeof(StateMachine4Test));
        }

        [TestMethod]
        public void FromXmlTest()
        {
            var service = _container.GetService<ISMService>();
            var xmlService = _container.GetService<IPersistenceService>();

            var sm = xmlService.FromSource<StateMachine4Test>(StateMachine4Test.xml4test);

            Assert.IsNotNull(sm);
            Assert.AreEqual(Guid.Parse("e7fc4673-0a9d-4d8e-8c66-1fa1f3eca3d0"), sm.SmId);
            Assert.IsInstanceOfType(sm, typeof(StateMachine4Test));
            Assert.IsTrue(sm.CurrentStateId == "state3");
        }

        [TestMethod]
        public void ToXmlTest()
        {
            var service = _container.GetService<ISMService>();
            var xmlService = _container.GetService<IPersistenceService>();

            var sm = new StateMachine4Test();
            sm.SetDefinition(Definition);
            var s = xmlService.To(sm);
        }

        [TestMethod]
        public void SMLoadTest()
        {
            var service = _container.GetService<ISMService>();
            var xmlService = _container.GetService<IPersistenceService>();

            var strDefinition = xmlService.To<StateMachineDefinition>(Definition);
            var sm = xmlService.Load<StateMachine4Test>(StateMachine4Test.xml4test, strDefinition);

            Assert.IsTrue(sm.Definition.Transitions.All(_ => _.Trigger != null && _.Trigger.NestedAction != null && _.Trigger.NestedAction.StateMachine != null), "something wrong with triggers after deserilization");
            Assert.IsTrue(sm.Definition.States.SelectMany(_ => _.EnterActions).All(_ => _.NestedAction != null && _.NestedAction.StateMachine != null), "something wrong with enter actions after deserilization");
            Assert.IsTrue(sm.Definition.States.SelectMany(_ => _.ExitActions).All(_ => _.NestedAction != null && _.NestedAction.StateMachine != null), "something wrong with exit actions after deserilization");
        }

        [TestMethod]
        public void DefinitionToXmlTest()
        {
            var xmlService = _container.GetService<IPersistenceService>();

            var defXml = xmlService.To<StateMachineDefinition>(Definition);

            Assert.IsNotNull(defXml);
        }
    }
}

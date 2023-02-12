using System;
using System.Collections.Generic;
using System.Text;
using Lamar;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IoC.StateMachine.Core.Extension;
using Microsoft.Extensions.DependencyInjection;
using IoC.StateMachine.Core;
using System.Linq;
using IoC.StateMachine.Abstractions;
using IoC.StateMachine.Lamar;

namespace IoC.StateMachine.Tests
{
    [TestClass]
    public class XmlTests : StateMachineTest
    {
        [TestInitialize]
        public override void TestInitialize()
        {
            var container = new ServiceRegistry();
            container.AddLogging();
            container.AddSMCore(c =>
            {
                c.Services.AddLamarFactories();
            });

            container.For<ISMAction>().Use<TestAction>().Named("TestAction").Scoped();
            container.For<ISMAction>().Use<TestInitAction>().Named("TestInitAction").Scoped();
            container.For<ISMTrigger>().Use<TestTrigger>().Named("TestTrigger").Scoped();
            container.For<ISMAction>().Use<TestActionSetPropTo2>().Named("TestActionSetPropTo2").Scoped();

            _container = new Container(container).ServiceProvider;

            SetUpDefinition(_container);
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

            var strDefinition = xmlService.To(Definition);
            var sm = xmlService.Load<StateMachine4Test>(StateMachine4Test.xml4test, xmlService.ObjectFromSource<StateMachineDefinition>(strDefinition));

            Assert.IsTrue(sm.Definition.Transitions.All(_ => _.Trigger != null && _.Trigger.NestedAction != null && _.Trigger.NestedAction.StateMachine != null), "something wrong with triggers after deserilization");
            Assert.IsTrue(sm.Definition.States.SelectMany(_ => _.EnterActions).All(_ => _.NestedAction != null && _.NestedAction.StateMachine != null), "something wrong with enter actions after deserilization");
            Assert.IsTrue(sm.Definition.States.SelectMany(_ => _.ExitActions).All(_ => _.NestedAction != null && _.NestedAction.StateMachine != null), "something wrong with exit actions after deserilization");
        }

        [TestMethod]
        public void SMLoadTest2()
        {
            var service = _container.GetService<ISMService>();
            var xmlService = _container.GetService<IPersistenceService>();

            var strDefinition = xmlService.To(Definition);
            var sm = xmlService.Load(StateMachine4Test.xml4test, xmlService.ObjectFromSource<StateMachineDefinition>(strDefinition), typeof(StateMachine4Test)) as StateMachine4Test;

            Assert.IsTrue(sm.Definition.Transitions.All(_ => _.Trigger != null && _.Trigger.NestedAction != null && _.Trigger.NestedAction.StateMachine != null), "something wrong with triggers after deserilization");
            Assert.IsTrue(sm.Definition.States.SelectMany(_ => _.EnterActions).All(_ => _.NestedAction != null && _.NestedAction.StateMachine != null), "something wrong with enter actions after deserilization");
            Assert.IsTrue(sm.Definition.States.SelectMany(_ => _.ExitActions).All(_ => _.NestedAction != null && _.NestedAction.StateMachine != null), "something wrong with exit actions after deserilization");
        }
    }
}

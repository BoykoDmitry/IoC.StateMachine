﻿using System;
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
    [TestClass]
    public class StateMachineTest : BaseSMTestClass
    {
        [TestInitialize]
        public virtual void TestInitialize()
        {
            var container = new ServiceRegistry();

            container.AddSMCore(c =>
            {
                c.Services.AddSingleton<ISMFactory, SMFactory>()
                          .AddSingleton<IActionFabric, ActionFabric>()
                          .AddSingleton<ITriggerFabric, TriggerFabric>();
            });

            container.For<ISMAction>().Use<TestAction>().Named("TestAction").Scoped();
            container.For<ISMAction>().Use<TestInitAction>().Named("TestInitAction").Scoped();
            container.For<ISMTrigger>().Use<TestTrigger>().Named("TestTrigger").Scoped();
            container.For<ISMAction>().Use<TestActionSetPropTo2>().Named("TestActionSetPropTo2").Scoped();

            _container = new Container(container).ServiceProvider;
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
        public void DefinitionToXmlTest()
        {
            var xmlService = _container.GetService<IPersistenceService>();

            var defXml = xmlService.To<StateMachineDefinition>(Definition);

            Assert.IsNotNull(defXml);
        }
    }
}

using IoC.StateMachine.Core;
using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Interfaces;
using IoC.StateMachine.Serialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.ExampleApp
{
    //            _container.RegisterType<ISMService, SMService>();
    //            _container.RegisterInstance<IPersistenceService>(new DataContractPersistenceService(new List<string>() { "IoC.ExampleApp" }));
    //            _container.RegisterType<IStateProcessor, StateProcessor>();
    //            _container.RegisterType<ISMAction, InitContext>("InitContext");
    //            _container.RegisterType<ISMAction, CheckNumber>("CheckNumber");
    //            _container.RegisterType<ISMTrigger, GuessOKTrigger>("GuessOKTrigger");

    /// <summary>
    /// Example application, user should guess randomly generated integer value
    /// </summary>
    class Program
    {
        private static IServiceProvider _container;
        static void Main(string[] args)
        {

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<ISMService, SMService>();
            serviceCollection.AddSingleton<IPersistenceService>(_ => new DataContractPersistenceService(new List<string>() { "IoC.ExampleApp" }, _));
            serviceCollection.AddTransient<IStateProcessor, StateProcessor>();
            serviceCollection.AddSingleton<ISMFactory, SMFactory>();
            serviceCollection.AddSingleton<IActionFabric, ActionFabric>();
            serviceCollection.AddSingleton<ITriggerFabric, TriggerFabric>();
            serviceCollection.AddTransient<IStateMachine, GuessStateMachine>();
            serviceCollection.AddTransient<GuessStateMachine>();

            serviceCollection.AddTransient<InitContext>();
            serviceCollection.AddTransient<CheckNumber>();
            serviceCollection.AddTransient<GuessOKTrigger>();

            _container = serviceCollection.BuildServiceProvider();

            var smService = _container.GetRequiredService<ISMService>();

            var sm = smService.Start<GuessStateMachine>(null, GuessStateMachine.GetDefinition());

            Console.WriteLine(sm.ToString());
            //Console.WriteLine(sm.Context.Number);
            smService.Push(sm, null);

            while (!sm.Finished)
            {          
                var str = Console.ReadLine();
                int i = -1;

                if (int.TryParse(str, out i))
                {
                    var param = new SMParametersCollection();
                    param.Add("EnteredNumber", i);
                    
                    smService.Push(sm, param);

                    Console.WriteLine(sm.Context.Result);
                }
                else
                {
                    Console.WriteLine("Error!");
                }

                Console.WriteLine(sm.ToString());
            }
           

            var persistance = _container.GetRequiredService<IPersistenceService>();
            Console.WriteLine(persistance.To(sm));
            Console.ReadKey();
        }
    }
}

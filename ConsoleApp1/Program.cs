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
using IoC.StateMachine.Core.Extension;
using IoC.StateMachine.Abstractions;
using Microsoft.Extensions.Logging;

namespace IoC.ExampleApp
{
    /// <summary>
    /// Example application, user should guess randomly generated integer value
    /// </summary>
    class Program
    {
        private static IServiceProvider _container;
        static void Main(string[] args)
        {

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(_ => _.AddConsole().SetMinimumLevel(LogLevel.Trace));

            serviceCollection.AddSMCore(s =>
            {
                s.Services.AddSingleton<ISMFactory, SMFactory>()
                          .AddSingleton<IActionFabric, ActionFabric>()
                          .AddSingleton<ITriggerFabric, TriggerFabric>();
            });

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

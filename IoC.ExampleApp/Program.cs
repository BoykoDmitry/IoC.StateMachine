using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.ExampleApp
{
    /// <summary>
    /// Example application, user should guess randomly generated integer value
    /// </summary>
    class Program
    {
        private static IUnityContainer _container = new UnityContainer();
        static void Main(string[] args)
        {
            Unity4StateMachine.SetUpContainer(_container);

            IoC.StateMachine.IoC.SetContainer(new Unity4StateMachine(_container));

            var smService = _container.Resolve<ISMService>();

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
           

            var persistance = _container.Resolve<IPersistenceService>();
            Console.WriteLine(persistance.To(sm));
            Console.ReadKey();
        }
    }
}

using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine
{
    public static class IoC
    {
        private static IAmContainer Container;

        public static void SetContainer(IAmContainer container)
        {
            Container = container;
        }

        public static object Get(Type t, string key = null)
        {
            Affirm.IsNotNull(Container, "IoC is not initialized.");

            return Container.Get(t, key);
        }

        public static T Get<T>(string key = null)
        {
            Affirm.IsNotNull(Container, "IoC is not initialized.");

            return (T)Container.Get(typeof(T), key);
        }

        /// <summary>
        /// Gets all instances of a particular type.
        /// </summary>
        /// <typeparam name="T">The type to resolve.</typeparam>
        /// <returns>The resolved instances.</returns>
        public static IEnumerable<T> GetAll<T>()
        {
            Affirm.IsNotNull(Container, "IoC is not initialized.");

            return Container.GetAll(typeof(T)).Cast<T>();
        }

        public static void BuildUp(object target)
        {
            Affirm.IsNotNull(Container, "IoC is not initialized.");

            Container.BuildUp(target);
        }

        public static IAmContainer CreateChildContainer()
        {
            Affirm.IsNotNull(Container, "IoC is not initialized.");

            return Container.GetChildContainer();
        }
    }
}

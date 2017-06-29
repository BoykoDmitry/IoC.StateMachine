using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine
{
    /// <summary>
    /// Setups and provides container features globaly 
    /// </summary>
    public static class IoC
    {
        private static IAmContainer Container;

        /// <summary>
        /// Initializes container with given instance of <see cref="IAmContainer"/> 
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainer(IAmContainer container)
        {
            Container = container;
        }

        /// <summary>
        /// Get instance of given type
        /// </summary>
        /// <param name="t">type to resolve</param>
        /// <param name="key">key of registered instance</param>
        /// <returns>The instance of given type</returns>
        public static object Get(Type t, string key = null)
        {
            Affirm.IsNotNull(Container, "IoC is not initialized.");

            return Container.Get(t, key);
        }

        /// <summary>
        /// Get instance of a particular type.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="key">Key of registered instance</param>
        /// <returns></returns>
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


        /// <summary>
        /// Build up object
        /// </summary>
        /// <param name="target">The object to build up</param>
        public static void BuildUp(object target)
        {
            Affirm.IsNotNull(Container, "IoC is not initialized.");

            Container.BuildUp(target);
        }


        /// <summary>
        /// Creates child container
        /// </summary>
        /// <returns>Instance of <see cref="IAmContainer"/></returns>
        public static IAmContainer CreateChildContainer()
        {
            Affirm.IsNotNull(Container, "IoC is not initialized.");

            return Container.GetChildContainer();
        }
    }
}

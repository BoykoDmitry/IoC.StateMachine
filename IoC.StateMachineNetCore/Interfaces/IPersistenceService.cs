using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    /// <summary>
    /// Encapsulates logic for serialization of state machine objects
    /// </summary>
    public interface IPersistenceService
    {
        /// <summary>
        /// Converts string to state machine 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">serialized object</param>
        /// <returns></returns>
        T FromSource<T>(string source) where T : class, IStateMachine;

        /// <summary>
        /// serializes object
        /// </summary>
        string To<T>(T sm);

        /// <summary>
        /// serializes state machine <see cref="IStateMachine"/>
        /// </summary>
        string To(IStateMachine sm);

        /// <summary>
        /// deserializes object
        /// </summary>
        T ObjectFromSource<T>(string source) where T : class;

        /// <summary>
        /// deserializes object
        /// </summary>
        object ObjectFromSource(string source, Type t);

        /// <summary>
        /// Load state machine from string, builds up loaded instance
        /// </summary>
        /// <typeparam name="T">IStateMachine</typeparam>
        /// <param name="xml">serialized instance of <see cref="IStateMachine"/></param>
        /// <param name="definition">serialized instance of <see cref="IStateMachineDefinition"/></param>
        /// <returns>Instance of state machine</returns>
        T Load<T>(string xml, string definition) where T : class, IStateMachine;

        /// <summary>
        /// Injects dependencies for statae machine definition 
        /// resolves actions and triggers 
        /// child container(<see cref="IAmContainer.GetChildContainer"/>) will be created for each instance of state machine
        /// </summary>
        /// <param name="sm">instance of state machine</param>
        /// <param name="def">definition of state machine</param>
        void BuildUpDefinition(IStateMachine sm, IStateMachineDefinition def);

        /// <summary>
        /// deserializes from string
        /// </summary>
        /// <param name="source">serialized object</param>
        /// <param name="type"></param>
        /// <returns></returns>
		object FromSource(string source, Type type);		
    }
}

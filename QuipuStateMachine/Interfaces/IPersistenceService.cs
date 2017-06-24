using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface IPersistenceService
    {
        T FromSource<T>(string source) where T : class, IStateMachine;
        string To<T>(T sm);
        string To(IStateMachine sm);
        T ObjectFromSource<T>(string source) where T : class;
        object ObjectFromSource(string source, Type t);
        T Load<T>(string xml, string definition) where T : class, IStateMachine;
        void BuildUpDefinition(IStateMachine sm, IStateMachineDefinition def);

		object FromSource(string source, Type type);		
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface IParameter<TKey, TValue>
    {
        TKey Key { get; set; }
        TValue Value { get; set; }
    }

    public interface ISMParameter : IParameter<string, object>
    {
    }

    public interface ISMParameters : ICollection<ISMParameter>
    {
        void Add(string key, object value);
        bool TryGetValue<T>(string key, out T objVal) where T : class;
        T GetParameter<T>(string key) where T : IConvertible;
    }
}

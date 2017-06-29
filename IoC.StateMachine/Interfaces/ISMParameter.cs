using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    /// <summary>
    /// Key value pair 
    /// </summary>
    public interface IParameter<TKey, TValue> : IRoot
    {
        TKey Key { get; set; }
        TValue Value { get; set; }
    }

    /// <summary>
    /// Parameter for <see cref="ISMParameters"/>
    /// </summary>
    public interface ISMParameter : IParameter<string, object>
    {
    }

    /// <summary>
    /// Collection of parameters that used for actions or triggers 
    /// </summary>
    public interface ISMParameters : ICollection<ISMParameter>
    {
        void Add(string key, object value);
        bool TryGetValue<T>(string key, out T objVal) where T : class;
        T GetParameter<T>(string key) where T : IConvertible;
    }
}

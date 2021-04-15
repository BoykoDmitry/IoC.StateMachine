using IoC.StateMachine.Interfaces;
using IoC.StateMachine.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    /// <summary>
    /// Represents parameter(key, value) <see cref="ISMParameter"/>
    /// </summary>
    [Serializable]
    [DataContract]
    public class SMParameter : INotifyPropertyChanged, ISMParameter
    {
        public SMParameter() { }
        public SMParameter(string key, object value)
        {
            Key = key;
            Value = value;
        }
        [DataMember]
        public string Key { get; set; }

        private object _value;

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        [DataMember]
        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("Value"));
            }
        }

        public override string ToString()
        {
            return Value == null ? base.ToString() : string.Format("{0}:{1}", Key, Value);
        }
    }

    /// <summary>
    /// Collection of parameters(key, value) <see cref="ISMParameters"/>
    /// </summary>
    [Serializable]
    [DataContract]
    public class SMParametersCollection : ISMParameters
    {
        private List<ISMParameter> _parameters;
        [DataMember]
        public List<ISMParameter> Parameters
        {
            get
            {
                _parameters = _parameters ?? new List<ISMParameter>();
                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        public SMParametersCollection()
        {
            // Parameters = new List<ISMParameter>();
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public int Count
        {
            get
            {
                return Parameters.Count;
            }
        }


        public object GetValue(string key)
        {
            var s = Get(key);
            return s == null ? null : s.Value;
        }

        public ISMParameter Get(string key)
        {
            return Parameters.FirstOrDefault(_ => _.Key == key);
        }

        public void Add(string key, object value)
        {
            Parameters.Add(new SMParameter(key, value));
        }

        public bool TryGetValue<T>(string key, out T objVal) where T : class
        {
            objVal = null;
            var val = Parameters.FirstOrDefault(_ => _.Key == key);

            if (val != null)
                objVal = val.Value as T;

            return val != null;
        }

        public T GetParameter<T>(string key) where T : IConvertible
        {
            if (!Parameters.Any())
                return default(T);

            var value = Parameters.FirstOrDefault(_ => _.Key == key);

            if (value != null)
                return (T)System.Convert.ChangeType(value.Value, typeof(T));

            return default(T);
        }

        public void Add(ISMParameter item)
        {
            Parameters.Add(item);
        }

        public void Clear()
        {
            Parameters.Clear();
        }

        public bool Contains(ISMParameter item)
        {
            return Parameters.Contains(item);
        }

        public void CopyTo(ISMParameter[] array, int arrayIndex)
        {
            Parameters.CopyTo(array, arrayIndex);
        }

        public bool Remove(ISMParameter item)
        {
            return Parameters.Remove(item);
        }

        public IEnumerator<ISMParameter> GetEnumerator()
        {
            return Parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Parameters.GetEnumerator();
        }

        public override string ToString()
        {
            return Parameters.Count == 0 ? base.ToString() : string.Join("\r\n\t", Parameters.Select(_ => _.ToString()).ToArray());
        }
    }
}

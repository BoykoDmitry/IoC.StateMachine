using IoC.StateMachine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoC.StateMachine.Interfaces;
using System.Runtime.Serialization;
using System.IO;

namespace IoC.StateMachine.Serialization
{
    public class DataContractPersistenceService : BasePersistenceService
    {
        private readonly AssemblyDataContractResolver _resolver;
        public DataContractPersistenceService(IEnumerable<string> assemblyNames)
        {
            _resolver = new AssemblyDataContractResolver(assemblyNames);           
        }

        public static int MaxItemsInObjectGraph = 15000;
        private static DataContractSerializer getSerializer(Type t, AssemblyDataContractResolver res, string root)
        {
            var x = new DataContractSerializer(t, !string.IsNullOrEmpty(root) ? root : t.Name, "", res.KnownTypes, MaxItemsInObjectGraph, true, true, null, res);
            return x;
        }

        public string ToXML(object o, AssemblyDataContractResolver res, string root = null)
        {
            string serializedValue = "";

            using (MemoryStream ms = new MemoryStream())
            {
                var ser = getSerializer(o.GetType(), res, root);

                ser.WriteObject(ms, o);
                ms.Position = 0;
                var sr = new StreamReader(ms, Encoding.UTF8);
                serializedValue = sr.ReadToEnd();

                sr.Close();
                ms.Close();
            }

            return serializedValue;
        }

        public object FromXML(string xml, Type type, AssemblyDataContractResolver res = null)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                using (MemoryStream xms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                {
                    var x = getSerializer(type, res ?? _resolver, null);
                    var s = x.ReadObject(xms);
                    return s;
                }
            }
            return null;
        }

        public override object FromSource(string source, Type type)
        {
            return FromXML(source, type);
        }

        public override T FromSource<T>(string source)
        {
            return FromXML(source, typeof(T)) as T;
        }

        public override object ObjectFromSource(string source, Type t)
        {
            return FromXML(source, t);
        }

        public override T ObjectFromSource<T>(string source)
        {
            return FromXML(source, typeof(T)) as T;
        }

        public override string To(IStateMachine sm)
        {
            return ToXML(sm, _resolver);
        }

        public override string To<T>(T obj)
        {
            return ToXML(obj, _resolver);
        }
    }
}

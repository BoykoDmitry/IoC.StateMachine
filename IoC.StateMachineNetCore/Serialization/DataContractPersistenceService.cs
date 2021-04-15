using IoC.StateMachine.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;
using IoC.StateMachine.Abstractions;
using Microsoft.Extensions.Logging;

namespace IoC.StateMachine.Serialization
{
    /// <summary>
    /// Implementation of serialization with usage of <see cref="DataContractSerializer"/>
    /// </summary>
    public class DataContractPersistenceService : BasePersistenceService
    {
        private readonly AssemblyDataContractResolver _resolver;
        /// <summary>
        /// List of assemblies to be considered in serialization
        /// </summary>
        /// <param name="assemblyNames"></param>
        public DataContractPersistenceService(IEnumerable<string> assemblyNames, IServiceProvider serviceProvider, ILogger<DataContractPersistenceService> logger) : base(serviceProvider, logger) 
        {
            _resolver = new AssemblyDataContractResolver(assemblyNames);           
        }

        public static int MaxItemsInObjectGraph = 15000;
        private static DataContractSerializer getSerializer(Type t, AssemblyDataContractResolver res, string root)
        {
            var x = new DataContractSerializer(t, !string.IsNullOrEmpty(root) ? root : t.Name, "", res.KnownTypes);
            return x;
        }

        public string ToXML(object o, AssemblyDataContractResolver res, string root = null)
        {
            string serializedValue = "";

            var ms = new MemoryStream();
            var vr = XmlDictionaryWriter.CreateTextWriter(ms);

            var ser = getSerializer(o.GetType(), res, root);
            ser.WriteObject(vr, o, res);
            vr.Flush();
            ms.Position = 0;

            using (var sr = new StreamReader(ms, Encoding.UTF8))
            {
                serializedValue = sr.ReadToEnd();
            }

            ms.Close();
            //vr.Close(); -- hope ms.close will dispose textwritter too.

            return serializedValue;
        }

        public object FromXML(string xml, Type type, AssemblyDataContractResolver res = null)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                using (MemoryStream xms = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
                using (var rd = XmlDictionaryReader.CreateTextReader(xms, new XmlDictionaryReaderQuotas()))
                {
                    var x = getSerializer(type, res ?? _resolver, null);
                    var s = x.ReadObject(rd, false, res ?? _resolver);
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

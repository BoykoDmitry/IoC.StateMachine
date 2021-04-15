using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using IoC.StateMachine.Abstractions;

namespace IoC.StateMachine.Serialization
{
    public class AssemblyDataContractResolver : DataContractResolver
    {
        Dictionary<string, XmlDictionaryString> dictionary = new Dictionary<string, XmlDictionaryString>();
        private readonly IList<Assembly> assemblies;

        public AssemblyDataContractResolver NextResolver { get; set; }

        public AssemblyDataContractResolver(IEnumerable<string> assemblyNames)
        {
            KnownTypes = new List<Type>();
            assemblies = new List<Assembly>();

            foreach (var assemblyName in assemblyNames)
            {
                var assembly = Assembly.Load(new AssemblyName(assemblyName));
                assemblies.Add(assembly);
                foreach (Type type in assembly.GetTypes())
                {
                    bool knownTypeFound = false;
                    Attribute[] attrs = Attribute.GetCustomAttributes(type);
                    if (attrs.Length != 0)
                    {
                        foreach (Attribute attr in attrs)
                        {
                            if (attr is KnownTypeAttribute)
                            {
                                Type t = ((KnownTypeAttribute)attr).Type;
                                if (KnownTypes.IndexOf(t) < 0)
                                    KnownTypes.Add(t);
                                knownTypeFound = true;
                            }
                        }
                    }
                    if (!knownTypeFound)
                    {
                        var name = type.Name;
                        var namesp = type.Namespace;
                        if (!dictionary.ContainsKey(name))
                            dictionary.Add(name, new XmlDictionaryString(XmlDictionary.Empty, name, 0));

                        if (!string.IsNullOrEmpty(namesp))
                            if (!dictionary.ContainsKey(namesp))
                                dictionary.Add(namesp, new XmlDictionaryString(XmlDictionary.Empty, namesp, 0));
                    }
                }
            }
        }

        public IList<Type> KnownTypes
        {
            get; set;
        }

        // Used at deserialization  
        // Allows users to map xsi:type name to any Type
        public override Type ResolveName(string typeName, string typeNamespace, Type declaredType, DataContractResolver knownTypeResolver)
        {
            XmlDictionaryString tName;
            XmlDictionaryString tNamespace;

            if (dictionary.TryGetValue(typeName, out tName) && dictionary.TryGetValue(typeNamespace, out tNamespace))
            {
                return assemblies.Select(_ => _.GetType(tNamespace.Value + "." + tName.Value)).FirstOrDefault();
            }
            else
                return knownTypeResolver.ResolveName(typeName, typeNamespace, declaredType, NextResolver) ?? declaredType;
        }

        // Used at serialization  
        // Maps any Type to a new xsi:type representation  
        public override bool TryResolveType(Type type, Type declaredType, DataContractResolver knownTypeResolver, out XmlDictionaryString typeName, out XmlDictionaryString typeNamespace)
        {
            knownTypeResolver.TryResolveType(type, declaredType, NextResolver, out typeName, out typeNamespace);
            if (typeName == null || typeNamespace == null)
            {
                typeName = new XmlDictionaryString(XmlDictionary.Empty, type.Name, 0);
                typeNamespace = new XmlDictionaryString(XmlDictionary.Empty, type.Namespace, 0);
            }
            return typeName != null && typeNamespace != null;
        }
    }
}

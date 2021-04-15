using IoC.StateMachine.Abstractions;
using IoC.StateMachine.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Json
{
    /// <summary>
    /// Implementation of serialization with usage of <see cref="Newtonsoft"/>
    /// </summary>
    public class JsonPersistenceService : BasePersistenceService
    {
        private readonly JsonSerializerSettings settings;
        public JsonPersistenceService(IServiceProvider serviceProvider, ILogger<JsonPersistenceService> logger) : base(serviceProvider, logger)
        {
            settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

        }

        public string ToJson(object o)
        {
            var indented = Formatting.Indented;

            return JsonConvert.SerializeObject(o, indented, settings); 
        }

        public object FromJson(string json, Type type)
        {
            if (!string.IsNullOrEmpty(json))
            {
                return JsonConvert.DeserializeObject(json, settings);
            }
            return null;
        }

        public override object FromSource(string source, Type type)
        {
            return FromJson(source, type);
        }

        public override T FromSource<T>(string source)
        {
            return FromJson(source, typeof(T)) as T;
        }

        public override object ObjectFromSource(string source, Type t)
        {
            return FromJson(source, t);
        }

        public override T ObjectFromSource<T>(string source)
        {
            return FromJson(source, typeof(T)) as T;
        }

        public override string To(IStateMachine sm)
        {
            return ToJson(sm);
        }

        public override string To<T>(T obj)
        {
            return ToJson(obj);
        }
    }
}

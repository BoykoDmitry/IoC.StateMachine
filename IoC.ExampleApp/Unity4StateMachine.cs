﻿//using IoC.StateMachine.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using IoC.StateMachine;
//using IoC.StateMachine.Core;
//using IoC.StateMachine.Serialization;

//namespace IoC.ExampleApp
//{
//    public class Unity4StateMachine : IAmContainer
//    {
//        public static void SetUpContainer(IUnityContainer _container)
//        {
//            _container.RegisterType<ISMService, SMService>();
//            _container.RegisterInstance<IPersistenceService>(new DataContractPersistenceService(new List<string>() { "IoC.ExampleApp" }));
//            _container.RegisterType<IStateProcessor, StateProcessor>();
//            _container.RegisterType<ISMAction, InitContext>("InitContext");
//            _container.RegisterType<ISMAction, CheckNumber>("CheckNumber");
//            _container.RegisterType<ISMTrigger, GuessOKTrigger>("GuessOKTrigger");

//        }


//        private readonly IUnityContainer _container;
//        public Unity4StateMachine(IUnityContainer container)
//        {
//            Affirm.ArgumentNotNull(container, "container");
//            _container = container;
//        }

//        public void BuildUp(object target)
//        {
//            _container.BuildUp(target);
//        }

//        public object Get(Type service, string key = null)
//        {
//            return _container.Resolve(service, key);
//        }

//        public T Get<T>(string key = null)
//        {
//            return _container.Resolve<T>(key);
//        }

//        public IEnumerable<object> GetAll(Type service)
//        {
//            return _container.ResolveAll(service);
//        }

//        public IAmContainer GetChildContainer()
//        {
//            return new Unity4StateMachine(_container.CreateChildContainer());
//        }

//        public void Register(Type fromT)
//        {
//            _container.RegisterType(fromT);
//        }

//        public void Register(Type fromT, Type toT)
//        {
//            _container.RegisterType(fromT, toT);
//        }

//        public void RegisterInstance(Type service, object instance)
//        {
//            _container.RegisterInstance(service, instance);
//        }

//        #region IDisposable Support
//        private bool disposedValue = false; // To detect redundant calls
//        private bool isBeingDissposed = false;

//        protected virtual void Dispose(bool disposing)
//        {
//            if (this.isBeingDissposed)
//                return;

//            if (!disposedValue)
//            {
//                if (disposing)
//                {
//                    if (_container != null)
//                    {
//                        isBeingDissposed = true;
//                        _container.Dispose();
//                        isBeingDissposed = false;
//                    }
//                }

//                disposedValue = true;
//            }
//        }



//        // This code added to correctly implement the disposable pattern.
//        public void Dispose()
//        {
//            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
//            Dispose(true);
//            // TODO: uncomment the following line if the finalizer is overridden above.
//            // GC.SuppressFinalize(this);
//        }
//        #endregion
//    }
//}

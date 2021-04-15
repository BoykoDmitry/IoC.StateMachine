using IoC.StateMachine.Core.Classes;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using IoC.StateMachine.Core.Extension;
using Microsoft.Extensions.DependencyInjection;
using IoC.StateMachine.Abstractions;

namespace IoC.StateMachine.Core
{
    /// <summary>
    /// Base class for state machine
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataContract]
	[Serializable]
    public abstract class StateMachineBase<T> : StateMachineDto<T>, IStateMachine
        where T: ContextBase
    {
		public StateMachineBase()
		{
			StateHistory = new Moves();
		}

        [XmlIgnore]   
        public IStateMachineDefinition Definition { get; private set; }
        [XmlIgnore]
        public IState CurrentState { get; set; }

        public void SetDefinition(IStateMachineDefinition definition)
        {
            Affirm.ArgumentNotNull(definition, "definition");

            definition.Validate();

            Definition = definition;
        }

        public void SetCurrentState(string stateId)
        {
            SetCurrentState(Definition.GetStateById(stateId));
        }

        public void SetCurrentState(IState state)
        {
            if (state != null)
                Definition.CheckState(state);

			StateHistory.Add(CurrentStateId, state.id);
			PreviousStateId = CurrentStateId;

            CurrentState = state;
            CurrentStateId = state.id;

            if (state.EndPoint)
                Finished = true;
        }

        [DataMember]
        public string PreviousStateId { get; set; }

        Moves _stateHistory = null;
        [DataMember]
        public Moves StateHistory
        {
            get { return _stateHistory ?? new Moves(); }
            set { _stateHistory = value; } }

		[XmlIgnore]
        public IEnumerable<IMove> StateMoves 
		{ 
			get { return _stateHistory; }
			set
			{
                _stateHistory.Clear();
				if (value != null)
					foreach (var m in value)
                        _stateHistory.Add((Move)m);
			}
		}

        public IServiceScope Container { get; set; }

        public override string ToString()
        {
            return "StateMachine {0}, current state {1} ".FormIt(SmId, CurrentStateId);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Container?.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~StateMachineBase() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

    }
}

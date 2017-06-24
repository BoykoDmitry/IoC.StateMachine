using IoC.StateMachine.Core.Classes;
using IoC.StateMachine.Interfaces;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using IoC.StateMachine.Core.Extension;

namespace IoC.StateMachine.Core
{
    [DataContract]
	[Serializable]
    public abstract class StateMachineBase<T> : StateMachineDto<T>
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
        [XmlIgnore]
        public bool WasInit { get; set; }

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

        public override string ToString()
        {
            return "StateMachine {0}, current state {1} ".FormIt(SmId, CurrentStateId);
        }
      
    }
}

using System;
using System.Runtime.Serialization;

namespace IoC.StateMachine.Core
{
    [Serializable]
    [DataContract]
	public class StateMachineDto<T>
	{
        [DataMember]
        public Guid SmId { get; set; }

		private T _context;
        [DataMember]
        public virtual T Context { get { return _context; } set { _context = value; } }

        [DataMember]
        public string CurrentStateId { get; set; }

        [DataMember]
        public bool Finished { get; set; }
	}
}

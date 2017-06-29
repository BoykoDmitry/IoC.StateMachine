using IoC.StateMachine.Interfaces;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    /// <summary>
    /// base class for action holders (triggers or actions)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    [DataContract]
    public abstract class SMActionHolderBase<T> 
    {
        /// <summary>
        /// Parameters for action 
        /// </summary>
        private SMParametersCollection _parameters;
        [DataMember]
        public SMParametersCollection Parameters
        {
            get
            {
                _parameters = _parameters ?? new SMParametersCollection();

                return _parameters;
            }
            set
            {
                _parameters = value;
            }
        }

        /// <summary>
        /// Key which will be used to resolve action class from container
        /// </summary>
        [DataMember]
        public string Code { get; set; }

		[NonSerialized]
		T _nestedAction;

        [XmlIgnore]
		public T NestedAction { get { return _nestedAction; } set { _nestedAction = value; } }

        public void SetParameters(ISMParameters parameters)
        {
            Parameters = parameters as SMParametersCollection;
        }
        /// <summary>
        /// Order of execution, among other actions in the list
        /// </summary>
        [DataMember]
        public int Order { get; set; }
    }

    /// <summary>
    /// Wrapper for <see cref="ISMAction"/>
    /// </summary>
	[Serializable]
    [DataContract]
    public class SMActionHolder : SMActionHolderBase<ISMAction>, IActionHolder
    {
        ISMParameters IHolderBase.Parameters
        {
            get
            {
                return Parameters as ISMParameters;
            }
        }

        /// <summary>
        /// Execution of the action with parameters from state machine push
        /// </summary>
        /// <param name="TransitionParameters">Parameters from state machine push <see cref="ISMService.Push(IStateMachine, ISMParameters)"/></param>
        public void Invoke(ISMParameters TransitionParameters)
        {
            if (NestedAction != null)
                NestedAction.Invoke(Parameters,TransitionParameters);
        }
    }

    /// <summary>
    /// Wrapper for <see cref="ISMTrigger"/>
    /// </summary>
    [Serializable]
    [DataContract]
    public class SMATriggerHolder : SMActionHolderBase<ISMTrigger>, ITriggerHolder
    {
        /// <summary>
        /// Defines if result of the trigger must be inverted
        /// </summary>
        [DataMember]
        public bool Inverted
        {
            get; set;
        }

        ISMParameters IHolderBase.Parameters
        {
            get
            {
                return Parameters as ISMParameters;
            }
        }

        /// <summary>
        /// Execution of the action with parameters from state machine push
        /// </summary>
        /// <param name="transition">Transition to which trigger belongs <see cref="ITransition"/></param>
        /// <param name="TransitionParameters">Parameters from state machine push <see cref="ISMService.Push(IStateMachine, ISMParameters)"/></param>
        public bool Invoke(ITransition transition,ISMParameters TransitionParameters)
        {
            if (NestedAction != null)
            {
                var result = NestedAction.Invoke(transition, Parameters, TransitionParameters);

                return Inverted ? !result : result;
            }

            return false;
        }
    }
}

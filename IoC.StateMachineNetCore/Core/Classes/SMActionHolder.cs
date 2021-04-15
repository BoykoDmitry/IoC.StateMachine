using IoC.StateMachine.Abstractions;
using Microsoft.Extensions.Logging;
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
        protected readonly ILogger _logger;
        protected SMActionHolderBase(ILogger logger)
        {
            Affirm.ArgumentNotNull(logger, nameof(logger));

            _logger = logger;
        }      
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
        public SMActionHolder(ILogger<SMActionHolder> logger) : base(logger)
        {
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
        public SMATriggerHolder(ILogger<SMATriggerHolder> logger) : base(logger)
        {
        }

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
                _logger.LogDebug("trigger {0} for transition {1} is going to be executed".FormIt(NestedAction.GetType().ToString(), transition.Text));

                var result = NestedAction.Invoke(transition, Parameters, TransitionParameters);
                
                var inv = Inverted ? !result : result;

                _logger.LogDebug("Trigger result is {0}".FormIt(inv));

                return inv;
            }

            return false;
        }
    }
}

using IoC.StateMachine.Interfaces;
using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace IoC.StateMachine.Core.Classes
{
    [Serializable]
    [DataContract]
    public abstract class SMActionHolderBase<T> 
    {
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

        [DataMember]
        public int Order { get; set; }
    }

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

        public void Invoke(ISMParameters TransitionParameters)
        {
            if (NestedAction != null)
                NestedAction.Invoke(Parameters,TransitionParameters);
        }
    }

	[Serializable]
    [DataContract]
    public class SMATriggerHolder : SMActionHolderBase<ISMTrigger>, ITriggerHolder
    {
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

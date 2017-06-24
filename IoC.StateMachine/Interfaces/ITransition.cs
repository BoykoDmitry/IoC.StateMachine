using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface ITransition : ISMBaseElement
    {
        string TargetStateId { get; set; }
        string SourceStateId { get; set; }
        ITriggerHolder Trigger { get; set; }
        double Curviness { get; set; }
    }

}

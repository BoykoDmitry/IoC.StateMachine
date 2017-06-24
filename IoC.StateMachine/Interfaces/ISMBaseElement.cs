using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface ISMBaseElement
    {
        IStateMachine StateMachine { get; set; }
        string id { get; set; }
        string Points { get; set; }
        string Text { get; set; }
    }
}

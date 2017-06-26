using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
    public interface IState : ISMBaseElement
    {
        IList<IActionHolder> EnterActions { get; }
        IList<IActionHolder> ExitActions { get; }
        bool StartPoint { get; set; }
        bool EndPoint { get; set; }
		string id { get; }
    }   
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Interfaces
{
	public interface IMove
	{
		string SourceStateId { get; }
		string TargetStateId { get; }
		int Order { get; }
	}

	public interface IMoveCollection : ICollection<IMove>
	{
		IMove Add(string source, string target);
	}
}

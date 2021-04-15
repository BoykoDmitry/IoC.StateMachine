using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Abstractions
{
    /// <summary>
    /// Represents one move of state machine
    /// </summary>
	public interface IMove
	{
        /// <summary>
        /// from state id
        /// </summary>
		string SourceStateId { get; }

        /// <summary>
        /// to state id
        /// </summary>
		string TargetStateId { get; }

        /// <summary>
        /// Order
        /// </summary>
		int Order { get; }
	}

    /// <summary>
    /// Collection to store moves
    /// </summary>
	public interface IMoveCollection : ICollection<IMove>
	{
		IMove Add(string source, string target);
	}
}

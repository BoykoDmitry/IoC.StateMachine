﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Abstractions
{
    public interface IRoot 
    {

    }

    public interface IHaveStateMachine
    {
        /// <summary>
        /// Parent state machine
        /// </summary>
        IStateMachine StateMachine { get; set; }
    }

    /// <summary>
    /// Base class with common properties for state machine definition classes
    /// </summary>
    public interface ISMBaseElement : IRoot, IHaveStateMachine
    {
        /// <summary>
        /// id of the element, like transition or state
        /// </summary>
        string id { get; set; }

        /// <summary>
        /// can be used to store coordinates of the element in the screen, for designer
        /// </summary>
        string Points { get; set; }

        /// <summary>
        /// can be used to store comments and etc, for designer
        /// </summary>
        string Text { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoC.StateMachine.Abstractions
{
    /// <summary>
    /// Represents transition between two states <see cref="ITransition"/>
    /// </summary>
    public interface ITransition : ISMBaseElement
    {
        /// <summary>
        /// State to 
        /// </summary>
        string TargetStateId { get; set; }

        /// <summary>
        /// State from 
        /// </summary>
        string SourceStateId { get; set; }

        /// <summary>
        /// Trigger wrapper wich defines condition for transition
        /// </summary>
        ITriggerHolder Trigger { get; set; }

        /// <summary>
        /// For designer, curviness of the line 
        /// </summary>
        double Curviness { get; set; }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Target
{
    /// <summary>
    /// A class inheriting from <c>Target</c> that specifies what should be done when a fun target is hit.
    /// </summary>
    public class FunTarget : Target
    {
        /// <summary>
        /// A method that is called when the fun target is hit. It increments the counter for number of fun targets hit.
        /// </summary>
        protected override void Hit()
        {
            // Give points to player
            pointsController.IncrementFunTargetCounter();
        }
    }
}


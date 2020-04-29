using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RandomColor
{
    /// <summary>
    /// Gives the mesh a "rainbowy" random color
    /// </summary>
    public class RandomColor : MonoBehaviour
    { 
        /// <summary>
        /// Gives the mesh a "rainbowy" random color
        /// </summary>
        void Start()
        {
            GetComponent<Renderer>().material.color = Random.ColorHSV(0, 1, 1, 1, 1, 1, 1, 1);
        }
    }
}

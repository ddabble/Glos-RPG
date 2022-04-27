using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RandomColor
{
    /// <summary>
    /// Gives the mesh a "rainbowy" random color
    /// </summary>
    public class RandomBikeColor : MonoBehaviour
    {
        /// <summary>
        /// Gives the mesh a "rainbowy" random color
        /// </summary>
        void Start()
        {
            var color = Random.ColorHSV(0, 1, 1, 1, 0.6f, 0.6f, 1, 1);
            for (int i = 0; i < 6; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                child.GetComponent<Renderer>().material.color = color;
            }
        }
    }
}

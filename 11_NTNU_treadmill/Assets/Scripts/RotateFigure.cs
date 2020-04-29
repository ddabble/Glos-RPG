using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RotateFigure
{
    /// <summary>
    /// A class that gives a game object smooth rotation and vertical oscillation 
    /// </summary>
    public class RotateFigure : MonoBehaviour
    {
        // Rotates and bounces the GameObject
        private float translationOffset;
        private float initialY;


        /// <summary>
        /// Sets a random starting rotation and a offset for the sine wave oscillation
        /// </summary>
        void Start()
        {
            translationOffset = (float)new System.Random(transform.GetHashCode()).NextDouble() * 10000f;
            initialY = transform.position.y;
            transform.Rotate(0, (float)new System.Random(transform.GetHashCode() + 1).NextDouble() * 10000f, 0);
        }

        /// <summary>
        /// Rotates the gameobject and moves it vertically along a sine wave.
        /// </summary>
        void Update()
        {
            transform.Rotate(0, 40f * Time.deltaTime, 0);
            transform.position = new Vector3(transform.position.x, initialY + Mathf.Sin(translationOffset + Time.timeSinceLevelLoad) / 5, transform.position.z);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace DestroyAfterTime
{
    /// <summary>
    /// Destroys the game object after a time delay.
    /// </summary>
    public class DestroyAfterTime : MonoBehaviour
    {
        /// <summary>
        /// The time in seconds from the object is spawned to it is destroyed.
        /// </summary>
        public float lifetime;

        /// <summary>
        /// Sets the game object to be destroyed after the time delay given by <c>lifetime</c>.
        /// </summary>
        void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IntegrasjonRotationHandler
{
    /// <summary>
    /// A class for that handles the rotation of the "Integrasjon" statue
    /// </summary>
    public class IntegrasjonRotationHandler : MonoBehaviour
    {

        /// <summary>
        /// The speed at which the statue rotates
        /// </summary>
        public float speed = 20f;

        /// <summary>
        /// Rotates the figure every game update
        /// </summary>
        void Update()
        {
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }
    }
}

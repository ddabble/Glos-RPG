using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpawnPoint
{
    /// <summary>
    /// A debug class for the spawn points for easier editing in the editor. Makes the spawn points appear as yellow spheres.
    /// </summary>
    public class SpawnPoint : MonoBehaviour
    {
        /// <summary>
        /// Instead of drawing the default gizmo, draws a yello sphere
        /// </summary>
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}

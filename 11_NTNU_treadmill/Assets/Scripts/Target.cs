using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Target
{
    /// <summary>
    /// A parent class for both info targets and fun targets
    /// </summary>
    public abstract class Target : MonoBehaviour
    {
        /// <summary>
        /// The audio source that is played when the target is hit.
        /// </summary>
        public AudioSource hitAudioSource;

        /// <summary>
        /// The meshes of this game object. Is used to turn of this object's visibility, but not destroy the object as that would
        /// destroy the audio source as well.
        /// </summary>
        public MeshRenderer[] mrs;

        /// <summary>
        /// The <c>PointsController</c> in the scene that e.g. counts the number of targets that is hit.
        /// </summary>
        protected Quest.PointsController pointsController;

        /// <summary>
        /// The type of the <c>OnHitEvent</c>
        /// </summary>
        public delegate void OnHit();

        /// <summary>
        /// An event that is triggered when the target is hit.
        /// </summary>
        public event OnHit OnHitEvent;

        /// <summary>
        /// Whether the target has been already hit.
        /// </summary>
        private bool hit = false;

        /// <summary>
        /// Finds the points controller in the scene, that is responsible for keeping the score of targets hit.
        /// </summary>
        private void Awake()
        {
            pointsController = GameObject.Find("QuestController").GetComponent<Quest.PointsController>();
        }

        /// <summary>
        /// Removes the game object, runs the hit-methods of the subclasses and plays a sound if the target collides with a projectile.
        /// </summary>
        /// <param name="other">The collision that triggers the method</param>
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "projectile" && !hit)
            {
                hit = true;
                OnHitEvent?.Invoke();
                Hit();
                hitAudioSource.Play();
                foreach (MeshRenderer mr in mrs)
                {
                    mr.enabled = false;
                }
                Destroy(other);
                Destroy(this.transform.gameObject, hitAudioSource.clip.length);
            }
        }

        /// <summary>
        /// A method that is overriden by subclasses and is called when the target is hit.
        /// </summary>
        protected abstract void Hit();
    }
}


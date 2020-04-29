using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Race
{
    /// <summary>
    /// Handles the finish line of the race
    /// </summary>
    public class RaceGoalHandler : MonoBehaviour
    {

        /// <summary>
        /// The time since start of the game at which the race was started.
        /// </summary>
        public float startTime;

        /// <summary>
        /// The difference in time since startTime and when the player reaching the goal.
        /// </summary>
        private float raceTime;

        /// <summary>
        ///Collider of the line to cross.
        /// </summary>
        private BoxCollider bCollider;

        /// <summary>
        /// Mesh renderer of the line to cross.
        /// </summary>
        private MeshRenderer meshRenderer;

        /// <summary>
        /// Event which calls triggers when player crosses finish line
        /// </summary>
        public UnityEvent finishRace = new UnityEvent();

        /// <summary>
        /// TO ensure that the trigger when you collide with the goal is not triggered twice.
        /// </summary>
        private bool collided = false;

        /// <summary>
        /// Sets the start time variable when game object is activated.
        /// </summary>
        private void OnEnable()
        {
            startTime = Time.time;
        }

        /// <summary>
        /// Disables the meshrenderer and collider for the red line, and calculates the race time.
        /// </summary>
        /// <param name="other">The collider that initiated the trigger event.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player" && !collided)
            {
                collided = true;
                bCollider.enabled = false;
                meshRenderer.enabled = false;
                raceTime = Time.time - startTime;
                Debug.Log(Time.time - startTime);
                
                finishRace.Invoke();
            }
        }

        public float GetRaceTime()
        {
            return raceTime;
        }

        /// <summary>
        /// Restarts the finish line to its initial state
        /// </summary>
        public void Restart()
        {
            bCollider = GetComponent<BoxCollider>();
            meshRenderer = GetComponent<MeshRenderer>();
            collided = false;
            bCollider.enabled = true;
            meshRenderer.enabled = true;
        }
    }
}

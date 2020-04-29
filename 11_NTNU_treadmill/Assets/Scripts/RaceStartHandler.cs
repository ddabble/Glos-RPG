using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Race
{
    /// <summary>
    /// Handles the starting line of the race.
    /// </summary>
    public class RaceStartHandler : MonoBehaviour
    {

        /// <summary>
        /// Mesh renderer of the line to cross.
        /// </summary>
        private MeshRenderer meshRenderer;

        /// <summary>
        /// The gameobject to activate when the race is started.
        /// </summary>
        public GameObject finishLine;

        /// <summary>
        /// Event which calls triggers when player crosses start line
        /// </summary>
        public UnityEvent startRace = new UnityEvent();

        /// <summary>
        /// Information explaining the race (and the reason for it)
        /// </summary>
        public GameObject infoBox;
        
        /// <summary>
        /// Assigns the meshrenderer at startup
        /// </summary>
        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            SetVisibility(false);
        }

        /// <summary>
        /// Disables the meshrenderer for the red line, and activates the finish line.
        /// </summary>
        /// <param name="other">The collider that initiated the trigger event.</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Player")
            {
                meshRenderer.enabled = false;
                infoBox.SetActive(false);
                finishLine.SetActive(true);
                startRace.Invoke();
            }
        }

        /// <summary>
        /// Sets the visibility of the start line
        /// </summary>
        /// <param name="visibility">Whether the start line is going to be visible or not</param>
       public void SetVisibility(bool visibility)
        {
            GameObject startline = transform.parent.gameObject;
            foreach (MeshRenderer mesh in startline.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.enabled = visibility;
            }
        }

        /// <summary>
        /// Restarts the race to its initial state
        /// </summary>
        public void RestartRace()
        {
            SetVisibility(true);
            meshRenderer.enabled = true;
            finishLine.SetActive(true);
            finishLine.GetComponentInChildren<RaceGoalHandler>().Restart();
            finishLine.SetActive(false);
        }
    }
}

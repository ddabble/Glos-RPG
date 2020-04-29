using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;

namespace HandDisplay
{
    /// <summary>
    /// A class for the hand display that toggles the display when the button is clicked on the controller.
    /// </summary>
    public class HandDisplay : MonoBehaviour
    {
        /// <summary>
        /// The action that toggles the hologram (the button click).
        /// </summary>
        public SteamVR_Action_Boolean grabPinch;

        /// <summary>
        /// The controller where the input happens (the left controller).
        /// </summary>
        private readonly SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.LeftHand;

        /// <summary>
        /// The hologram in your left hand whose visibility is toggled.
        /// </summary>
        private GameObject hologram;

        /// <summary>
        /// The type of <c>OnToggleEvent</c>
        /// </summary>
        public delegate void OnToggle();

        /// <summary>
        /// An event that is triggered when the hologram display's visibility is toggled.
        /// </summary>
        public event OnToggle OnToggleEvent;

        /// <summary>
        /// Whether or not the hand display is currently visible.
        /// </summary>
        private bool visible = true;

        /// <summary>
        /// Finds the hologram in the game hierarchy and set a trigger on the "toggle hologram visibility"-button on the controller.
        /// </summary>
        void Start()
        {
            Debug.Log("Starting handDisplay script");
            hologram = GameObject.Find("HandDisplay - Sphere");
            if (grabPinch != null)
            {
                grabPinch.AddOnStateDownListener(ToggleDisplay, inputSource);
            }
        }

        private void OnDestroy()
        {
            grabPinch.RemoveOnStateDownListener(ToggleDisplay, inputSource);
        }

        /// <summary>
        /// Checks whether the space button is clicked and toggles the display based on that.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                ToggleDisplay(null, inputSource);
            }
        }

        /// <summary>
        /// Sets the visibility of the hand display.
        /// </summary>
        /// <param name="visibility">Whether the display will be visible or not</param>
        public void SetVisibility(bool visibility)
        {
            foreach (MeshRenderer mesh in gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                mesh.enabled = visibility;
            }
            foreach (TextMeshProUGUI text in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                text.enabled = visibility;
            }
            foreach (Light light in gameObject.GetComponentsInChildren<Light>())
            {
                light.enabled = visibility;
            }
        }

        /// <summary>
        /// Toggles the display when the button on the controller.
        /// </summary>
        /// <param name="fromAction">The action that triggers this function (the button click)</param>
        /// <param name="fromSource">The source for the action (the controller)</param>
        public void ToggleDisplay(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            visible = !visible;
            SetVisibility(visible);
            OnToggleEvent?.Invoke();
        }

        /// <summary>
        /// Shows display when targets are hit
        /// </summary>
        public void ShowDisplay()
        {
            visible = true;
            SetVisibility(visible);
        }

    }

}
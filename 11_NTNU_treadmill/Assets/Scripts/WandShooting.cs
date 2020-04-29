using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


namespace Wand
{
    /// <summary>
    /// A class for the wand that is set on the wand so that it is able to shoot.
    /// </summary>
    public class WandShooting : MonoBehaviour
    {
        /// <summary>
        /// The prefab of the projectile that is instansiated when you shoot.
        /// </summary>
        public GameObject projectile;

        /// <summary>
        /// The action that triggers the bullet to be shot.
        /// </summary>
        public SteamVR_Action_Boolean grabPinch;

        private readonly SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.RightHand;

        /// <summary>
        /// The force that the projectile get upon instansiation. It determines the projectile's speed.
        /// </summary>
        public float ejectionForce = 15.0f;

        /// <summary>
        /// The type of the <c>OnShootEvent</c>
        /// </summary>
        public delegate void OnShoot();

        /// <summary>
        /// Event that is triggered when the wand is shooting.
        /// </summary>
        public event OnShoot OnShootEvent;
        

        /// <summary>
        /// Creates a listener on the shooting button when the wand is enabled.
        /// </summary>
        private void OnEnable()
        {
            if (grabPinch != null)
            {
                grabPinch.AddOnStateDownListener(Shoot, inputSource);
            }
        }

        /// <summary>
        /// Removes the lister on the shooting button when the wand is disabled.
        /// </summary>
        private void OnDisable()
        {
            //grabPinch.RemoveListener();
            grabPinch.RemoveOnStateDownListener(Shoot, inputSource);
        }

        /// <summary>
        /// Checks whether space is pressed, and if it is you shoot. [for debugging purposes]
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot(null, inputSource);
            }
        }

        /// <summary>
        /// Is triggered when you are clicking on the controller button and shoots a magic bullet from the 
        /// .
        /// </summary>
        /// <param name="fromAction">The action that triggers this function (the button click)</param>
        /// <param name="fromSource">The source for the action (the controller)</param>
        private void Shoot(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
        {
            GameObject magicBullet = Instantiate(projectile, transform.position + transform.TransformVector(8f, 0, 0), transform.rotation) as GameObject;
            magicBullet.GetComponent<Rigidbody>().AddForce(transform.TransformVector(ejectionForce, 0, 0));

            OnShootEvent?.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonHit : MonoBehaviour
{

    public UnityEvent buttonPress = new UnityEvent();

    /// <summary>
    /// When a player presses the button, we invoke the buttonPress event assigned in the Questmanager script
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.tag == "handCollider")
        {
            buttonPress.Invoke();
        }
    }
}

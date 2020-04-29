using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOmniFeatures : MonoBehaviour
{
    /// <summary>
    /// The list of objects we wish to disable
    /// </summary>
    public GameObject[] objectsToDisable;

    /// <summary>
    /// The list of objects we wish to enable
    /// </summary>
    public GameObject[] objectsToEnable;

    /// <summary>
    /// Enables the objects we wish to enable and disables the objects we wish to disable
    /// </summary>
    void Awake()
    {
        foreach (GameObject obj in objectsToDisable)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
    }
}

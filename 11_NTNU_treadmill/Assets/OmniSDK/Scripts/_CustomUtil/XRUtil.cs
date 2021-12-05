using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

internal static class XRUtil
{
    // Code copied from https://docs.unity3d.com/2021.2/Documentation/ScriptReference/XR.XRDevice-isPresent.html
    public static bool XRDeviceIsPresent()
    {
        var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
        SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
        foreach (var xrDisplay in xrDisplaySubsystems)
        {
            if (xrDisplay.running)
            {
                return true;
            }
        }
        return false;
    }
}

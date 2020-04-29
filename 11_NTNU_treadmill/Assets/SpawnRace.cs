using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRace : MonoBehaviour
{
    public GameObject startLine;

    /// <summary>
    /// Spawns the race startline
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            startLine.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var playerScript = other.GetComponent<PlayerInteraction>();

        if(other.GetComponent<PlayerInteraction>() != null)
        {
            playerScript.Crash();
        }
    }
}

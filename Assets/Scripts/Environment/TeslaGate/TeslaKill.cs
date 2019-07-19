using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaKill : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Player")
        {
            other.transform.root.GetComponent<PlayerEvents>().killPlayer();
        }
        else if(other.gameObject.tag == "FriendlyNPC")
        {
            other.gameObject.GetComponent<ScriptedNPC>().kill();
        }
    }
}

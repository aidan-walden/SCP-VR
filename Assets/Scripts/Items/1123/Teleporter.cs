using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] protected Transform destination;

    protected virtual void OnTriggerEnter(Collider other)
    {  
        if(other.tag == "Player")
        {
            other.transform.position = destination.transform.position;
        }
    }
}

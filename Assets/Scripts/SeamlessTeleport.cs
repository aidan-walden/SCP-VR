using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeamlessTeleport : MonoBehaviour
{
    public Transform destination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherBody = other.GetComponent<Rigidbody>();
        Vector3 prevVelocity = otherBody.velocity;
        other.transform.position = destination.position;
        other.transform.rotation = Quaternion.Euler(-destination.right);
        otherBody.velocity = prevVelocity;
    }
}

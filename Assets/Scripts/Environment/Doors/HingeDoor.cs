using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeDoor : MonoBehaviour
{
    private Transform child;

    private void Awake()
    {
        child = transform.GetChild(0);
    }

    public void swapParents() //Makes the first child of the door the doors parent
    {
        transform.parent = child;
    }
}

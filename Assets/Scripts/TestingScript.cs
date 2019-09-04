using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScript : MonoBehaviour
{
    public GameObject otherObject;
    private void Start()
    {
        transform.rotation = Quaternion.identity;
        otherObject.transform.rotation = transform.rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotator : MonoBehaviour {
    public float waitTime = 0.1f;
	// Use this for initialization
	void Start () {
        StartCoroutine(rotateImage(waitTime));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator rotateImage(float waitTime)
    {
        while(true)
        {
            transform.Rotate(Vector3.forward);
            yield return new WaitForSeconds(waitTime);
        }
    }
}

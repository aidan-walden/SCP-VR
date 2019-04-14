using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Battery : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            Radio radioScript = collision.gameObject.GetComponent<Radio>();
            if (radioScript != null)
            {
                radioScript.batteryCharge += 15;
                Destroy(gameObject);
            }
        }
    }
}

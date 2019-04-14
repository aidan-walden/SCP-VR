using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGen : MonoBehaviour
{
    public string seed;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("THE NUMBER VALUE SEED FOR " + seed + " IS: " + seed.GetHashCode());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

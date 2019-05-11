using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedGen : MonoBehaviour
{
    public string seed;
    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(seed.GetHashCode());
        Debug.Log("THE NUMBER VALUE SEED FOR " + seed + " IS: " + seed.GetHashCode() + ". RANDOM NUMBER FOR THIS SEED: " + Random.Range(1, 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

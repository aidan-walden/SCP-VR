using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Items : MonoBehaviour
{
    [SerializeField] GameObject[] itemObjects;
    public Dictionary<string, GameObject> items;
    // Use this for initialization
    void Start()
    {
        items = new Dictionary<string, GameObject>();
        foreach(GameObject item in itemObjects)
        {
            items.Add(item.name, item);
        }
    }
}


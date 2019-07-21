using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScientistSpawn : MonoBehaviour
{
    [SerializeField] Transform otherSpawn;
    [SerializeField] GameObject scientist;
    public GameObject testingObj;
    private static bool sciSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if(!sciSpawned && other.transform.root.tag == "Player")
        {
            GameObject spawnedSci = Instantiate(scientist);
            spawnedSci.GetComponent<NavMeshAgent>().Warp(otherSpawn.position);
            spawnedSci.GetComponent<ScriptedNPC>().destination = transform.GetChild(0).transform;
            sciSpawned = true;
        }
    }
}

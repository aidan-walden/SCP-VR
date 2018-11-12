using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Collider))]
public class CompleteLinkImmediate : MonoBehaviour {
    bool enemyIsWalkingThru = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerStay(Collider other)
    {
        NavMeshAgent enemyNav = other.transform.root.GetComponent<NavMeshAgent>();
        AgentLinkMover mover = other.transform.root.GetComponent<AgentLinkMover>();
        if (enemyNav != null && enemyNav.isOnOffMeshLink && !enemyIsWalkingThru)
        {
            if(mover != null)
            {
                enemyIsWalkingThru = true;
                StartCoroutine(mover.MoveNavMesh());
            }
            else
            {
                enemyNav.CompleteOffMeshLink();
            }
        }
    }
}

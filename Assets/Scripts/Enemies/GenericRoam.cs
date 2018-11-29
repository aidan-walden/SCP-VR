using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GenericRoam : MonoBehaviour {
    public float roamingDestMaxDist = 20f;
    int destTries = 0;
    [SerializeField] Enemy enemy;
    NavMeshAgent enemyNav;
    bool shouldRoam = true;
	// Use this for initialization
	void Awake () {
        enemyNav = enemy.GetComponent<NavMeshAgent>();
        enemyNav.autoTraverseOffMeshLink = false;
	}

    private void Start()
    {
        chooseRoamingDest();
    }

    // Update is called once per frame
    void Update () {

        if(enemyNav.isOnOffMeshLink)
        {
            Debug.Log(enemyNav.currentOffMeshLinkData.linkType);
        }

        if(!enemyNav.pathPending && !enemy.playerTargeted && shouldRoam) //Check if enemy has reached dest
        {
            if(enemyNav.remainingDistance <= enemyNav.stoppingDistance)
            {
                if(!enemyNav.hasPath || enemyNav.velocity.sqrMagnitude == 0f)
                {
                    chooseRoamingDest();
                }
            }
        }
    }

    void chooseRoamingDest()
    {
        Vector3 raycastDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)); //Choose a random direction to go in
        RaycastHit dest;
        if (!Physics.Raycast(transform.position, raycastDir, out dest, roamingDestMaxDist)) //If the raycast didnt find any obstructions
        {
            Vector3 endPos = transform.position + raycastDir * roamingDestMaxDist;
            enemyNav.SetDestination(endPos);
        }
        else
        {
            Debug.Log("Could not find valid destination");
            destTries++;
            if(destTries <= 5)
            {
                chooseRoamingDest(); //Retry
            }
            else
            {
                destTries = 0;
                Invoke("chooseRoamingDest", 5f);
            }
        }
    }


    public void toggleRoaming(bool roam)
    {
        shouldRoam = roam;
        enemyNav.ResetPath();
    }
}

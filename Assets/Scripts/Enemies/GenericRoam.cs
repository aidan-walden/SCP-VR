using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GenericRoam : MonoBehaviour {
    public float roamingDestMaxDist = 20f;
    protected int destTries = 0;
    [SerializeField] protected Enemy enemy;
    protected NavMeshAgent enemyNav;
    protected bool shouldRoam = true;
    protected bool isChoosingDest = false;
    // Use this for initialization
    protected virtual void Awake () {
        enemyNav = enemy.GetComponent<NavMeshAgent>();
        enemyNav.autoTraverseOffMeshLink = false;
	}

    protected virtual void Start()
    {
        StartCoroutine(chooseRoamingDest());
    }

    // Update is called once per frame
    protected virtual void Update () {

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
                    if(!isChoosingDest)
                    {
                        StartCoroutine(chooseRoamingDest());
                    }
                }
            }
        }
    }

    protected virtual IEnumerator chooseRoamingDest()
    {
        isChoosingDest = true;
        Vector3 raycastDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)); //Choose a random direction to go in
        RaycastHit dest;
        if (!Physics.Raycast(transform.position, raycastDir, out dest, roamingDestMaxDist)) //If the raycast didnt find any obstructions
        {
            Vector3 endPos = transform.position + raycastDir * roamingDestMaxDist;
            enemyNav.SetDestination(endPos);
            isChoosingDest = false;
        }
        else
        {
            Debug.Log("Could not find valid destination. Dest Tries: " + destTries);
            if(destTries < 5)
            {
                destTries++;
                StartCoroutine(chooseRoamingDest()); //Retry
            }
            else
            {
                destTries = 0;
                Invoke("chooseRoamingDest", 5f);
            }
        }
        yield return null;
    }


    public virtual void toggleRoaming(bool roam)
    {
        shouldRoam = roam;
        enemyNav.ResetPath();
    }

    public Transform getFurthestNav(Transform[] positions)
    {
        Transform furthestNav = null;
        float minDist = 0;
        Vector3 currentPos = transform.position;
        foreach (Transform transform in positions)
        {
            float dist = Vector3.Distance(transform.position, currentPos);
            if (dist > minDist)
            {
                furthestNav = transform;
                minDist = dist;
            }
        }
        return furthestNav;
    }

    public Transform getClosestNav(Transform[] positions)
    {
        Transform closestNav = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform transform in positions)
        {
            float dist = Vector3.Distance(transform.position, currentPos);
            if (dist < minDist)
            {
                closestNav = transform;
                minDist = dist;
            }
        }
        return closestNav;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoctorRoam : GenericRoam {
    [SerializeField] SphereCollider doorDetector;
    DetectDoors detectDoors;
    [SerializeField] float doorDistance;
    public Transform furthestPoint;
    protected override void Start()
    {
        detectDoors = doorDetector.gameObject.GetComponent<DetectDoors>();
        StartCoroutine(chooseRoamingDest());
    }

    protected override IEnumerator chooseRoamingDest()
    {
        isChoosingDest = true;
        yield return new WaitForSeconds(1f);
        Transform[] closestDoors = detectDoors.doorsInRange.ToArray();
        for (int i = 0; i < 2; i++)
        {
            Transform closestDoor = getClosestDoor(closestDoors);
            SlidingDoor doorScript = closestDoor.gameObject.GetComponent<SlidingDoor>();
            //TODO: Set dest to door, go thru door, remove door from closest doors array, repeat 1 more time
            Debug.Log(closestDoor.name);
            OffMeshLink closestDoorLink = closestDoor.GetComponent<OffMeshLink>();
            Transform[] startAndEnd =  {closestDoorLink.startTransform, closestDoorLink.endTransform };
            furthestPoint = getFurthestNav(startAndEnd);
            enemyNav.SetDestination(getFurthestNav(startAndEnd).position);
            if (i == 0 && closestDoors.Length > 1)
            {
                bool enemyHasReachedDest = false;
                while (!enemyHasReachedDest)
                {
                    if (!enemyNav.pathPending)
                    {
                        if (enemyNav.remainingDistance <= enemyNav.stoppingDistance)
                        {
                            if (!enemyNav.hasPath || enemyNav.velocity.sqrMagnitude == 0f)
                            {
                                enemyHasReachedDest = true;
                            }
                        }
                    }
                    yield return null;
                }
                List<Transform> tempList = new List<Transform>(closestDoors);
                tempList.Remove(closestDoor);
                closestDoors = tempList.ToArray();
            }
        }
        isChoosingDest = false;
    }

    Transform getClosestDoor(Transform[] doors)
    {
        Transform closestDoor = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Transform transform in doors)
        {
            float dist = Vector3.Distance(transform.position, currentPos);
            if (dist < minDist)
            {
                closestDoor = transform;
                minDist = dist;
            }
        }
        return closestDoor;
    }

    Transform getFurthestNav(Transform[] positions)
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

}

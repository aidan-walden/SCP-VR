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

    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Doctor is on off mesh link: " + enemyNav.isOnOffMeshLink);
        }
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
            //Set dest to door, go thru door, remove door from closest doors array, repeat 1 more time
            Debug.Log(closestDoor.name);
            OffMeshLink closestDoorLink = closestDoor.GetComponent<OffMeshLink>();
            Transform[] startAndEnd =  {closestDoorLink.startTransform, closestDoorLink.endTransform };
            furthestPoint = getFurthestNav(startAndEnd);
            enemyNav.SetDestination(furthestPoint.position + (-furthestPoint.transform.right * 2));
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
                                Debug.Log("Doctor has reached dest");
                            }
                        }
                    }
                    yield return null;
                }
                while (enemyNav.isOnOffMeshLink)
                {
                    if(!enemyNav.isOnOffMeshLink)
                    {
                        Debug.Log("Doctor has stopped off mesh link");
                        break;
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

}

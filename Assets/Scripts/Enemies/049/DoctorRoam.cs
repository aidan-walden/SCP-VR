using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoctorRoam : GenericRoam {
    [SerializeField] SphereCollider doorDetector;
    DetectDoors detectDoors;
    [SerializeField] float doorDistance;
    List<Transform> prevDoors = new List<Transform>();
    public Transform furthestPoint;
    protected override void Start()
    {
        detectDoors = doorDetector.gameObject.GetComponent<DetectDoors>();
        StartCoroutine(chooseRoamingDest());
    }

    protected override void Update()
    {
        base.Update();

    }
    
    
    protected override IEnumerator chooseRoamingDest() //we must remove the previous 2 doors from possible doors when this function is called after the first time
    {
        isChoosingDest = true;
        yield return new WaitForSeconds(1f);
        List<Transform> closestDoors = new List<Transform>();
        List<Transform> doorsInRange = detectDoors.doorsInRange;
        foreach (Transform door in doorsInRange) //Cannot use previous doors
        {
            if (!prevDoors.Contains(door))
            {
                Debug.Log("ADDING " + door.gameObject.name);
                closestDoors.Add(door);
            }
        }
        closestDoors = sortByClosestFirst(closestDoors); //Sort doors in range by closest first to establish priority
        if(closestDoors.Count > 2)
        {
            closestDoors.RemoveRange(2, closestDoors.Count - 2); //Use only the 2 closest doors
        }
        foreach (Transform door in prevDoors)
        {
            Debug.Log("PREVIOUS DOOR: " + door.gameObject.GetInstanceID());
        }
        foreach (Transform door in doorsInRange)
        {
            Debug.Log("DOORSINRANGE: " + door.gameObject.GetInstanceID());
        }
        prevDoors = new List<Transform>();
        foreach (Transform door in closestDoors)
        {
            prevDoors.Add(door);
        }
        for (int i = 0; i < 2; i++)
        {
            Transform closestDoor = getClosestDoor(closestDoors);
            Debug.Log("CLOSEST DOOR: " + closestDoor.gameObject.name);
            SlidingDoor doorScript = closestDoor.gameObject.GetComponent<SlidingDoor>();
            //Set dest to door, go thru door, remove door from closest doors array, repeat 1 more time
            OffMeshLink closestDoorLink = closestDoor.GetComponent<OffMeshLink>();
            Transform[] startAndEnd =  {closestDoorLink.startTransform, closestDoorLink.endTransform };
            furthestPoint = getFurthestNav(startAndEnd);
            enemyNav.SetDestination(furthestPoint.position + (-furthestPoint.transform.right * 2));
            if (i == 0 && closestDoors.Count > 1)
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
                closestDoors.Remove(closestDoor);
            }
        }
        isChoosingDest = false;
    }
    

    Transform getClosestDoor(List<Transform> doors)
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

    List<Transform> sortByClosestFirst(List<Transform> objects)
    {
        List<Vector3> objectPos = new List<Vector3>();
        foreach(Transform thing in objects)
        {
            objectPos.Add(thing.position);
        }
        objectPos.Sort((x, y) => { return (transform.position - x).sqrMagnitude.CompareTo((transform.position - y).sqrMagnitude); });
        List<Transform> sortedObjects = new List<Transform>();
        foreach(Vector3 pos in objectPos)
        {
            foreach(Transform thing in objects)
            {
                if(pos == thing.position)
                {
                    sortedObjects.Add(thing);
                }
            }
        }
        return sortedObjects;
    }

}

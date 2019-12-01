using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGen : MonoBehaviour
{
    public Room startingRoomPrefab;
    public List<Room> rooms = new List<Room>();
    public List<Room> roomsLeft = new List<Room>();
    public List<Room> generatedRooms = new List<Room>();
    public Transform enviornment;
    public string seed;
    private void Start()
    {
        roomsLeft = rooms;
        Room startRoom = Instantiate(startingRoomPrefab);
        startRoom.transform.position = Vector3.zero; //Place starting room in center of map
        startRoom.transform.rotation = Quaternion.identity;
        startRoom.transform.SetParent(enviornment);
        UnityEngine.Random.InitState(seed.GetHashCode());
        StartCoroutine(generateRooms(startRoom));
    }

    IEnumerator generateRooms(Room prevRoom)
    {
        if(prevRoom.doorsRemaining > 0)
        {
            Room newRoom = null;
            foreach (GameObject door in prevRoom.doors)
            {
                Room room = roomsLeft[UnityEngine.Random.Range(0, roomsLeft.Count)];
                int commonness = 0;
                if (room.commonness == 0 || room.commonness == 100)
                {
                    commonness = 45;
                }
                else
                {
                    commonness = room.commonness;
                }
                int decidedRandom = UnityEngine.Random.Range(1, 46);
                //if (decidedRandom <= commonness) //Random.Range with integers is exclusive (this means max is 99), this is intentional.
                if (true)
                {
                    //Create room here
                    GameObject tempParent = new GameObject("tempParent");
                    newRoom = Instantiate(room);
                    newRoom.transform.position = prevRoom.transform.forward * 200;
                    //TODO: Change origin of new room to be on the edge, in the doorway closest to the old room, then move the room to the old rooms door + a few inches
                    GameObject closestDoor = null;
                    float minDist = Mathf.Infinity;
                    Vector3 currentPos = door.transform.position;
                    if (room.doorsRemaining > 1) //No need to find closest door if there is only one option
                    {
                        foreach (GameObject newDoor in newRoom.doors) //Find closest door spot
                        {
                            float dist = Vector3.Distance(transform.position, currentPos);
                            if (dist < minDist)
                            {
                                closestDoor = newDoor;
                                minDist = dist;
                            }
                        }
                    }
                    else
                    {
                        closestDoor = newRoom.doors[0];
                    }
                    tempParent.transform.position = closestDoor.transform.position; //Move tempParent gameobject to edge of room
                    newRoom.transform.SetParent(tempParent.transform);
                    //tempParent.transform.rotation = Quaternion.identity;
                    Debug.Log("About to generate new room " + newRoom.gameObject.name + ". Attaching to room " + prevRoom.name + ". Door rotation is: " + door.transform.rotation.eulerAngles.y + " Math.Floor'd: " + (float)Math.Floor(door.transform.rotation.eulerAngles.y));
                    if(door.transform.rotation.eulerAngles.y < 0f) //Negative numbers exist
                    {
                        tempParent.transform.rotation = Quaternion.Euler(0f, (float)Math.Ceiling(door.transform.rotation.eulerAngles.y), 0f); //Align new room to match rotation of door leading to it, use Math.Ceiling to fix floating point error
                    }
                    else
                    {
                        tempParent.transform.rotation = Quaternion.Euler(0f, (float)Math.Floor(door.transform.rotation.eulerAngles.y), 0f); //Align new room to match rotation of door leading to it, use Math.Floor to fix floating point error
                    }
                    tempParent.transform.position = door.transform.position; //New room should now be perfectly aligned
                    Destroy(door);
                    placeDoors(newRoom, closestDoor);
                    newRoom.transform.SetParent(enviornment);
                    Destroy(tempParent);
                }
            }
            yield return null;
            Debug.Log("Restarting room generate coroutine with the new room " + newRoom.roomName);
            StartCoroutine(generateRooms(newRoom));
        }
    }

    void reshuffle(List<Room> list)
    {
        Room[] array = list.ToArray();
        //Knuth shuffle algorithm
        for (int t = 0; t < array.Length; t++)
        {
            Room tmp = array[t];
            int r = UnityEngine.Random.Range(t, array.Length);
            array[t] = array[r];
            array[r] = tmp;
        }
        list = new List<Room>(array);
    }

    void placeDoors(Room room, GameObject door)
    {
        if(room.doorsRemaining > 0)
        {
            bool doorExisting = false;
            Collider[] hitColliders = Physics.OverlapSphere(door.transform.position, 0.1f); //Check if a door is already present where we want to place one
            foreach (Collider collide in hitColliders)
            {
                try
                {
                    if (collide.transform.parent.transform.parent.tag == "Door")
                    {
                        Debug.Log("FOUND ALREADY EXISTING DOOR WHILE ATTEMPTING TO PLACE DOOR FOR ROOM: " + room.gameObject.name);
                        room.doorsRemaining -= 1;
                        doorExisting = true;
                        break;
                    }
                }
                catch (NullReferenceException) //If the object we hit in the OverlapShpere has no parent object, or its parent object has no parent object (it is not a door), the script will throw an exception and stop us from creating our door. This catch statement prevents that.
                {
                }

            }
            if (!doorExisting)
            {
                GameObject newDoor = Instantiate(room.doorPrefab);
                newDoor.transform.position = door.transform.position;
                newDoor.transform.rotation = Quaternion.Euler(-90f, door.transform.rotation.y, 90f + door.transform.rotation.z);
                newDoor.transform.SetParent(room.transform);
                Debug.Log("Deleting door " + door.name + " in room " + room.roomName);
                room.doors.Remove(door); //Remove door from list before deleting it to prevent error when we get to the new room later
                Destroy(door);
                room.doorsRemaining -= 1;
            }
            if (room.commonness != 100)
            {
                roomsLeft.Remove(room);
            }
            bool containsRoom = false;
            foreach(Room oldRoom in generatedRooms)
            {
                if(oldRoom.Equals(room))
                {
                    containsRoom = true;
                    break;
                }
            }
            if(!containsRoom)
            {
                generatedRooms.Add(room);
            }
        }
    }
}

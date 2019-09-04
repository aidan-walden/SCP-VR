using UnityEngine;
using System.Collections.Generic;

public class Room : MonoBehaviour
{
    public string roomName, desc;
    public int commonness; //Commonness is out of 100
    public bool overlapCheck, decals = true;
    public bool large = false; //Make large true if the room has a non-standard shape, and as a result must go outside the boundaries for the assigned shape. This does not apply to exit rooms, which should always be set to false, because the additional parts are so far away from the main section of the room that it doesn't make a difference.
    public int zone1, zone2 = 0; //1 for LCZ, 2 FOR HCZ, 3 for EZ
    public int doorsRemaining; //This should initially be set to the amount of entrances the room has.
    public List<GameObject> doors = new List<GameObject>();
    public GameObject doorPrefab;

    public bool Equals(Room room)
    {
        if(roomName == room.roomName)
        {
            return true;
        }
        return false;
    }
}

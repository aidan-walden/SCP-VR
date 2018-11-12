using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]
public class DoorButton : MonoBehaviour
{

    private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);

    private SlidingDoor doorScript;
    public DualSlidingDoor dualDoorScript;
    public SlidingDoor door;
    private bool doorIsOpen, isDualDoor;
    // Use this for initialization
    void Start()
    {
        dualDoorScript = GetComponentInParent<DualSlidingDoor>();
        isDualDoor = dualDoorScript != null;
        if(isDualDoor)
        {
            doorIsOpen = dualDoorScript.DoorIsOpen;
        }
        else
        {
            doorIsOpen = door.doorStartsOpen;
        }
    }

    public void changeDoorState()
    {
        if(isDualDoor)
        { 
            dualDoorScript.moveDoor(!doorIsOpen);
        }
        else
        {
            door.moveDoor(!doorIsOpen);
        }
        doorIsOpen = !doorIsOpen;
    }
}

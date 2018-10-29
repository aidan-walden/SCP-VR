using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class DoorButton : MonoBehaviour
    {

        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);

        private SlidingDoor doorScript;
        private DualSlidingDoor dualDoorScript;
        public SlidingDoor door;
        private bool doorIsOpen;
        // Use this for initialization
        void Start()
        {
            dualDoorScript = GetComponentInParent<DualSlidingDoor>();
            if(dualDoorScript != null)
            {
                doorIsOpen = dualDoorScript.doorIsOpen;
            }
            else
            {
                doorIsOpen = door.doorStartsOpen;
            }
        }

        public void changeDoorState()
        {
            if(dualDoorScript != null)
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
}

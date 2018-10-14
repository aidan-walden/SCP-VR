using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class DoorButton : MonoBehaviour
    {

        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers);

        private Interactable interactable;
        public GameObject door;
        public bool isOpen = false;
        private bool doorIsOpening = false;
        private SlidingDoor doorScript;
        // Use this for initialization
        void Start()
        {
            doorScript = door.GetComponent<SlidingDoor>();
            interactable = this.gameObject.GetComponent<Interactable>();
        }

        // Update is called once per frame
        void Update()
        {
            if (interactable.attachedToHand)
            {
                Debug.Log("Changing door");
                changeDoorState();
            }
        }

        void onAttachedToHand(Hand hand)
        {
            
        }

        public void changeDoorState()
        {
            Debug.Log("about to open door");
            if (!doorIsOpening)
            {
                Debug.Log("Starting coroutine...");
                StartCoroutine("doChangeDoor");
            }
        }

        IEnumerator doChangeDoor()
        {
            Debug.Log("Cycling through door sounds...");
            doorIsOpening = true;
            isOpen = !isOpen;
            doorScript.moveDoor(isOpen);
            yield return new WaitForSeconds(doorScript.doorSounds.clip.length);
            doorIsOpening = false;
        }
    }
}

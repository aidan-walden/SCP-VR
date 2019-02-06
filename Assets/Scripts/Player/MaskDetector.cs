using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Valve.VR.InteractionSystem
{
    public class MaskDetector : MonoBehaviour
    {
        Interactable interactable;
        Mask maskScript;
        Hand maskHand;
        GameObject mask;
        [SerializeField] private float maskOffset = 2f;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.tag == "Mask")
            {
                mask = other.gameObject;
                interactable = mask.GetComponent<Interactable>();
                if (interactable.attachedToHand)
                {
                    Debug.Log("Mask in range");
                    maskScript = mask.GetComponent<Mask>();
                    maskHand = maskScript.maskHand;
                    if (interactable != null)
                    {
                        mask.transform.position = transform.position + (transform.forward / maskOffset);
                        Debug.Log("User detached");
                        mask.transform.parent = transform;
                        mask.GetComponent<Rigidbody>().isKinematic = true;
                        Debug.Log("Parenting mask to player");
                    }
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            for(int i = 0; i < 25; i++)
            {
                Debug.Log("MASKDETECTOR ONTRIGGEREXIT");
            }
            if(interactable.attachedToHand && other.transform.root.tag == "Player" && other.gameObject.GetComponent<Hand>() != null)
            {
                Debug.Log("Hand Leaving Mask Detector with mask");
                Rigidbody rg = mask.GetComponent<Rigidbody>();
                other.gameObject.transform.parent = null;
                rg.isKinematic = false;
                Debug.Log("Deparenting...");
                mask = null;
            }
            else if(!interactable.attachedToHand && other.transform.root.tag == "Player" && other.gameObject.GetComponent<Hand>() != null)
            {
                Debug.Log("Player hand left the trigger but was not attached to mask");
            }
            else
            {
                Debug.Log("Something left the trigger but did not meet conditions");
            }
        }
    }
}

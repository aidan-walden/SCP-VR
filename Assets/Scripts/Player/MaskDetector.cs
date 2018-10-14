using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Valve.VR.InteractionSystem
{
    public class MaskDetector : MonoBehaviour
    {
        private bool maskAttached = false;
        public Interactable interactable;
        [SerializeField] private float maskOffset = 2f;

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == "Mask" && !maskAttached)
            {
                Debug.Log("Mask in range");
                interactable = other.gameObject.GetComponent<Interactable>();
                if(!interactable.attachedToHand)
                {
                    Debug.Log("User detached");
                    other.gameObject.transform.position = transform.position + (transform.forward / maskOffset);
                    other.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    other.gameObject.transform.parent = transform;
                    maskAttached = true;
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            Rigidbody rg = other.gameObject.GetComponent<Rigidbody>();
            if(rg != null)
            {
                rg.constraints = RigidbodyConstraints.None;
                other.gameObject.transform.parent = null;
                maskAttached = false;
            }
            

        }
    }
}

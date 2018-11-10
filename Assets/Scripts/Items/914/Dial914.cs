using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class Dial914 : MonoBehaviour
    {

        Hand playerHand;
        bool isHeld, rotateToOrig = false;
        public bool isKey = false;
        Quaternion initialGrabRotation;
        public float rotateSpeed;
        Quaternion rotateTo;
        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame

        private void Update()
        {
            if (isHeld)
            {
                Debug.Log(playerHand.transform.rotation.z);
                transform.rotation = Quaternion.Euler(0, 0, (transform.rotation * (playerHand.transform.rotation * Quaternion.Inverse(initialGrabRotation))).z);
                if (isKey)
                {
                    if(transform.rotation.z == 90)
                    {
                        playerHand.DetachObject(this.gameObject);
                        rotateToOrig = true;
                    }
                }
            }
            if (rotateToOrig)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, rotateTo, Time.deltaTime * rotateSpeed);
                if (transform.rotation == rotateTo)
                {
                    rotateToOrig = false;
                }
            }
        }

        protected virtual void OnAttachedToHand(Hand hand)
        {
            playerHand = hand;
            initialGrabRotation = hand.transform.rotation; //Get rotation of hand in Vector3 form
            isHeld = true;
        }

        protected virtual void OnDetachedFromHand(Hand hand)
        {
            isHeld = false;
            if (!isKey)
            {
                if(transform.rotation.z > 90)
                {
                    rotateDial(90);
                }
                else if(transform.rotation.z < -90)
                {
                    rotateDial(-90);
                }
            }
        }

        void rotateDial(float rotation)
        {
            rotateTo = Quaternion.Euler(0, 0, rotation);
            rotateToOrig = true;
        }
    }
}

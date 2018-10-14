using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    public class PlayerMovement : MonoBehaviour
    {
        public SteamVR_Action_Boolean isForward;
        public SteamVR_Action_Boolean isBack;
        public SteamVR_Action_Boolean isLeft;
        public SteamVR_Action_Boolean isRight;

        public float moveVertical;
        public float moveHorizontal;

        public Hand hand;

        // Use this for initialization
        void Start()
        {

        }

        private void OnEnable()
        {
            if (hand == null)
                hand = this.GetComponent<Hand>();

            if (isForward == null)
            {
                Debug.LogError("No forward action assigned");
                return;
            }
            if(isBack == null)
            {
                Debug.LogError("No back action assigned");
            }
            if(isLeft == null)
            {
                Debug.LogError("No left action assigned");
            }
            if(isRight == null)
            {
                Debug.LogError("No right action assigned");
            }

            isForward.AddOnChangeListener(OnForwardActionChange, hand.handType);
            isBack.AddOnChangeListener(OnBackActionChange, hand.handType);
            isLeft.AddOnChangeListener(OnLeftActionChange, hand.handType);
            isRight.AddOnChangeListener(OnRightActionChange, hand.handType);
        }


        private void OnForwardActionChange(SteamVR_Action_In actionIn)
        {
            if (isForward.GetStateDown(hand.handType))
            {
                Debug.Log("Forward down");
                moveVertical += 1f;
            }
            if (isForward.GetStateUp(hand.handType))
            {
                Debug.Log("Forward up");
                moveVertical -= 1f;
            }
        }
        private void OnBackActionChange(SteamVR_Action_In actionIn)
        {
            if (isBack.GetStateDown(hand.handType))
            {
                Debug.Log("Back down");
                moveVertical -= 1f;
            }
            if (isBack.GetStateUp(hand.handType))
            {
                Debug.Log("Forward up");
                moveVertical += 1f;
            }
        }
        private void OnLeftActionChange(SteamVR_Action_In actionIn)
        {
            if (isLeft.GetStateDown(hand.handType))
            {
                Debug.Log("Left down");
                moveHorizontal -= 1f;
            }
            if (isLeft.GetStateUp(hand.handType))
            {
                Debug.Log("Left up");
                moveHorizontal += 1f;
            }
        }
        private void OnRightActionChange(SteamVR_Action_In actionIn)
        {
            if (isRight.GetStateDown(hand.handType))
            {
                Debug.Log("Right down");
                moveHorizontal += 1f;
            }
            if (isRight.GetStateUp(hand.handType))
            {
                Debug.Log("Right up");
                moveHorizontal -= 1f;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

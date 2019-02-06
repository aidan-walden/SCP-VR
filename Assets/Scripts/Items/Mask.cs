using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]
public class Mask : MonoBehaviour {
    public Hand maskHand;
    //TODO: Use hand to assume location of mask. When hand enters trigger, snap mask to face and make hand invisible but keep track of hand. When hand exits, detach the mask and bring it with.
    protected virtual void OnAttachedToHand(Hand hand)
    {
        maskHand = hand;
    }

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        maskHand = null;
    }
}

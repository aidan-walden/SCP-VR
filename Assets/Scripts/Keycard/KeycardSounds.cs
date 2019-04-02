using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]
public class KeycardSounds : MonoBehaviour
{
    public AudioClip keycardPickup;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void OnAttachedToHand(Hand hand)
    {
        Debug.Log("Playing sound");

        hand.transform.root.GetComponent<PlayerEvents>().playSound(keycardPickup);
    }
}

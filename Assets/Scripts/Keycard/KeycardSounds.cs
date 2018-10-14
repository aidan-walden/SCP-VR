using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(Interactable))]
    public class KeycardSounds : MonoBehaviour
    {
        public AudioSource playerAudio;
        public AudioClip keycardPickup;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void onAttachedToHand(Hand hand)
        {
            Debug.Log("Playing sound");
            playerAudio.clip = keycardPickup;
            playerAudio.Play();
        }
    }
}

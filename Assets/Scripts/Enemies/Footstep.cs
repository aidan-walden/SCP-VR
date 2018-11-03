using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footstep : MonoBehaviour {
    public AudioSource footstepsSounds;
    public AudioClip[] footsteps;
    public int footstepCount = 0;

    void FootstepEvent(float volume)
    {
        footstepsSounds.clip = footsteps[footstepCount];
        footstepsSounds.volume = volume;
        footstepsSounds.Play();
        if(footstepCount == footsteps.Length - 1)
        {
            footstepCount = 0;
        }
        else
        {
            footstepCount++;
        }
    }
}

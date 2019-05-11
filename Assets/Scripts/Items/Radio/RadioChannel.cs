using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[System.Serializable]
public class RadioChannel {
    public bool isPlaying = false;
    public int ChannelNum
    {
        get
        {
            return channelNum;
        }
    }

    public float WaitBwClipsDuration
    {
        get
        {
            return waitBwClipsDuration;
        }
    }

    public int channelNum;
    public int currentTrack = 0;
    public float waitBwClipsDuration;
    public AudioClip[] messages;
    public bool isCommsChannel;
    public AudioSource channelSounds;
    public AudioClip idleSound;
    public bool commsActive = false;

    public RadioChannel(int channelNum, float waitBwClipsDuration, AudioClip idleSound, bool isCommsChannel = false)
    {
        this.channelNum = channelNum;
        this.waitBwClipsDuration = waitBwClipsDuration;
        this.isCommsChannel = isCommsChannel;
        this.idleSound = idleSound;
    }

    public IEnumerator startComms(AudioClip comm)
    {
        if(isCommsChannel)
        {
            yield return new WaitForSeconds(channelSounds.clip.length - channelSounds.time + WaitBwClipsDuration);
            commsActive = true;
            channelSounds.loop = false;
            channelSounds.clip = comm;
            channelSounds.Play();
            yield return new WaitForSeconds(comm.length);
            commsActive = false;
        }
        else
        {
            Debug.LogWarning("Start Comms called on non-comms channel. Ignoring.");
        }
    }
}

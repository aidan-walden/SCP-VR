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

    /* 
     public RadioChannel(int channelNum, float waitBwClipsDuration, AudioClip[] channelSounds)
     {
         this.channelNum = channelNum;
         this.waitBwClipsDuration = waitBwClipsDuration;
         Array.Copy(this.channelSounds, channelSounds, this.channelSounds.Length);
     }
     */

    //ADDING THE ARRAY TO THE CONSTRUCTOR BREAKS EVERYTHING??

    public RadioChannel(int channelNum, float waitBwClipsDuration)
    {
        Debug.Log("this.channelNum: " + this.channelNum);
        Debug.Log("channelNum: " + channelNum);
        this.channelNum = channelNum;
        Debug.Log("this.channelNum after: " + this.channelNum);
        Debug.Log("channelNum after: " + channelNum);
        this.waitBwClipsDuration = waitBwClipsDuration;
        
        //Array.Copy(channelSounds, this.channelSounds, channelSounds.Length);
    }
    


}

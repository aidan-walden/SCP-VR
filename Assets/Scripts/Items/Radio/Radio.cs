using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;


[System.Serializable]
public class AudioClipList
{
    public AudioClip[] clips;
}


[RequireComponent(typeof(Throwable))]
public class Radio : MonoBehaviour {
    public AudioMixer mixer;
    public RadioChannel[] radioChannels = new RadioChannel[3];
    public Text radioText;
    public static CustomSongs customSongs;
    public bool radioIsPlaying, playerIsHolding = false;
    private int currentChannel = 1;
    public int batteryCharge = 15;
    public int CurrentChannel
    {
        get
        {
            return currentChannel;
        }
        set
        {
            if(playerIsHolding)
            {
                if (value == radioChannels.Length + 1 || value < 1)
                {
                    currentChannel = 1;
                }
                else
                {
                    currentChannel = value;
                }
                radioText.text = currentChannel.ToString();
            }
        }
    }

    public AudioClipList[] channelSounds;

    [SerializeField] AudioSource activeSource;
    [SerializeField] AudioClip radioStatic;
    
    // Use this for initialization
    void Awake () {
        if(customSongs == null)
        {
            customSongs = GameObject.FindWithTag("GameController").GetComponent<CustomSongs>();
        }
        mixer = (AudioMixer)Resources.Load("AudioMixer");
        //radioChannels[0] = new RadioChannel(1, 0.2f, channelSounds[0].clips);
        //radioChannels[1] = new RadioChannel(2, 0.2f, channelSounds[1].clips);
        radioChannels[0] = new RadioChannel(1, 0.2f, radioStatic);
        radioChannels[1] = new RadioChannel(2, 0.2f, radioStatic);
        radioChannels[2] = new RadioChannel(3, 0.2f, radioStatic);
        radioChannels[3] = new RadioChannel(4, 0.2f, radioStatic, true);
        radioChannels[4] = new RadioChannel(5, 0.2f, radioStatic, true);
        radioChannels[1].messages = channelSounds[1].clips;
        radioChannels[2].messages = channelSounds[2].clips;
        radioChannels[3].messages = channelSounds[3].clips;
        radioChannels[4].messages = channelSounds[4].clips;
        radioText.text = currentChannel.ToString();
        StartCoroutine(setSounds());
    }

    private void Start()
    {
        
    }

    IEnumerator setSounds()
    {
        yield return new WaitUntil(() => customSongs.AudioIsLoaded == true);
        radioChannels[0].messages = customSongs.getAudioClips();
        if(radioChannels[0].messages.Length == 0)
        {
            radioChannels[0].messages[0] = radioChannels[0].idleSound;
            radioChannels[0].waitBwClipsDuration = 0f;
        }
        foreach (RadioChannel channel in radioChannels)
        {
            channel.channelSounds = this.gameObject.AddComponent<AudioSource>();
            channel.channelSounds.playOnAwake = false;
            channel.channelSounds.spatialBlend = 1f;
            channel.channelSounds.rolloffMode = AudioRolloffMode.Logarithmic;
            channel.channelSounds.minDistance = 0.2f;
            channel.channelSounds.outputAudioMixerGroup = mixer.FindMatchingGroups("Music")[0];
            if (channel.isCommsChannel)
            {
                channel.channelSounds.clip = channel.idleSound;
                channel.waitBwClipsDuration = 0f;
            }
            else
            {
                channel.channelSounds.clip = channel.messages[0];
            }
        }
    }

    // Update is called once per frame
    void Update () {
    }

    public void testChange()
    {
        StartCoroutine(radioChannels[3].startComms(radioChannels[3].messages[0]));

    }
    protected virtual void OnAttachedToHand(Hand hand)
    {
        ChannelSwitcher switcher = hand.GetComponent<ChannelSwitcher>();
        if (switcher != null)
        {
            switcher.radio = this;
        }
        playerIsHolding = true;
        if(batteryCharge > 0)
        {
            changeChannel();
        }
    }

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        StartCoroutine(stopRadio());
    }

    public void changeChannel()
    {
        foreach (RadioChannel channel in radioChannels)
        {
            if (channel.ChannelNum == CurrentChannel)
            {
                channel.channelSounds.mute = false;
                if(channel.isCommsChannel)
                {
                    if(!channel.commsActive)
                    {
                        channel.channelSounds.loop = true;
                    }
                }
                else
                {
                    if (channel.messages.Length == 1)
                    {
                        channel.channelSounds.loop = true;
                    }
                    else
                    {
                        channel.channelSounds.loop = false;
                    }
                }
            }
            else
            {
                channel.channelSounds.mute = true;
            }
            if(!channel.channelSounds.isPlaying)
            {
                channel.channelSounds.Play();
            }
        }
        if(!radioIsPlaying)
        {
            radioIsPlaying = true;
            foreach (RadioChannel channel in radioChannels)
            {
               StartCoroutine(cycleClips(channel));
            }
        }
        if (batteryCharge > 0)
        {
            batteryCharge--;
        }
    }

    IEnumerator cycleClips(RadioChannel channel)
    {
        if (radioIsPlaying)
        {
            StartCoroutine(degradeBattery());
            yield return new WaitForSeconds((channel.channelSounds.clip.length - channel.channelSounds.time) + channel.WaitBwClipsDuration);
            if(radioIsPlaying)
            {
                if(!channel.isCommsChannel)
                {
                    if (channel.currentTrack == channel.messages.Length - 1)
                    {
                        channel.currentTrack = 0;
                    }
                    else
                    {
                        channel.currentTrack++;
                    }
                    channel.channelSounds.clip = channel.messages[channel.currentTrack];
                    channel.channelSounds.Play();
                }
                else
                {
                    if(!channel.commsActive)
                    {
                        if(channel.channelSounds.clip != channel.idleSound)
                        {
                            channel.channelSounds.clip = channel.idleSound;
                        }
                        channel.channelSounds.Play();
                    }
                }
                StartCoroutine(cycleClips(channel));
            }
        }
    }

    IEnumerator stopRadio()
    {
        playerIsHolding = false;
        yield return new WaitForSeconds(radioChannels[currentChannel - 1].channelSounds.clip.length - radioChannels[currentChannel - 1].channelSounds.time);
        if(!playerIsHolding)
        {
            radioIsPlaying = false;
            StopCoroutine(degradeBattery());
            foreach(RadioChannel channel in radioChannels)
            {
                channel.channelSounds.Pause();
            }
        }
    }

    IEnumerator degradeBattery()
    {
        while(true)
        {
            yield return new WaitForSeconds(30f);
            batteryCharge--;
        }   
    }
}

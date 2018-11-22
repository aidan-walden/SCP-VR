using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;


[System.Serializable]
public class AudioClipList
{
    public AudioClip[] clips;
}


[RequireComponent(typeof(Throwable))]
public class Radio : MonoBehaviour {
    public RadioChannel[] radioChannels = new RadioChannel[3];
    public Text radioText;
    public CustomSongs customSongs;

    public bool radioIsPlaying, playerIsHolding = false;
    private int currentChannel = 1;
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
    List<AudioSource> channelSources = new List<AudioSource>();
    
    // Use this for initialization
    void Awake () {
        //radioChannels[0] = new RadioChannel(1, 0.2f, channelSounds[0].clips);
        //radioChannels[1] = new RadioChannel(2, 0.2f, channelSounds[1].clips);
        radioChannels[0] = new RadioChannel(1, 0.2f);
        radioChannels[1] = new RadioChannel(2, 0.2f);
        radioChannels[2] = new RadioChannel(3, 0.2f);
        radioChannels[1].messages = channelSounds[1].clips;
        radioChannels[2].messages = channelSounds[2].clips;
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
            AudioClip[] tempList = { radioStatic };
            radioChannels[0].messages = tempList;
            radioChannels[0].waitBwClipsDuration = 0f;
        }
        foreach (RadioChannel channel in radioChannels)
        {
            AudioSource channelSounds = this.gameObject.AddComponent<AudioSource>();
            channelSounds.playOnAwake = false;
            channelSounds.spatialBlend = 1f;
            channelSounds.rolloffMode = AudioRolloffMode.Logarithmic;
            channelSounds.minDistance = 0.2f;
            channelSounds.clip = channel.messages[0];
            channelSources.Add(channelSounds);
        }
    }

    // Update is called once per frame
    void Update () {
    }

    protected virtual void OnAttachedToHand(Hand hand)
    {
        ChannelSwitcher switcher = hand.GetComponent<ChannelSwitcher>();
        if (switcher != null)
        {
            switcher.radio = this;
        }
        playerIsHolding = true;
        changeChannel();
    }

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        StartCoroutine(stopRadio());
    }

    public void changeChannel()
    {
        foreach(AudioSource source in channelSources)
        {
            if(channelSources.IndexOf(source) == CurrentChannel - 1)
            {
                source.mute = false;
                activeSource = source;
                if(radioChannels[CurrentChannel - 1].messages.Length == 1)
                {
                    source.loop = true;
                }
                else
                {
                    source.loop = false;
                }
            }
            else
            {
                source.mute = true;
            }
            if(!source.isPlaying)
            {
                source.Play();
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
    }

    IEnumerator cycleClips(RadioChannel channel)
    {
        AudioSource channelSource = channelSources[channel.channelNum - 1];
        if (radioIsPlaying)
        {
            yield return new WaitForSeconds((channelSource.clip.length - channelSource.time) + channel.WaitBwClipsDuration);
            if(radioIsPlaying)
            {
                if (channel.currentTrack == channel.messages.Length - 1)
                {
                    channel.currentTrack = 0;
                }
                else
                {
                    channel.currentTrack++;
                }
                channelSource.clip = channel.messages[channel.currentTrack];
                channelSource.Play();
                StartCoroutine(cycleClips(channel));
            }
        }
    }

    IEnumerator stopRadio()
    {
        playerIsHolding = false;
        yield return new WaitForSeconds(activeSource.clip.length - activeSource.time);
        if(!playerIsHolding)
        {
            radioIsPlaying = false;
            foreach(AudioSource source in channelSources)
            {
                source.Pause();
            }
        }
    }
}

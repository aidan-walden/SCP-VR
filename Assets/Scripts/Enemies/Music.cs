using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
    public AudioClip chaseMusic;
    public AudioClip killSound;
    bool chaseMusicActive = false;
    AudioSource[] playerAudio;
    AudioSource playerMusic, playerSounds;
    // Use this for initialization
    void Start () {
        playerAudio = GetComponentInParent<TargetPlayer>().player.transform.root.GetComponents<AudioSource>(); //Get the HeadCollider's master object's AudioSource components using the Player parameter from TargetPlayer
        playerMusic = playerAudio[0];
        playerSounds = playerAudio[1];
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void toggleChaseMusic()
    {
        chaseMusicActive = !chaseMusicActive;
        if(chaseMusicActive)
        {
            playerMusic.clip = chaseMusic;
            playerMusic.loop = true;
            playerMusic.Play();
        }
        else
        {
            playerMusic.loop = false;
            playerMusic.Stop();
            playerMusic.clip = null;
        }
        
    }

    public void playKillSound()
    {
        playerSounds.clip = killSound;
        playerSounds.Play();
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
    public AudioClip chaseMusic;
    public AudioClip killSound;
    bool chaseMusicActive = false;
    AudioSource[] playerAudio;
    AudioSource playerMusic;
    static GameObject player;
    PlayerEvents playerEvents;
    // Use this for initialization
    void Start () {
        player = Enemy.player;
        playerAudio = player.GetComponents<AudioSource>();
        playerMusic = playerAudio[0];
        playerEvents = player.GetComponent<PlayerEvents>();
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
        playerEvents.playSound(killSound);
    }

   

    
}

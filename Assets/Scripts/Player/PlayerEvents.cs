using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEvents : MonoBehaviour {
    [SerializeField] private bool godMode = false;
    public bool playerIsDead = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void killPlayer()
    {
        if(!godMode && !playerIsDead)
        {
            playerIsDead = true;
            GetComponentInChildren<Camera>().enabled = false;
            Invoke("loadGame", 3f);
        }
    }

    private void loadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public IEnumerator playSound(AudioClip sound)
    {
        AudioSource audioSource = this.gameObject.AddComponent<AudioSource>(); //Create a new audio source for every sound so that way we don't have to worry about swapping out sounds at correct times
        audioSource.clip = sound;
        //TODO: Set volume to that of user preferences when they are implimented
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(audioSource); //Destory the audio source after the sound is done playing so that the components of the player don't get cluttered

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEvents : MonoBehaviour {
    private bool godMode = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void killPlayer()
    {
        if(!godMode)
        {
            Camera VRCam = GetComponentInChildren<Camera>();
            VRCam.enabled = false;
            Invoke("loadGame", 3f);
        }
    }

    private void loadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

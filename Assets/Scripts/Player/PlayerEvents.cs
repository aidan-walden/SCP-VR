using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEvents : MonoBehaviour {
    [SerializeField] private bool godMode = false;
    [SerializeField] Image blinkOverlay;
    [SerializeField] float blinkSmooth;
    [SerializeField] AudioSource playerSounds;
    public bool playerIsDead, playerIsBlinking = false;
    public bool GodMode
    {
        get
        {
            return godMode;
        }
    }
    private bool ringOn = false;
    public bool RingOn
    {
        get
        {
            return ringOn;
        }
        set
        {
            godMode = value;
            ringOn = value;
        }
    }
    public float blinkDur = 0.3f;
	// Use this for initialization
	void Start () {
        RingOn = true;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(blink());
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(blinkOverlay.color.a);
        }
    }

    public void killPlayer()
    {
        if(!godMode && !playerIsDead)
        {
            playerIsDead = true;
            //TODO: Proper death screen
            GetComponentInChildren<Camera>().enabled = false;
            Invoke("loadGame", 5f);
        }
    }

    private void loadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void playSound(AudioClip sound)
    {
        playerSounds.PlayOneShot(sound, 1f);
    }

    IEnumerator blinkOn()
    {
        while(blinkOverlay.color.a < 1f)
        {
            Debug.Log("Blink overlay opacity: " + blinkOverlay.color.a);
            float newOpacity = Mathf.Lerp(blinkOverlay.color.a, 1f, Time.deltaTime * blinkSmooth);
            if (newOpacity >= 0.99)
            {
                Debug.Log("Close enough");
                newOpacity = 1f;
            }
            blinkOverlay.color = new Color(0f, 0f, 0f, newOpacity);
            yield return null;
        }
        Debug.Log("player is blinking");
        playerIsBlinking = true;
        yield return null;
    }

    IEnumerator blinkOff()
    {
        playerIsBlinking = false;
        while (blinkOverlay.color.a > 0f)
        {
            Debug.Log("Blink overlay opacity: " + blinkOverlay.color.a);
            float newOpacity = Mathf.Lerp(blinkOverlay.color.a, 0f, Time.deltaTime * blinkSmooth);
            if (newOpacity <= 0.01)
            {
                newOpacity = 0f;
            }
            blinkOverlay.color = new Color(0f, 0f, 0f, newOpacity);
            yield return null;
        }
        Debug.Log("player is not blinking");
        yield return null;
    }

    IEnumerator blink()
    {
        StartCoroutine(blinkOn());
        yield return new WaitUntil(() => playerIsBlinking);
        Invoke("startBlinkOff", blinkDur);
    }

    void startBlinkOff()
    {
        StartCoroutine(blinkOff());
    }
}

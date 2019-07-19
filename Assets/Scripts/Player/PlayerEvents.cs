using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEvents : MonoBehaviour {
    [SerializeField] private bool godMode = false;
    [SerializeField] Image blinkOverlay;
    [SerializeField] float blinkSmooth;
    AudioSource playerSounds;
    [SerializeField] AudioSource[] intercomSounds;
    [SerializeField] AudioClip intercomStart, intercomEnd, testSound;
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

    private void Awake()
    {
        playerSounds = GetComponent<AudioSource>();
    }

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
            playSound(testSound, true);
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

    public void playSound(AudioClip sound, bool useIntercom = false)
    {
        if (useIntercom)
        {
            StartCoroutine(playIntercom(sound));
        }
        else
        {
            playerSounds.PlayOneShot(sound, 1f);
        }
    }

    IEnumerator playIntercom(AudioClip sound)
    {
        intercomSounds[0].PlayOneShot(intercomStart, 1f);
        yield return new WaitForSeconds(intercomStart.length);
        intercomSounds[1].PlayOneShot(sound, 1f);
        yield return new WaitForSeconds(sound.length + 1.5f);
        intercomSounds[0].PlayOneShot(intercomEnd, 1f);
    }

    IEnumerator blinkOn()
    {
        while(blinkOverlay.color.a < 1f)
        {
            float newOpacity = Mathf.Lerp(blinkOverlay.color.a, 1f, Time.deltaTime * blinkSmooth);
            if (newOpacity >= 0.99)
            {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShyGuyTrigger : MonoBehaviour {
    public AudioClip startRage, rageActive, roaming;
    public float rageEnterBuffer = 3f;
    public float rageDuration = 12f;
    public float rageCooldown = 10f;

    private bool isRaging = false;
    private AudioSource shyGuySounds;
    private ShyGuyAttack attackScript;
	// Use this for initialization
	void Start () {
        shyGuySounds = GetComponentInChildren<AudioSource>();
        attackScript = GetComponent<ShyGuyAttack>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void rageMode(bool enteringRage = true)
    {
        if(!isRaging)
        {
            StartCoroutine(enterRage());
        }
    }

    private IEnumerator enterRage()
    {
        Debug.Log("096 is coming for you");
        isRaging = true;
        shyGuySounds.Stop();
        shyGuySounds.loop = false;
        shyGuySounds.clip = startRage;
        shyGuySounds.Play();
        yield return new WaitForSeconds(rageEnterBuffer);
        shyGuySounds.Stop();
        shyGuySounds.clip = rageActive;
        shyGuySounds.loop = true;
        shyGuySounds.Play();
        attackScript.toggleAttack(true);
        yield return new WaitForSeconds(rageDuration);
        StartCoroutine(stopRage());
        
    }

    private IEnumerator stopRage()
    {
        shyGuySounds.Stop();
        shyGuySounds.loop = true;
        shyGuySounds.clip = roaming;
        shyGuySounds.Play();
        attackScript.toggleAttack(false);
        yield return new WaitForSeconds(rageCooldown);
        isRaging = false;
    }
}

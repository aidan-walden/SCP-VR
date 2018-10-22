using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShyGuyTrigger : MonoBehaviour {
    public AudioClip startRage, rageActive, roaming;
    public float rageEnterBuffer = 3f;
    public float rageDuration = 12f;
    public float rageCooldown = 10f;
    public float rageSpeed = 8f;
    public float roamSpeed = 1f;

    private bool isRaging = false;
    private AudioSource shyGuySounds;
    private TargetPlayer attackScript;
    private Animator shyGuyAnims;
    private NavMeshAgent shyGuy;
    private GenericRoam roamingScript;
	// Use this for initialization
	void Start () {
        shyGuySounds = GetComponentInChildren<AudioSource>();
        attackScript = GetComponentInParent<TargetPlayer>();
        shyGuyAnims = GetComponent<Animator>();
        shyGuy = GetComponent<NavMeshAgent>();
        roamingScript = GetComponent<GenericRoam>();
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
        roamingScript.toggleRoaming(false);
        shyGuy.speed = rageSpeed;
        Debug.Log("096 is coming for you");
        isRaging = true;
        shyGuyAnims.StopPlayback();
        shyGuyAnims.SetBool("isRoaming", false);
        shyGuyAnims.SetBool("isEnteringRage", true);
        shyGuySounds.Stop();
        shyGuySounds.loop = false;
        shyGuySounds.clip = startRage;
        shyGuySounds.Play();
        yield return new WaitForSeconds(rageEnterBuffer);
        shyGuySounds.Stop();
        shyGuySounds.clip = rageActive;
        shyGuySounds.loop = true;
        shyGuySounds.Play();
        shyGuyAnims.SetBool("isEnteringRage", false);
        shyGuyAnims.SetBool("isRaging", true);
        attackScript.targetPlayer(shyGuy, true);
        yield return new WaitForSeconds(rageDuration);
        StartCoroutine(stopRage());
        
    }

    private IEnumerator stopRage()
    {
        shyGuy.speed = roamSpeed;
        shyGuySounds.Stop();
        shyGuySounds.loop = true;
        shyGuySounds.clip = roaming;
        shyGuySounds.Play();
        shyGuyAnims.StopPlayback();
        shyGuyAnims.SetBool("isRaging", false);
        shyGuyAnims.SetBool("isRoaming", true);
        attackScript.targetPlayer(shyGuy, false);
        roamingScript.toggleRoaming(true);
        yield return new WaitForSeconds(rageCooldown);
        isRaging = false;
    }

    public bool getIsRaging()
    {
        return isRaging;
    }
}

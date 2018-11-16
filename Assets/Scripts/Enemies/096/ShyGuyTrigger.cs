using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(GenericRoam))]
[RequireComponent(typeof(SphereCollider))]
public class ShyGuyTrigger : Enemy {
    public AudioClip startRage, rageActive, roaming;
    public float rageEnterBuffer = 3f;
    public float rageDuration = 12f;
    public float rageCooldown = 10f;
    public float rageSpeed = 8f;
    public float roamSpeed = 1f;

    private bool isRaging = false;
    public bool IsRaging
    {
        get
        {
            return isRaging;
        }
    }
    [SerializeField] GenericRoam roamingScript;
    [SerializeField] SphereCollider raycastFinder;
	// Use this for initialization
	void Start () {
        //StartCoroutine(enterRage());
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
        raycastFinder.enabled = false;
        roamingScript.toggleRoaming(false);
        enemyNav.speed = rageSpeed;
        Debug.Log("096 is coming for you");
        isRaging = true;
        enemyAnims.StopPlayback();
        enemyAnims.SetBool("isRoaming", false);
        enemyAnims.SetBool("isEnteringRage", true);
        enemySounds.Stop();
        enemySounds.loop = false;
        enemySounds.clip = startRage;
        enemySounds.Play();
        yield return new WaitForSeconds(rageEnterBuffer);
        enemySounds.Stop();
        enemySounds.clip = rageActive;
        enemySounds.loop = true;
        enemySounds.Play();
        enemyAnims.SetBool("isEnteringRage", false);
        enemyAnims.SetBool("isRaging", true);
        targetPlayer(true);
        yield return new WaitForSeconds(rageDuration);
        StartCoroutine(stopRage());
        
    }

    private IEnumerator stopRage()
    {
        raycastFinder.enabled = true;
        enemyNav.speed = roamSpeed;
        enemySounds.Stop();
        enemySounds.loop = true;
        enemySounds.clip = roaming;
        enemySounds.Play();
        enemyAnims.StopPlayback();
        enemyAnims.SetBool("isRaging", false);
        enemyAnims.SetBool("isRoaming", true);
        targetPlayer(false);
        roamingScript.toggleRoaming(true);
        yield return new WaitForSeconds(rageCooldown);
        isRaging = false;
    }
}

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
    public bool IsRaging
    {
        get
        {
            return isRaging;
        }
    }
    [SerializeField] AudioSource shyGuySounds;
    [SerializeField] TargetPlayer attackScript;
    [SerializeField] Animator shyGuyAnims;
    [SerializeField] NavMeshAgent shyGuy;
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
        raycastFinder.enabled = true;
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

    private void OnTriggerEnter(Collider collision) //Kill player if the "hitbox" collides with him/her
    {
        if (attackScript.PlayerTargeted && collision.gameObject.transform.root.name == "Player")
        {
            collision.gameObject.transform.root.GetComponent<PlayerEvents>().killPlayer();
        }
    }
}

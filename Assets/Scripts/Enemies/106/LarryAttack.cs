using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LarryAttack : MonoBehaviour {
    [SerializeField] Animator larryAnims;
    float floorRiseTime, idleTime, grabTime, ceilingWalkTime, ceilingRiseTime, ceilingGrabTime, walkTime;
    [SerializeField] private float larryTimer;
    [SerializeField] TargetPlayer targetPlayer;
    [SerializeField] NavMeshAgent larryNav;
    [SerializeField] AudioClip riseFromGround, laugh;
    [SerializeField] AudioSource larryBase, larryFace;
    GameObject mucus;
    [SerializeField] GameObject mucusPrefab;
    [SerializeField] Vector3 newMucusScale;
    public bool growMucus = false;
	// Use this for initialization
	void Start () {
        larryTimer = Random.Range(300, 600);
        updateAnimTimes();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L))
        {
            startAttack(targetPlayer.player.transform);
        }
        if(!targetPlayer.PlayerTargeted)
        {
            larryTimer -= Time.deltaTime;
        }
        if (larryTimer <= 0)
        {
            startAttack(targetPlayer.player.transform);
            larryTimer = Random.Range(300, 600);
        }
        if(growMucus)
        {
            mucus.transform.localScale = Vector3.Lerp(mucus.transform.localScale, newMucusScale, Time.deltaTime);
            if (mucus.transform.localScale == newMucusScale)
            {
                growMucus = false;
            }
        }
    }

    void startAttack(Transform victimTransform, bool comeFromCeiling = false)
    {
        Vector3 larryTeleport = victimTransform.position;
        StartCoroutine(emergeFromGround(victimTransform, larryTeleport));
    }

    private IEnumerator emergeFromGround(Transform victimTransform, Vector3 teleportTo)
    {
        larryNav.Warp(teleportTo);
        transform.rotation = Quaternion.Inverse(victimTransform.rotation);
        larryAnims.SetBool("isIdle", false);
        larryAnims.SetBool("isComingFromGround", true);
        createMucus();
        larryBase.clip = riseFromGround;
        larryBase.Play();
        larryFace.clip = laugh;
        larryFace.Play();
        yield return new WaitForSeconds(floorRiseTime);
        larryAnims.SetBool("isComingFromGround", false);
        larryAnims.SetBool("isWalking", true);
        targetPlayer.targetPlayer(larryNav, true);
    }

    private void createMucus()
    {
        mucus = Instantiate(mucusPrefab);
        Debug.Log("X: " + transform.position.x + " Y: " + transform.position.y + " Z: " + transform.position.z);
        mucus.transform.parent = null;
        mucus.transform.position = new Vector3(transform.position.x, larryNav.height / transform.localScale.y, transform.position.z);
        mucus.transform.rotation = Quaternion.identity;
        growMucus = true;
    }

    public void playerLost()
    {
        targetPlayer.targetPlayer(larryNav, false);
        larryAnims.SetBool("isWalking", false);
        larryAnims.SetBool("isIdle", true);
        larryNav.Warp(transform.position - (transform.up * 200));
    }

    private void updateAnimTimes()
    {
        AnimationClip[] clips = larryAnims.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "idle":
                    idleTime = clip.length;
                    break;
                case "ground_rise":
                    floorRiseTime = clip.length;
                    break;
                case "grab":
                    grabTime = clip.length;
                    break;
                case "walk":
                    walkTime = clip.length;
                    break;
                case "ceiling_walk":
                    ceilingWalkTime = clip.length;
                    break;
                case "ceiling_grab":
                    ceilingGrabTime = clip.length;
                    break;
                case "ceiling_rise":
                    ceilingRiseTime = clip.length;
                    break;  
            }
        }
    }

}

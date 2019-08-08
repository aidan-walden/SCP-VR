using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LarryAttack : Enemy {
    
    float floorRiseTime, idleTime, grabTime, ceilingWalkTime, ceilingRiseTime, ceilingGrabTime, walkTime;
    [SerializeField] private float larryTimer;
    [SerializeField] AudioClip riseFromGround, laugh;
    [SerializeField] AudioSource larryBase;
    GameObject mucus;
    [SerializeField] GameObject mucusPrefab;
    [SerializeField] Vector3 newMucusScale;
    public bool growMucus = false;
	// Use this for initialization
	protected override void Awake () {
        base.Awake();
        lookForPlayer = false;
        larryTimer = Random.Range(300, 600);
        updateAnimTimes();
    }
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
		if(Input.GetKeyDown(KeyCode.L))
        {
            startAttack(player.transform);
        }
        if(!playerTargeted)
        {
            larryTimer -= Time.deltaTime;
        }
        if (larryTimer <= 0)
        {
            startAttack(player.transform);
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
        enemyNav.Warp(teleportTo);
        transform.rotation = Quaternion.Inverse(victimTransform.rotation);
        enemyAnims.SetBool("isIdle", false);
        enemyAnims.SetBool("isComingFromGround", true);
        createMucus();
        larryBase.clip = riseFromGround;
        larryBase.Play();
        enemySounds.clip = laugh;
        enemySounds.Play();
        yield return new WaitForSeconds(floorRiseTime);
        enemyAnims.SetBool("isComingFromGround", false);
        enemyAnims.SetBool("isWalking", true);
        OnPlayerSpotted();
    }

    private void createMucus()
    {
        //TODO: Use Object Pooling instead of Instantiate
        mucus = Instantiate(mucusPrefab);
        Debug.Log("X: " + transform.position.x + " Y: " + transform.position.y + " Z: " + transform.position.z);
        mucus.transform.parent = null;
        mucus.transform.position = new Vector3(transform.position.x, enemyNav.height / transform.localScale.y, transform.position.z);
        mucus.transform.rotation = Quaternion.identity;
        growMucus = true;
    }

    protected override void OnPlayerLost()
    {
        targetPlayer(false);
        enemyAnims.SetBool("isWalking", false);
        enemyAnims.SetBool("isIdle", true);
        enemyNav.Warp(transform.position - (transform.up * 200));
    }

    private void updateAnimTimes()
    {
        AnimationClip[] clips = enemyAnims.runtimeAnimatorController.animationClips;
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

    /*public void bringPlayerToPocket()
    {
        player.transform.root.position += Vector3.up * 20;
        OnPlayerLost();
    }*/

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorAttack : Enemy {

    [SerializeField] AudioClip[] enemyChasingLines, enemyLostLines;
    [SerializeField] AudioClip ringDetected;
    [SerializeField] float armReachRange;
    bool victimHasRingOn = false;

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Doctor is on off mesh link: " + enemyNav.isOnOffMeshLink);
        }
        if (playerTargeted)
        {
            if (armReachRange > 0)
            {
                enemyAnims.SetBool("isIdle", false);
                enemyAnims.SetBool("isWalking", true);
                if (sqrDist < armReachRange * armReachRange && !enemyAnims.GetBool("isInReachRange"))
                {
                    enemyAnims.ResetTrigger("stopReachRange");
                    raiseArmWhileWalking();
                }
                else if (sqrDist > armReachRange * armReachRange && enemyAnims.GetBool("isInReachRange"))
                {
                    enemyAnims.ResetTrigger("startReachRange");
                    lowerArmWhileWalking();
                }
            }
        }
        if(enemyNav.velocity.magnitude > 0)
        {
            setIdle(0);
            setWalk(1);
        }
        else
        {
            setWalk(0);
            enemyAnims.SetTrigger("stopWalk");
        }
    }

    protected override void OnPlayerSpotted()
    {
        startWalk();
        base.OnPlayerSpotted();
        StartCoroutine(chaseVoice());

    }

    void startWalk()
    {
        enemyAnims.SetBool("isIdle", false);
        enemyAnims.SetBool("isWalking", true);
    }

    void raiseArmWhileWalking()
    {
        enemyAnims.SetBool("isInReachRange", true);
        enemyAnims.SetTrigger("startReachRange");
    }

    void lowerArmWhileWalking()
    {
        enemyAnims.SetTrigger("stopReachRange");
        enemyAnims.SetBool("isInReachRange", false);
        enemyAnims.SetBool("isWalking", true);
    }

    void stopWalk()
    {
        enemyAnims.SetTrigger("stopWalk");
    }

    protected override void OnPlayerLost()
    {
        base.OnPlayerLost();
        stopWalk();
        StartCoroutine(searchVoice());
    }

    protected override void OnPlayerAttacked()
    {
        base.OnPlayerAttacked();
        if(playerScript.playerIsDead)
        {
            enemyAnims.SetBool("isWalking", false);
            enemyAnims.SetTrigger("stopReachRange");
            enemyAnims.SetTrigger("stopWalk");
            enemyAnims.SetTrigger("playerKilled");
        }else
        {
            if(playerScript.RingOn)
            {
                if(!victimHasRingOn)
                {
                    StartCoroutine(ringVoice());
                }
            }
        }

    }

    public void setWalk(int doWalk)
    {
        doWalk = Mathf.Clamp(doWalk, 0, 1);
        enemyAnims.SetBool("isWalking", doWalk != 0);
    }

    public void setIdle(int doIdle)
    {
        doIdle = Mathf.Clamp(doIdle, 0, 1);
        if(doIdle == 1)
        {
            if (!playerTargeted)
            {
                enemyAnims.SetBool("isIdle", true);
            }
        }
        else
        {
            enemyAnims.SetBool("isIdle", false);
        }
    }

    IEnumerator chaseVoice()
    {
        while(playerTargeted)
        {
            yield return new WaitForSeconds(Random.Range(2f, 6f));
            if(!enemySounds.isPlaying)
            {
                enemySounds.clip = enemyChasingLines[Random.Range(0, enemyChasingLines.Length - 1)];
                enemySounds.Play();
                yield return new WaitForSeconds(enemySounds.clip.length);
            }
            yield return null;
        }
    }

    IEnumerator searchVoice()
    {
        int playerSearchedTimes = 0;
        while(!playerTargeted && playerSearchedTimes <= 5)
        {
            yield return new WaitForSeconds(Random.Range(5f, 12f));
            if (!enemySounds.isPlaying)
            {
                enemySounds.clip = enemyLostLines[Random.Range(0, enemyLostLines.Length - 1)];
                enemySounds.Play();
                yield return new WaitForSeconds(enemySounds.clip.length);
                playerSearchedTimes++;
            }
            yield return null;
        }
    }

    IEnumerator ringVoice()
    {
        victimHasRingOn = true;
        while (!playerScript.playerIsDead && playerTargeted && playerScript.RingOn)
        {
            yield return new WaitForSeconds(Random.Range(1f, 4f));
            if (!enemySounds.isPlaying)
            {
                enemySounds.clip = ringDetected;
                enemySounds.Play();
                yield return new WaitForSeconds(enemySounds.clip.length);
            }
            yield return null;
        }
    }
}

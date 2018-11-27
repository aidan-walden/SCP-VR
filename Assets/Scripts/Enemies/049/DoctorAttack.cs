using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoctorAttack : Enemy {

    [SerializeField] float armReachRange;
    protected override void OnPlayerSpotted()
    {
        startWalk();
        base.OnPlayerSpotted();
        
    }

    protected override void Update()
    {
        base.Update();

        if(playerTargeted && armReachRange > 0 )
        {
            enemyAnims.SetBool("isIdle", false);
            enemyAnims.SetBool("isWalking", true);
            if(sqrDist < armReachRange * armReachRange && !enemyAnims.GetBool("isInReachRange"))
            {
                enemyAnims.ResetTrigger("stopReachRange");
                raiseArmWhileWalking();
            }
            else if(sqrDist > armReachRange * armReachRange && enemyAnims.GetBool("isInReachRange"))
            {
                enemyAnims.ResetTrigger("startReachRange");
                lowerArnWhileWalking();
            }
        }
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

    void lowerArnWhileWalking()
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
    }

    protected override void OnPlayerAttacked()
    {
        base.OnPlayerAttacked();
        enemyAnims.SetBool("isWalking", false);
        enemyAnims.SetTrigger("stopReachRange");
        enemyAnims.SetTrigger("stopWalk");
        enemyAnims.SetTrigger("playerKilled");

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
}

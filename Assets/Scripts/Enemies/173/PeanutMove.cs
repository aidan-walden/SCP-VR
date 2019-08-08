using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using AidanTools;

[RequireComponent(typeof(NavMeshAgent))]
public class PeanutMove : Enemy {
    Camera playerCam;
    AidanTools.AidanTools tools;
    public Renderer peanutRender;
    bool PlayerIsBlinking
    {
        get
        {
            return playerScript.playerIsBlinking;
        }
        set
        {
            if(!value)
            {
                enemyNav.ResetPath();
            }
            playerScript.playerIsBlinking = value;
        }
    }
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        tools = new AidanTools.AidanTools();
        playerCam = player.transform.GetChild(0).GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    protected override void Update () {
        PlayerIsBlinking = playerScript.playerIsBlinking;
        Vector3 relativePos;
        if (enemyChasesPlayer)
        {
            relativePos = player.transform.position - transform.position;
            sqrDist = relativePos.sqrMagnitude;
        }
        if (playerTargeted && (PlayerIsBlinking || !playerCanSee())) //Constantly update destination to change with the players position
        {
            if (enemyRange > 0 && sqrDist > enemyRange * enemyRange)
            {
                OnPlayerLost();
            }
            else
            {
                enemyNav.SetDestination(player.transform.position);
            }
        }
        else if (lookForPlayer)
        {
            if (enemyRange > 0 && sqrDist < enemyRange * enemyRange)
            {
                OnPlayerSpotted();
            }
        }
    }

    bool playerCanSee()
    {
        if(peanutRender.isVisible)
        {
            if (tools.objectIsVisible(playerCam, this.gameObject))
            {
                return true;
            }
        }
        return false;
    }
}

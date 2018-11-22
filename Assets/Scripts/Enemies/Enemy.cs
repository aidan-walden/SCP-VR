using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {
    [SerializeField] protected Animator enemyAnims;
    public bool playerTargeted = false;
    public bool enemyChasesPlayer = true;
    protected bool lookForPlayer = true;
    public float enemyRange;
    protected float sqrDist;
    [SerializeField] protected GameObject player;
    [SerializeField] protected NavMeshAgent enemyNav;
    [SerializeField] protected AudioSource enemySounds;
    PlayerEvents playerScript;

    protected virtual void Start()
    {
        enemyNav.autoTraverseOffMeshLink = false;
        playerScript = player.transform.root.GetComponent<PlayerEvents>();
    }

    protected virtual void Update()
    {
        Vector3 relativePos;
        if(enemyChasesPlayer)
        {
            relativePos = player.transform.position - transform.position;
            sqrDist = relativePos.sqrMagnitude;
        }
        if (playerTargeted) //Constantly update destination to change with the players position
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

    protected virtual void OnPlayerLost()
    {
        targetPlayer(false);
        lookForPlayer = true;
    }

    protected virtual void OnPlayerSpotted()
    {
        lookForPlayer = false;
        targetPlayer(true);
    }

    protected virtual void OnPlayerAttacked()
    {
        
        if(!playerScript.GodMode)
        {
            playerScript.killPlayer();
        }
    }

    protected virtual void targetPlayer(bool goAfterPlayer = true) //Stop current destination and enter the update loop
    {
        Debug.Log(enemyNav.name + ", " + goAfterPlayer);
        enemyNav.ResetPath();
        playerTargeted = goAfterPlayer;
        if (!goAfterPlayer)
        {
            Debug.Log("Clearing dest" + ", " + enemyNav.name);
            enemyNav.ResetPath();

        }
        enemyNav.gameObject.GetComponent<Music>().toggleChaseMusic();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (playerTargeted && other.gameObject.transform.root.name == "Player")
        {
            OnPlayerAttacked();
        }
    }

    protected IEnumerator WaitForAnimation(Animation animation)
    {
        do
        {
            yield return null;
        } while (animation.isPlaying);
    }
}

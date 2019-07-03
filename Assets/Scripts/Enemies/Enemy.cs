using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentLinkMover))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour {
    [SerializeField] protected Animator enemyAnims;
    public bool playerTargeted = false;
    public bool enemyChasesPlayer = true;
    protected bool lookForPlayer = true;
    public float enemyRange;
    protected float sqrDist;
    public static GameObject player;
    [SerializeField] protected GameObject forceDestination;
    [SerializeField] protected NavMeshAgent enemyNav;
    [SerializeField] protected AudioSource enemySounds;
    protected static PlayerEvents playerScript;
    [SerializeField] protected GenericRoam enemyRoam;



    protected virtual void Start()
    {
        enemyNav.autoTraverseOffMeshLink = false;
        player = GameObject.FindGameObjectWithTag("Player");
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
                if(enemyRange > 0)
                {
                    NavMeshPath path = new NavMeshPath();
                    if (!enemyNav.CalculatePath(player.transform.position, path) || enemyNav.pathStatus == NavMeshPathStatus.PathPartial) //This works
                    {                    
                        OnPlayerLost();
                        enemyChasesPlayer = false;
                    }
                    else
                    {
                        if (enemyChasesPlayer)
                        {
                            enemyNav.SetDestination(player.transform.position);
                        }
                    }
                }
                else
                {
                    if(enemyChasesPlayer)
                    {
                        enemyNav.SetDestination(player.transform.position);
                    }
                }
            }
        }
        else if (lookForPlayer)
        {
            NavMeshPath path = new NavMeshPath();
            if (enemyChasesPlayer && enemyRange > 0 && sqrDist < enemyRange * enemyRange && enemyNav.CalculatePath(player.transform.position, path))
            {
                OnPlayerSpotted();
            }
        }
    }

    protected virtual void OnPlayerLost()
    {
        Debug.Log("ON PLAYER LOST. ENEMY : " + gameObject.name);
        if (enemyRoam != null)
        {
            enemyRoam.toggleRoaming(true);
        }
        targetPlayer(false);
        lookForPlayer = true;
    }

    protected virtual void OnPlayerSpotted()
    {
        enemyNav.isStopped = true;
        if (enemyRoam != null)
        {
            enemyRoam.toggleRoaming(false);
            enemyNav.ResetPath();
            lookForPlayer = false;
        }
        enemyNav.isStopped = false;
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
        enemyNav.ResetPath();
        playerTargeted = goAfterPlayer;
        //enemyRoam.toggleRoaming(!goAfterPlayer);
        if (!goAfterPlayer)
        {
            enemyNav.ResetPath();
        }
        Music enemyMusic = enemyNav.gameObject.GetComponent<Music>();
        if(enemyMusic != null)
        {
            enemyMusic.toggleChaseMusic();
        }
        
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

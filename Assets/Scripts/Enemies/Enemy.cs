using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour {
    [SerializeField] protected Animator enemyAnims;
    public bool playerIsInRange, playerTargeted = false;
    public float enemyRange;
    [SerializeField] protected GameObject player;
    [SerializeField] protected NavMeshAgent enemyNav;
    [SerializeField] protected AudioSource enemySounds;
    // Use this for initialization
    void Start () {
		
	}

    void FixedUpdate()
    {
        if (playerTargeted) //Constantly update destination to change with the players position
        {
            if (enemyRange > 0 && Vector3.Distance(player.transform.position, transform.position) > enemyRange)
            {
                OnPlayerLost();
            }
            enemyNav.SetDestination(player.transform.position);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    protected virtual void OnPlayerLost()
    {
        targetPlayer(false);
    }

    protected virtual void OnPlayerAttacked()
    {
        player.transform.root.GetComponent<PlayerEvents>().killPlayer();
    }

    protected virtual void targetPlayer(bool goAfterPlayer = true) //Stop current destination and enter the update loop
    {
        Debug.Log(enemyNav.name + ", " + goAfterPlayer);
        enemyNav.ResetPath();
        playerTargeted = goAfterPlayer;
        if (!goAfterPlayer)
        {
            Debug.Log("Clearing dest" + ", " + enemyNav.name);
            enemyNav.isStopped = true;
            enemyNav.ResetPath();
            enemyNav.isStopped = false;

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
}

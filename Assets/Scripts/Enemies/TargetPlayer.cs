using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class TargetPlayer : MonoBehaviour {
    bool playerTargeted, playerIsInRange = false;
    public bool PlayerTargeted
    {
        get
        {
            return playerTargeted;
        }
    }
    public float enemyRange;
    NavMeshAgent enemy;
    public GameObject player;
    public UnityEvent playerLost;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(playerTargeted) //Constantly update destination to change with the players position
        {
            if(Vector3.Distance(player.transform.position, transform.position) > enemyRange)
            {
                OnPlayerLost();
            }
            enemy.SetDestination(player.transform.position);
        }
	}

    public void targetPlayer(NavMeshAgent nav, bool goAfterPlayer = true) //Stop current destination and enter the update loop
    {
        enemy = nav;
        Debug.Log(enemy.name + ", " + goAfterPlayer);
        enemy.SetDestination(enemy.transform.position);
        playerTargeted = goAfterPlayer;
        if(!goAfterPlayer)
        {
            Debug.Log("Clearing dest" + ", " + nav.name);
            nav.isStopped = true;
            nav.ResetPath();
            nav.isStopped = false;

        }
        enemy.gameObject.GetComponent<Music>().toggleChaseMusic();
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerTargeted && other.gameObject.transform.root.name == "Player")
        {
            PlayerEvents playerEvents = other.gameObject.transform.root.GetComponent<PlayerEvents>();
            if(!playerEvents.playerIsDead)
            {
                playerEvents.killPlayer();
            }
        }
    }

    private void OnPlayerLost()
    {
        playerLost.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetPlayer : MonoBehaviour {
    bool playerTargeted = false;
    public bool PlayerTargeted
    {
        get
        {
            return playerTargeted;
        }
    }
    NavMeshAgent enemy;
    public GameObject player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(playerTargeted) //Constantly update destination to change with the players position
        {
            enemy.SetDestination(player.transform.position);
        }
	}

    public void targetPlayer(NavMeshAgent nav, bool goAfterPlayer = true) //Stop current destination and enter the update loop
    {
        enemy = nav;
        enemy.SetDestination(enemy.transform.position);
        playerTargeted = goAfterPlayer;
        enemy.gameObject.GetComponent<Music>().toggleChaseMusic();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerTargeted && other.gameObject.transform.root.name == "Player")
        {
            other.gameObject.transform.root.GetComponent<PlayerEvents>().killPlayer();
        }
    }
}

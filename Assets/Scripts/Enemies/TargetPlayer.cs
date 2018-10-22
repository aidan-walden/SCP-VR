using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TargetPlayer : MonoBehaviour {
    bool playerTargeted = false;
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
    }

    private void OnTriggerEnter(Collider collision) //Kill player if the "hitbox" collides with him/her
    {
        if (playerTargeted && collision.gameObject.transform.root.name == "Player")
        {
            collision.gameObject.transform.root.GetComponent<PlayerEvents>().killPlayer();
        }
    }

    public bool getPlayerTargeted()
    {
        return playerTargeted;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShyGuyAttack : MonoBehaviour {
    private bool isAttacking = false;
    [SerializeField] private RaycastInit lookingScript;
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private TargetPlayer targetPlayer;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        if(isAttacking)
        {
            navAgent.SetDestination(lookingScript.headCollider.transform.position);
        }
    }

    public void toggleAttack(bool shouldAttack = true)
    {
        isAttacking = shouldAttack;
        if(!isAttacking)
        {
            
            navAgent.SetDestination(transform.position);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);
        Debug.Log("Collider root: " + collision.gameObject.transform.root.name);
        if(isAttacking && collision.gameObject.transform.root.name == "Player")
        {
            collision.gameObject.transform.root.GetComponent<PlayerEvents>().killPlayer();
        }
    }
}

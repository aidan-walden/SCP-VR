using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentLinkMover))]
[RequireComponent(typeof(Rigidbody))]
public class ScriptedNPC : MonoBehaviour
{
    public Transform destination;
    protected NavMeshAgent wanderNav;
    protected Animator wanderAnims;
    protected AudioSource wanderSounds;
    protected void Awake()
    {
        wanderNav = GetComponent<NavMeshAgent>();
        wanderAnims = GetComponent<Animator>();
        wanderSounds = GetComponent<AudioSource>();
        //wanderNav.updateUpAxis = false;
        //wanderNav.updateRotation = false;
    }

    private void Start()
    {
        startRun();
    }

    public void kill()
    {
        wanderNav.isStopped = true;
        wanderAnims.SetBool("isDead", true);
        Destroy(wanderSounds);
        CapsuleCollider coll = GetComponent<CapsuleCollider>();
        coll.center = new Vector3(coll.center.x, coll.center.y + 500, coll.center.z); //Trigger ontriggerexit for any triggers we are in when the NPC dies
    }

    public void startRun()
    {
        wanderAnims.SetBool("isIdle", false);
        wanderAnims.SetBool("isRunning", true);
        wanderNav.SetDestination(destination.position);
    }
    

}

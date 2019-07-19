using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(AgentLinkMover))]
[RequireComponent(typeof(Rigidbody))]
public class ScriptedNPC : MonoBehaviour
{
    protected NavMeshAgent wanderNav;
    protected Animator wanderAnims;
    protected AudioSource wanderSounds;
    protected bool isRunning = false;
    protected void Awake()
    {
        wanderNav = GetComponent<NavMeshAgent>();
        wanderAnims = GetComponent<Animator>();
        wanderSounds = GetComponent<AudioSource>();
        wanderNav.updateUpAxis = false;
        wanderNav.updateRotation = false;
    }

    protected void Start()
    {
        startRun();
    }

    public void kill()
    {
        wanderAnims.SetBool("isDead", true);
    }

    protected void startRun()
    {
        wanderAnims.SetBool("isIdle", false);
        wanderAnims.SetBool("isRunning", true);
        wanderNav.SetDestination(new Vector3(0, 0, 0));
        //StartCoroutine(run());
    }

    IEnumerator run()
    {
        while (isRunning)
        {
            
        }
        yield return null;
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlidingDoor : MonoBehaviour {
    public Transform openPos;
    private Vector3 moveTo, origPos;
    protected NavMeshAgent enemyNav;
    protected Enemy enemyScript;
    public float speed = 0.7127584f;
    [SerializeField] protected float computerCloseChance = 15f;
    [SerializeField] protected bool enemyCanOpen = true;
    public bool EnemyCanOpen
    {
        get
        {
            return enemyCanOpen;
        }
        set
        {
            offMeshLink.enabled = value;
            enemyCanOpen = value;
        }
    }
    public bool doorChanging, doorStartsOpen = false;
    [SerializeField] bool isDependent = false;
    [SerializeField] protected AudioClip[] doorOpen, doorClose;
    [SerializeField] protected AudioClip computerSound;
    protected bool doorIsOpen, enemyIsWalkingThru, doorIsLocked = false;
    public OffMeshLink offMeshLink;
    protected AgentLinkMover agentLinkMover;
    [HideInInspector] public AudioSource doorSounds;

    // Use this for initialization
    protected virtual void Start () {
        EnemyCanOpen = enemyCanOpen;
        origPos = transform.position;
        moveTo = origPos;
        doorSounds = GetComponent<AudioSource>();
        doorStartsOpen = doorIsOpen;
        if(!enemyCanOpen && !isDependent)
        {
            offMeshLink.activated = false;
        }
    }
	
	// Update is called once per frame
	protected virtual void Update () {
        if(doorChanging)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
            //TODO: Find speed/animation of original CB door
            if(transform.position == moveTo)
            {
                doorChanging = false;
                doorIsOpen = !(moveTo == origPos);
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log("Moving door to opposite state because player pressed O");
            moveDoor(!doorIsOpen);
        }

    }


    public virtual void moveDoor(bool openDoor)
    {
        if (doorStartsOpen)
        {
            openDoor = !openDoor; //Invert open door variable because the door is meant to start closed.
        }
        if(!doorChanging && !doorIsLocked)
        {
            if (openDoor)
            {
                if (!isDependent && doorOpen.Length > 0)
                {
                    System.Random rnd = new System.Random();
                    int openSound = rnd.Next(0, doorOpen.Length - 1);
                    doorSounds.clip = doorOpen[openSound];
                }
                moveTo = openPos.position;
            }
            else
            {
                if (!isDependent && doorClose.Length > 0)
                {
                    System.Random rnd = new System.Random();
                    int closeSound = rnd.Next(0, doorClose.Length - 1);
                    doorSounds.clip = doorClose[closeSound];
                }
                moveTo = origPos;
            }
            if (!isDependent)
            {
                doorSounds.Play();
            }
            doorChanging = true;
        }
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if(!isDependent)
        {
            if (other.tag == "EnemyNPC")
            {
                enemyNav = other.GetComponent<NavMeshAgent>();
                agentLinkMover = other.GetComponent<AgentLinkMover>();
                enemyScript = other.GetComponent<Enemy>();
                
            }
            else if (tag == "Player")
            {
                System.Random chance = new System.Random();
                int randomInt = chance.Next(0, 100);
                if (randomInt <= computerCloseChance && doorIsOpen)
                {
                    StartCoroutine(playComputerSound(computerSound));
                    Debug.Log("Moving door to closed because 079 decided to");
                    moveDoor(false);
                }
            }
        }
        
    }
    protected virtual void OnTriggerStay(Collider other)
    {
        if(!isDependent)
        {
            if (enemyNav != null && enemyCanOpen)
            {
                if (enemyNav.isOnOffMeshLink)
                {
                    if (!doorIsOpen && !doorChanging)
                    {
                        //speed *= 1.5f;
                        Debug.Log("Moving door to open because the enemy is accessing the offmeshlink");
                        moveDoor(true);
                    }

                    if (!doorChanging && !enemyIsWalkingThru && doorIsOpen)
                    {
                        //speed /= 1.5f;
                        enemyScript.enemyChasesPlayer = false;
                        StartCoroutine(agentLinkMover.MoveNavMesh());
                        enemyIsWalkingThru = true;
                        if (!enemyScript.playerTargeted)
                        {
                            StartCoroutine(closeDoorBehind());
                        }
                    }

                }
            }
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (!isDependent)
        {
            if (other.tag == "EnemyNPC")
            {
                Debug.Log("Closing door behind enemy");
                enemyIsWalkingThru = false;
                enemyScript.enemyChasesPlayer = true;
            }
        }
    }

    protected virtual IEnumerator playComputerSound(AudioClip sound)
    {
        AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.clip = sound;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(audioSource);
    }

    protected virtual IEnumerator closeDoorBehind()
    {
        yield return new WaitUntil(() => !enemyIsWalkingThru);
        Debug.Log("Moving door to closed because the enemy is closing the door behind it");
        moveDoor(false);
    }
}
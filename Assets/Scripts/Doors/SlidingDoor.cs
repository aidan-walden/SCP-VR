using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlidingDoor : MonoBehaviour {
    public Transform openPos;
    private Vector3 moveTo, origPos;
    private NavMeshAgent enemyNav;
    public float speed = 0.7127584f;
    [SerializeField] float computerCloseChance = 15f;
    public bool enemyCanOpen = true;
    public bool doorChanging, doorStartsOpen, isDependent = false;
    [SerializeField] private AudioClip[] doorOpen, doorClose;
    [SerializeField] private AudioClip computerSound;
    private bool doorIsOpen, enemyIsWalkingThru, doorIsLocked = false;
    [SerializeField] OffMeshLink offMeshLink;
    AgentLinkMover agentLinkMover;
    [HideInInspector] public AudioSource doorSounds;

    // Use this for initialization
    void Start () {
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
	void Update () {
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
            moveDoor(!doorIsOpen);
        }

    }


    public void moveDoor(bool openDoor)
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
    private void OnTriggerEnter(Collider other)
    {
        if(!isDependent)
        {
            if (other.transform.root.gameObject.tag == "EnemyNPC")
            {
                enemyNav = other.transform.root.GetComponent<NavMeshAgent>();
                agentLinkMover = other.transform.root.GetComponent<AgentLinkMover>();
            }
            else if (tag == "Player")
            {
                System.Random chance = new System.Random();
                int randomInt = chance.Next(0, 100);
                if (randomInt <= computerCloseChance && doorIsOpen)
                {
                    StartCoroutine(playComputerSound(computerSound));
                    moveDoor(false);
                }
            }
        }
        
    }
    private void OnTriggerStay(Collider other)
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
                        moveDoor(true);
                    }

                    if (!doorChanging && !enemyIsWalkingThru && doorIsOpen)
                    {
                        //speed /= 1.5f;
                        StartCoroutine(agentLinkMover.MoveNavMesh());
                        enemyIsWalkingThru = true;

                    }

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.root.gameObject.tag == "EnemyNPC")
        {
            enemyIsWalkingThru = false;
        }
    }

    IEnumerator playComputerSound(AudioClip sound)
    {
        AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.clip = sound;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        Destroy(audioSource);
    }
}
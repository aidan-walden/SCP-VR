using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DualSlidingDoor : MonoBehaviour
{
    public bool enemyCanOpen;
    private bool doorIsOpen = false;
    public bool DoorIsOpen
    {
        get
        {
            return doorIsOpen;
        }
    }
    public bool doorStartsOpen, enemyIsWalkingThru, doorIsLocked = false;
    private NavMeshAgent enemyNav;
    private AgentLinkMover agentLinkMover;
    [SerializeField] private SlidingDoor[] doors;
    [SerializeField] AudioSource doorSounds;
    [SerializeField] private AudioClip doorOpen, doorClose, computerSound;
    [SerializeField] float speed = 1f;
    [SerializeField] float computerCloseChance = 15f;
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            foreach (SlidingDoor door in doors)
            {
                door.speed = value;
            }
            speed = value;
        } 
    }
    // Use this for initialization
    void Start()
    {
        Speed = speed;
        doorIsOpen = doorStartsOpen;
        foreach(SlidingDoor door in doors)
        {
            door.enemyCanOpen = enemyCanOpen;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        string tag = other.transform.root.gameObject.tag;
        if (tag == "EnemyNPC")
        {
            enemyNav = other.transform.root.GetComponent<NavMeshAgent>();
            agentLinkMover = other.transform.root.GetComponent<AgentLinkMover>();
        }
        else if(tag == "Player")
        {
            System.Random chance = new System.Random();
            int randomInt = chance.Next(0, 100);
            if(randomInt <= computerCloseChance && doorIsOpen)
            {
                StartCoroutine(playComputerSound(computerSound));
                moveDoor(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (enemyNav != null && enemyCanOpen)
        {
            if (enemyNav.isOnOffMeshLink)
            {
                Debug.Log(enemyNav.name + " is on the off mesh link");
                if (!doorIsOpen)
                {
                    //speed *= 1.5f;
                    moveDoor(true);
                }

                if (!doors[0].doorChanging && !enemyIsWalkingThru && doorIsOpen)
                {
                    //speed /= 1.5f;
                    StartCoroutine(agentLinkMover.MoveNavMesh());
                    enemyIsWalkingThru = true;

                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemyIsWalkingThru = false;
    }

    public void changeDoorState()
    {
        moveDoor(!doorIsOpen);
    }

    public void moveDoor(bool openDoor)
    {
        if(doorStartsOpen)
        {
            openDoor = !openDoor;
        }
        if(!doors[0].doorChanging && !doors[1].doorChanging && !doorIsLocked)
        {
            if (openDoor)
            {
                doorSounds.clip = doorOpen;
            }
            else
            {
                doorSounds.clip = doorClose;
            }
            doorSounds.Play();
            foreach (SlidingDoor door in doors)
            {
                door.moveDoor(openDoor);
            }
            doorIsOpen = openDoor;
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
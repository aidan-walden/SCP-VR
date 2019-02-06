using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DualSlidingDoor : SlidingDoor
{
    public bool DoorIsOpen
    {
        get
        {
            return doorIsOpen;
        }
    }
    [SerializeField] private SlidingDoor[] doors;
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
    protected override void Start()
    {
        Speed = speed;
        doorIsOpen = doorStartsOpen;
        foreach(SlidingDoor door in doors)
        {
            door.EnemyCanOpen = enemyCanOpen;
        }
    }
    protected override void OnTriggerEnter(Collider other)
    {
        string tag = other.gameObject.tag;
        if (tag == "EnemyNPC")
        {
            enemyNav = other.GetComponent<NavMeshAgent>();
            agentLinkMover = other.GetComponent<AgentLinkMover>();
            enemyScript = other.GetComponent<Enemy>();
            
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
    protected override void OnTriggerStay(Collider other)
    {
        if (enemyNav != null)
        {
            if (enemyNav.isOnOffMeshLink)
            {
                Debug.Log(enemyNav.name + " is on the off mesh link");
                if (!doorIsOpen && enemyCanOpen && !doors[0].doorChanging && !doors[1].doorChanging )
                {
                    //speed *= 1.5f;
                    Debug.Log("Moving door to open because the enemy is accessing the off mesh link with door name " + this.name + " and object name " + other.name);
                    moveDoor(true);
                }

                if (!doors[0].doorChanging && !doors[1].doorChanging && !enemyIsWalkingThru && doorIsOpen)
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

    public void changeDoorState()
    {
        moveDoor(!doorIsOpen);
    }

    public override void moveDoor(bool openDoor)
    {
        if(doorStartsOpen)
        {
            openDoor = !openDoor;
        }
        if(!doors[0].doorChanging && !doors[1].doorChanging && !doorIsLocked)
        {
            System.Random rnd = new System.Random();
            int openSound = rnd.Next(0, doorOpen.Length - 1);
            int closeSound = rnd.Next(0, doorClose.Length - 1);
            if (openDoor)
            {
                doorSounds.clip = doorOpen[openSound];
            }
            else
            {
                doorSounds.clip = doorClose[closeSound];
            }
            doorSounds.Play();
            foreach (SlidingDoor door in doors)
            {
                door.moveDoor(openDoor);
            }
            doorIsOpen = openDoor;
        } 
    }
}
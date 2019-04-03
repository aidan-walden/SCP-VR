using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public AudioClip ding, movement;
    PlayerEvents playerScript;
    public ElevatorMove[] moveScripts = new ElevatorMove[2];
    bool elevatorIsMoving = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(callEvevator());
        }
    }

    IEnumerator callEvevator()
    {
        if (!elevatorIsMoving)
        {
            elevatorIsMoving = true;
            foreach (ElevatorMove moveScript in moveScripts)
            {
                if(moveScript.eleDoor.doorIsOpen)
                {
                    moveScript.eleDoor.moveDoor(false);
                }
                foreach (Transform eleObject in moveScript.objectsInEle)
                {
                    if (eleObject.root.tag == "Player")
                    {
                        playerScript = eleObject.root.gameObject.GetComponent<PlayerEvents>();

                    }
                }
            }
            foreach (ElevatorMove moveScript in moveScripts)
            {
                yield return new WaitUntil(() => !moveScript.eleDoor.doorChanging);
            }
            if (playerScript != null)
            {
                yield return new WaitForSeconds(ding.length);
                playerScript.playSound(movement);
            }
            yield return new WaitForSeconds(movement.length - 4f);
            foreach (ElevatorMove moveScript in moveScripts)
            {
                moveScript.swapElevators();
            }
            yield return new WaitForSeconds(4f);
            if (playerScript != null)
            {
                playerScript.playSound(ding);
            }

            foreach (ElevatorMove moveScript in moveScripts)
            {
                if (!moveScript.eleDoor.doorIsOpen)
                {
                    moveScript.eleDoor.moveDoor(true);
                }
            }
            playerScript = null;
            elevatorIsMoving = false;
        }
    }
}

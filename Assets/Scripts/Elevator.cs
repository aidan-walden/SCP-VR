using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public AudioClip ding, movement;
    public PlayerEvents playerScript;
    int player = 1;
    public ElevatorMove[] moveScripts = new ElevatorMove[2];
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
        foreach (ElevatorMove moveScript in moveScripts)
        {
            moveScript.eleDoor.moveDoor(false);
            foreach(Transform eleObject in moveScript.objectsInEle)
            {
                if(eleObject.root.tag == "Player")
                {
                    //player = eleObject.root.gameObject;
                }
            }
        }
        if (player != null)
        {
            //playerScript = player.GetComponent<PlayerEvents>();
            playerScript.playSound(ding);
        }
        foreach (ElevatorMove moveScript in moveScripts)
        {
            yield return new WaitUntil(() => !moveScript.eleDoor.doorChanging);
        }
        if (player != null)
        {
            yield return new WaitForSeconds(ding.length);
            playerScript.playSound(movement);
        }
        yield return new WaitForSeconds(movement.length);
        foreach(ElevatorMove moveScript in moveScripts)
        {
            moveScript.swapElevators();
        }
        if(player != null)
        {
            playerScript.playSound(ding);
        }

        foreach (ElevatorMove moveScript in moveScripts)
        {
            moveScript.eleDoor.moveDoor(true);
        }

    }
}

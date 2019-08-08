using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class SCP1123 : MonoBehaviour
{
    public Transform teleOne, teleThree, teleFour, newOfficerPos;
    public NaziOfficer naziScript;
    private bool isUsed = false;
    GameObject player;
    Hand playerHand;

    private void OnCollisionEnter(Collision collision)
    {
        playerHand = collision.gameObject.GetComponent<Hand>();
        if (!isUsed && playerHand != null && !collision.gameObject.transform.root.gameObject.GetComponent<PlayerEvents>().RingOn)
        {
            isUsed = true;
            player = collision.transform.root.gameObject;
            collision.transform.root.position = teleOne.position;
            naziScript.openRoomDoor();
        }
    }

    public void teleportPlayer(GameObject doorknob)
    {
        StartCoroutine(doorOpened(doorknob));
    }

    IEnumerator doorOpened(GameObject doorknob)
    {
        yield return new WaitForSeconds(4f);
        playerHand.DetachObject(doorknob);
        player.transform.position = teleThree.position;
        player.transform.rotation = teleThree.rotation;
        naziScript.gameObject.transform.position = newOfficerPos.position;
        naziScript.gameObject.transform.rotation = newOfficerPos.rotation;
        //TODO: Change player movement speed
        yield return new WaitForSeconds(5f);
        Transform oldTransform = player.transform;
        player.transform.position = teleFour.position;
        naziScript.shootGun(oldTransform);
        
    }
}

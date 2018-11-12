using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeycardReader : MonoBehaviour {
    public int lvl = 2;
    public AudioSource doorSound;
    public AudioClip doorChange, doorBuffer;
    public GameObject door;
    private bool doorIsOpening = false;
    private bool doorIsOpen = false;
    public bool isCheckpoit = false;
    private SlidingDoor doorScript;
	// Use this for initialization
	void Start () {
        doorScript = door.GetComponent<SlidingDoor>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if(collision.gameObject.tag == "Keycard" && !doorIsOpening)
        {
            KeycardAuth ksScript = collision.gameObject.GetComponent<KeycardAuth>();
            if(ksScript.Lvl >= lvl)
            {
                Debug.Log("Open door");
                if(isCheckpoit)
                {
                    StartCoroutine(fullCycleDoor());
                }
                else
                {
                    StartCoroutine(changeDoor());
                }
            }
            else
            {
                Debug.Log("Keycard is not high enough level!");
            }
            


        }
    }

    IEnumerator fullCycleDoor() //Opens the door, buffers, then closes the door again
    {
        Debug.Log("Cycling through door sounds...");
        doorIsOpening = true;
        doorSound.clip = doorChange;
        doorSound.Play();
        doorScript.moveDoor(true);
        yield return new WaitForSeconds(doorSound.clip.length);
        doorIsOpen = true;
        doorSound.clip = doorBuffer;
        doorSound.Play();
        yield return new WaitForSeconds(doorSound.clip.length);
        doorSound.clip = doorChange;
        doorSound.Play();
        doorScript.moveDoor(false);
        yield return new WaitForSeconds(doorSound.clip.length);
        doorIsOpen = false;
        doorIsOpening = false;



    }
    IEnumerator changeDoor() //Only changes the door to the opposite of what it was before
    {
        Debug.Log("Cycling through door sounds...");
        doorIsOpening = true;
        doorScript.moveDoor(!doorIsOpen);
        doorIsOpen = !doorIsOpen;
        yield return new WaitForSeconds(doorScript.doorSounds.clip.length);
        doorIsOpening = false;
    }
}

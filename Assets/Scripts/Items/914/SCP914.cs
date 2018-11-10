using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCP914 : MonoBehaviour {
    public GameObject inputTrigger;
    private enum upgradeSetting {ROUGH, COARSE, ONE_TO_ONE, FINE, VERY_FINE };
    private int currentSetting = (int)upgradeSetting.ONE_TO_ONE;
    public GameObject outputSpot, machineDial, inputDoor, outputDoor;
    private AudioSource machineSounds;
    public AudioClip machineBuffer, machineDone;
    [SerializeField]  private float waitingOffset = 2f;
    private Input914 input;
    private SlidingDoor inputDoorScript, outputDoorScript;
    private bool isUpgrading = false;
	// Use this for initialization
	void Start () {
        input = GetComponentInChildren<Input914>();
        inputDoorScript = inputDoor.GetComponent<SlidingDoor>();
        outputDoorScript = outputDoor.GetComponent<SlidingDoor>();
        machineSounds = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startUpgrade()
    {
        if(!isUpgrading)
        {
            Debug.Log("Start upgrade");
            StartCoroutine("fullMachineCycle");
        }
    }

    IEnumerator fullMachineCycle()
    {
        isUpgrading = true;
        //Close doors
        inputDoorScript.moveDoor(false);
        outputDoorScript.moveDoor(false);
        //yield return new WaitUntil(() => !inputDoorScript.doorChanging);
        machineSounds.clip = machineBuffer;
        machineSounds.Play();
        yield return new WaitForSeconds(machineSounds.clip.length - waitingOffset);
        List<GameObject> tempItemArray = new List<GameObject>(input.getInput());
        foreach(GameObject item in tempItemArray)
        {
            Debug.Log(item.name);
        }
        upgradeItems(tempItemArray);
        placeItemsInOutput(tempItemArray);
        yield return new WaitForSeconds(machineSounds.clip.length - machineSounds.time); //Wait the remaining duration of the sound
        //Open doors
        machineSounds.clip = machineDone;
        machineSounds.Play();
        inputDoorScript.moveDoor(true);
        outputDoorScript.moveDoor(true);
        isUpgrading = false;
    }


    void upgradeItems(List<GameObject> itemsToUpgrade)
    {
        Debug.Log("Upgrading items...");
        foreach(GameObject item in itemsToUpgrade)
        {
            Debug.Log(item.name);
            if(currentSetting == 3) //Fine setting
            {
                if (item.transform.parent.tag == "Keycard")
                {
                    Debug.Log(item.transform.parent.GetComponent<KeycardAuth>().lvl);
                    item.transform.parent.GetComponent<KeycardAuth>().lvl++;
                    Debug.Log(item.transform.parent.GetComponent<KeycardAuth>().lvl);
                }
            }
            
        }
    }

    void placeItemsInOutput(List<GameObject> itemsToPlace)
    {
        Debug.Log("Moving items...");
        foreach(GameObject item in itemsToPlace)
        {
            if(item.transform.parent.tag == "Keycard")
            {
                item.transform.parent.transform.position = outputSpot.transform.position;
            }
            item.transform.position = outputSpot.transform.position;
        }
    }


}

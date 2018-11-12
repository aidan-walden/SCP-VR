using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCP914 : MonoBehaviour {
    public GameObject inputTrigger, itemsManager;
    public enum upgradeSetting {ROUGH, COARSE, ONE_TO_ONE, FINE, VERY_FINE };
    public int currentSetting = (int)upgradeSetting.ONE_TO_ONE;
    public GameObject outputSpot, machineDial;
    private AudioSource machineSounds;
    public AudioClip machineBuffer, machineDone;
    [SerializeField]  private float waitingOffset = 2f;
    private Input914 input;
    public SlidingDoor inputDoor, outputDoor;
    private bool isUpgrading = false;
    private Items itemsHolder;
    // Use this for initialization
    void Start () {
        input = GetComponentInChildren<Input914>();
        machineSounds = GetComponent<AudioSource>();
        itemsHolder = itemsManager.GetComponent<Items>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startUpgrade()
    {
        if(!isUpgrading)
        {
            Debug.Log("Start upgrade");
            StartCoroutine(fullMachineCycle());
        }
    }

    IEnumerator fullMachineCycle()
    {
        isUpgrading = true;
        //Close doors
        inputDoor.moveDoor(true);
        outputDoor.moveDoor(true);
        //yield return new WaitUntil(() => !inputDoorScript.doorChanging);
        machineSounds.clip = machineBuffer;
        machineSounds.Play();
        yield return new WaitForSeconds(machineSounds.clip.length - waitingOffset);
        List<GameObject> tempItemArray = new List<GameObject>(input.getInput());
        foreach(GameObject item in tempItemArray)
        {
            Debug.Log(item.name);
        }
        placeItemsInOutput(upgradeItems(tempItemArray));
        yield return new WaitForSeconds(machineSounds.clip.length - machineSounds.time); //Wait the remaining duration of the sound
        //Open doors
        machineSounds.clip = machineDone;
        machineSounds.Play();
        inputDoor.moveDoor(false);
        outputDoor.moveDoor(false);
        isUpgrading = false;
    }


    List<GameObject> upgradeItems(List<GameObject> itemsToUpgrade)
    {
        List<GameObject> upgradedItems = new List<GameObject>();
        Debug.Log("Upgrading items...");
        foreach(GameObject item in itemsToUpgrade)
        {
            if(currentSetting == (int)upgradeSetting.FINE) //Fine setting
            {
               switch(item.name)
                {
                    case "Keycard":
                        KeycardAuth keycard = item.GetComponent<KeycardAuth>();
                        if(keycard.Lvl >= 3)
                        {
                            GameObject playingCard = Instantiate(itemsHolder.items["Playing Card"]);
                            playingCard.transform.position = item.transform.position;
                            Destroy(item);
                            upgradedItems.Add(playingCard);
                            break;
                        }
                        keycard.Lvl++;
                        upgradedItems.Add(item);
                        break;
                }
            }
        }
        return upgradedItems;
    }

    void placeItemsInOutput(List<GameObject> itemsToPlace)
    {
        Debug.Log("Moving items...");
        foreach(GameObject item in itemsToPlace)
        {
            item.transform.root.position = outputSpot.transform.position;
        }
    }
}

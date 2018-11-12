using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]
public class Dial914 : MonoBehaviour
{
    Hand playerHand;
    public bool rotateToOrig = false;
    public bool isKey = false;
    public float rotateSpeed;
    [SerializeField] Rigidbody dial;
    public SCP914 settingScript;
    Vector3 rotateTo;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame

    private void HandAttachedUpdate()
    {
        //* Quaternion.Inverse(initialGrabRotation).z * 25
        //Debug.Log(playerHand.transform.rotation.z);
        //dial.MoveRotation(Quaternion.Euler(0, 0, -playerHand.transform.rotation.z * 100));
        Vector3 eulerRotation = new Vector3(0, 0, playerHand.transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }

    private void Update()
    {
        Debug.Log("Euler Angles: " + transform.eulerAngles.z);
        if (rotateToOrig)
        {
            Debug.Log("Rotating...");
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotateTo), Time.deltaTime * rotateSpeed);
            if (transform.rotation == Quaternion.Euler(rotateTo))
            {
                Debug.Log("Stopping rotation...");
                rotateToOrig = false;
            }
        }
    }

    protected virtual void OnAttachedToHand(Hand hand)
    {
        playerHand = hand;
        if(isKey)
        {
            settingScript.startUpgrade();
            rotateDial(0f);
            playerHand.DetachObject(this.gameObject);
        }
    }

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        Debug.Log(transform.name + " has been detached from the hand. Rotation is now " + transform.rotation.z + ". Euler angles: " + transform.eulerAngles.z);
        if (!isKey)
        {
            chooseRotation();
        }
    }

    void rotateDial(float rotation)
    {
        Debug.Log("Starting rotation...");
        rotateTo = new Vector3(0, 0, rotation);
        rotateToOrig = true;
    }

    void chooseRotation()
    {
        float eulerZ = transform.eulerAngles.z;
        if (eulerZ <= 277f && eulerZ >= 180f) //Below very fine
        {
            Debug.Log("Rotate to very fine");
            rotateDial(277f);
            settingScript.currentSetting = (int)SCP914.upgradeSetting.VERY_FINE;
        }
        else if (eulerZ >= 82.9f && eulerZ <= 180f) //Below rough
        {
            Debug.Log("Rotate to rough");
            rotateDial(82.9f);
            settingScript.currentSetting = (int)SCP914.upgradeSetting.ROUGH;
        }
        else if(eulerZ <= 82.9f && eulerZ >= 40.5f) //Between rough and coarse
        {
            if(82.9f - eulerZ < eulerZ - 40.5f) //Closer to rough
            {
                rotateDial(82.9f); //Rotate to rough
                settingScript.currentSetting = (int)SCP914.upgradeSetting.ROUGH;
            }
            else //Closer to coarse
            {
                rotateDial(40.5f); //Rotate to coarse
                settingScript.currentSetting = (int)SCP914.upgradeSetting.COARSE;
            }
        }
        else if(eulerZ <= 40.5f && eulerZ >= 0) //Between coarse and 1 to 1
        {
            if(40.5f - eulerZ < eulerZ - 0) //Closer to coarse
            {
                rotateDial(40.5f); //Rotate to coarse
                settingScript.currentSetting = (int)SCP914.upgradeSetting.COARSE;
            }
            else
            {
                rotateDial(0f); //Rotate to 1 to 1
                settingScript.currentSetting = (int)SCP914.upgradeSetting.ONE_TO_ONE;
            }
        }
        else if(eulerZ >= 321f) //Between 1 to 1 and fine
        {
            if(359.9f - eulerZ < eulerZ - 321f) //Closer to 1 to 1
            {
                rotateDial(0f);
                settingScript.currentSetting = (int)SCP914.upgradeSetting.ONE_TO_ONE;
            }
            else
            {
                rotateDial(321f);
                settingScript.currentSetting = (int)SCP914.upgradeSetting.FINE;
            }
        }
        else if(eulerZ >= 277f && eulerZ <= 321f)
        {
            if(321f - eulerZ < eulerZ - 277f) //Closer to fine
            {
                rotateDial(321f);
                settingScript.currentSetting = (int)SCP914.upgradeSetting.FINE;
            }
            else
            {
                rotateDial(277f);
                settingScript.currentSetting = (int)SCP914.upgradeSetting.VERY_FINE;
            }
        }
    }
}

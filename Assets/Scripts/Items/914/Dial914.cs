using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(Rigidbody))]
public class Dial914 : MonoBehaviour
{
    Hand playerHand;
    public bool isKey = false;
    public float rotateSpeed;
    [SerializeField] Rigidbody dial;
    public SCP914 settingScript;
    float dialOffset;
    // Use this for initialization
    void Start()
    {
        dialOffset = transform.parent.parent.rotation.eulerAngles.y;
    }

    private void HandAttachedUpdate()
    {
        //* Quaternion.Inverse(initialGrabRotation).z * 25
        //Debug.Log(playerHand.transform.rotation.z);
        //dial.MoveRotation(Quaternion.Euler(0, 0, -playerHand.transform.rotation.z * 100));
        Vector3 eulerRotation = new Vector3(0, dialOffset, playerHand.transform.eulerAngles.z);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }


    private IEnumerator rotateToOrig(Vector3 rotateTo)
    {
        while (transform.rotation != Quaternion.Euler(rotateTo))
        {
            Debug.Log("Attempting to rotate to " + rotateTo + "...");
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotateTo), Time.deltaTime * rotateSpeed);
            yield return null;
        }
        Debug.Log("NEW ROTATION: " + transform.rotation.x + ", " + transform.rotation.y + ", " + transform.rotation.z);
        Debug.Log("EULER ANGLES: " + transform.eulerAngles.z);
        yield return null;
    }

    protected virtual void OnAttachedToHand(Hand hand)
    {
        StopAllCoroutines();
        playerHand = hand;
        if(isKey)
        {
            settingScript.startUpgrade();
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
        StartCoroutine(rotateToOrig(new Vector3(0, dialOffset, rotation)));
    }

    void chooseRotation()
    {
        float eulerZ = transform.eulerAngles.z;
        if (eulerZ <= 277f && eulerZ >= 180f) //Below very fine
        {
            Debug.Log("Rotate to very fine");
            rotateDial(277f);
            settingScript.currentSetting = SCP914.upgradeSetting.VERY_FINE;
        }
        else if (eulerZ >= 82.9f && eulerZ <= 180f) //Below rough
        {
            Debug.Log("Rotate to rough");
            rotateDial(82.9f);
            settingScript.currentSetting = SCP914.upgradeSetting.ROUGH;
        }
        else if(eulerZ <= 82.9f && eulerZ >= 40.5f) //Between rough and coarse
        {
            if(82.9f - eulerZ < eulerZ - 40.5f) //Closer to rough
            {
                rotateDial(82.9f); //Rotate to rough
                settingScript.currentSetting = SCP914.upgradeSetting.ROUGH;
            }
            else //Closer to coarse
            {
                rotateDial(40.5f); //Rotate to coarse
                settingScript.currentSetting = SCP914.upgradeSetting.COARSE;
            }
        }
        else if(eulerZ <= 40.5f && eulerZ >= 0) //Between coarse and 1 to 1
        {
            if(40.5f - eulerZ < eulerZ - 0) //Closer to coarse
            {
                rotateDial(40.5f); //Rotate to coarse
                settingScript.currentSetting = SCP914.upgradeSetting.COARSE;
            }
            else
            {
                rotateDial(0f); //Rotate to 1 to 1
                settingScript.currentSetting = SCP914.upgradeSetting.ONE_TO_ONE;
            }
        }
        else if(eulerZ >= 321f) //Between 1 to 1 and fine
        {
            if(359.9f - eulerZ < eulerZ - 321f) //Closer to 1 to 1
            {
                rotateDial(0f);
                settingScript.currentSetting = SCP914.upgradeSetting.ONE_TO_ONE;
            }
            else
            {
                rotateDial(321f);
                settingScript.currentSetting = SCP914.upgradeSetting.FINE;
            }
        }
        else if(eulerZ >= 277f && eulerZ <= 321f)
        {
            if(321f - eulerZ < eulerZ - 277f) //Closer to fine
            {
                rotateDial(321f);
                settingScript.currentSetting = SCP914.upgradeSetting.FINE;
            }
            else
            {
                rotateDial(277f);
                settingScript.currentSetting = SCP914.upgradeSetting.VERY_FINE;
            }
        }
    }
}

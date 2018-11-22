
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.SceneManagement;
using Valve.VR;

public class Inventory : MonoBehaviour
{
    public GameObject invSphere, scp096;
    public float offsetHoriz = 5;
    public float offsetVert = 10;
    GameObject invSphereDisplaying;
    public SteamVR_Action_Boolean openAction;

    public Hand hand;


    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();

        if (openAction == null)
        {
            Debug.LogError("No open inv action assigned");
            return;
        }

        openAction.AddOnChangeListener(OnOpenActionChange, hand.handType);
    }

    private void OnDisable()
    {
        if (openAction != null)
            openAction.RemoveOnChangeListener(OnOpenActionChange, hand.handType);
    }

    private void OnOpenActionChange(SteamVR_Action_In actionIn)
    {
        if (openAction.GetStateDown(hand.handType))
        {
            Open(hand);
        }
        if(openAction.GetStateUp(hand.handType))
        {
            Close(hand);
        }
    }

    public void Open(Hand hand)
    {
        //StartCoroutine(DoOpen(hand));
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //scp096.SetActive(!scp096.activeSelf);
        this.transform.root.position = new Vector3(-3.45f, 0f, 26.19f);
    }

    public void Close(Hand hand)
    {
        //StartCoroutine(DoClose(hand));
    }

    private IEnumerator DoOpen(Hand hand)
    {
        invSphereDisplaying = GameObject.Instantiate<GameObject>(invSphere);
        invSphereDisplaying.transform.parent = hand.transform;
        invSphereDisplaying.transform.position = hand.transform.position + (hand.transform.forward / offsetHoriz);
        invSphereDisplaying.transform.position += (-invSphereDisplaying.transform.up / offsetVert);
        yield return null;
        //TODO: Solve bug where the sphere position relative to the controller varies on controller position
    }

    private IEnumerator DoClose(Hand hand)
    {

        Destroy(invSphereDisplaying);
        yield return null;
    }
}
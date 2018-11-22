using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class ChannelSwitcher : MonoBehaviour
{
    public Radio radio;
    public SteamVR_Action_Boolean changeChannelUpAction, changeChannelDownAction;

    public Hand hand;


    private void OnEnable()
    {
        if (hand == null)
            hand = this.GetComponent<Hand>();

        if (changeChannelUpAction == null)
        {
            Debug.LogError("No change channel up action assigned");
            return;
        }
        if(changeChannelDownAction == null)
        {
            Debug.LogError("No change channel down action assigned");
            return;
        }

        changeChannelUpAction.AddOnChangeListener(OnRadioChannelUp, hand.handType);
        changeChannelDownAction.AddOnChangeListener(OnRadioChannelDown, hand.handType);
    }

    private void OnDisable()
    {
        if (changeChannelUpAction != null)
        {
            changeChannelUpAction.RemoveOnChangeListener(OnRadioChannelUp, hand.handType);
        }
        if(changeChannelDownAction != null)
        {
            changeChannelDownAction.RemoveOnChangeListener(OnRadioChannelDown, hand.handType);
        }
    }

    private void OnRadioChannelUp(SteamVR_Action_In actionIn)
    {
        if (changeChannelUpAction.GetStateDown(hand.handType))
        {
            changeChannelUp();
        }
    }

    private void OnRadioChannelDown(SteamVR_Action_In actionIn)
    {
        if (changeChannelUpAction.GetStateDown(hand.handType))
        {
            changeChannelDown();
        }
    }

    public void changeChannelUp()
    {
        if(radio != null)
        {
            radio.CurrentChannel++;
            radio.changeChannel();
        }
    }

    public void changeChannelDown()
    {
        if(radio != null)
        {
            radio.CurrentChannel--;
            radio.changeChannel();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;
using Valve.VR.Extras;
using Valve.VR;

public class MainMenu : MonoBehaviour
{
    public void loadFacility()
    {
        Transform SteamObjects = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
        Transform[] hands = { SteamObjects.GetChild(1), SteamObjects.GetChild(2) };
        foreach(Transform hand in hands) //Remove UI Laser interaction scripts
        {
            Destroy(hand.GetComponent<VRUIInput>());
            Destroy(hand.GetComponent<SteamVR_LaserPointer>());
            Destroy(hand.GetChild(5).gameObject);
        }
        GetComponent<SteamVR_LoadLevel>().Trigger();
    }

    public void quitGame()
    {
        Application.Quit();
    }


}

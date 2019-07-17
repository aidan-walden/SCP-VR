using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaFire : MonoBehaviour
{
    public AudioClip standby, charge, fire;
    public float chargeDiff, fireDiff;
    public AudioSource teslaSounds;
    public GameObject teslaKill;
    private bool playerInRange, teslaActive = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Player")
        {
            Debug.Log("Player has entered range");
            playerInRange = true;
            if(!teslaActive)
            {
                StartCoroutine(loopFire());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.root.tag == "Player")
        {
            Debug.Log("Player has left range");
            playerInRange = false;
        }
    }

    IEnumerator loopFire() //TODO: Randomly change rotation of teslaKill to make lights and electric texture chaotic
    {
        while(playerInRange)
        {
            teslaActive = true;
            teslaSounds.PlayOneShot(charge);
            yield return new WaitForSeconds(charge.length - chargeDiff);
            teslaSounds.PlayOneShot(fire);
            teslaKill.SetActive(true);
            yield return new WaitForSeconds(fire.length - fireDiff);
            teslaKill.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        teslaActive = false;
        yield return null;
    }
}

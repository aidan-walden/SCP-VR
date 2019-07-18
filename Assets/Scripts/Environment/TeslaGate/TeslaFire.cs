using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaFire : MonoBehaviour
{
    public AudioClip charge, fire;
    public float chargeDiff, fireDiff, electricRate;
    public AudioSource teslaSounds;
    public GameObject teslaKill;
    private bool playerInRange, teslaActive = false;
    public bool teslaReady = false;

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
        yield return new WaitUntil(() => teslaReady);
        while (playerInRange)
        {
            teslaActive = true;
            teslaSounds.PlayOneShot(charge);
            yield return new WaitForSeconds(charge.length - chargeDiff);
            teslaSounds.PlayOneShot(fire);
            teslaKill.SetActive(true);
            StartCoroutine(moveElectric());
            yield return new WaitForSeconds(fire.length - fireDiff);
            teslaKill.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        teslaActive = false;
        yield return null;
    }

    IEnumerator moveElectric()
    {
        while(teslaKill.activeSelf)
        {
            int coord = Random.Range(0, 2);
            if(coord == 0)
            {
                teslaKill.transform.localScale = new Vector3(teslaKill.transform.localScale.x * -1, teslaKill.transform.localScale.y, teslaKill.transform.localScale.z);
            }
            else if(coord == 1)
            {
                teslaKill.transform.localScale = new Vector3(teslaKill.transform.localScale.x, teslaKill.transform.localScale.y * -1, teslaKill.transform.localScale.z);
            }
            else
            {
                teslaKill.transform.localScale = new Vector3(teslaKill.transform.localScale.x, teslaKill.transform.localScale.y, teslaKill.transform.localScale.z * -1);
            }
            yield return new WaitForSeconds(electricRate);
        }
        yield return null;
    }
}

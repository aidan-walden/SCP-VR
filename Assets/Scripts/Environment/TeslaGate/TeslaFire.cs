using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaFire : MonoBehaviour
{
    [SerializeField] AudioClip charge, fire;
    [SerializeField] float chargeDiff, fireDiff, electricRate;
    [SerializeField] AudioSource teslaSounds;
    [SerializeField] GameObject electricity, teslaKill;
    private bool playerInRange, teslaActive = false;
    public bool teslaReady = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.root.tag == "Player" || other.gameObject.tag == "FriendlyNPC")
        {
            Debug.Log("Valid target has entered range");
            playerInRange = true;
            if(!teslaActive)
            {
                StartCoroutine(loopFire());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.root.tag == "Player" || other.gameObject.tag == "FriendlyNPC")
        {
            Debug.Log("Target has left range");
            playerInRange = false;
        }
    }


    IEnumerator loopFire()
    {
        yield return new WaitUntil(() => teslaReady);
        while (playerInRange)
        {
            teslaActive = true;
            teslaSounds.PlayOneShot(charge);
            yield return new WaitForSeconds(charge.length - chargeDiff);
            teslaSounds.PlayOneShot(fire);
            teslaKill.SetActive(true);
            electricity.SetActive(true);
            StartCoroutine(moveElectric());
            yield return new WaitForSeconds(fire.length - fireDiff);
            teslaKill.SetActive(false);
            electricity.SetActive(false);
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
                electricity.transform.localScale = new Vector3(teslaKill.transform.localScale.x * -1, teslaKill.transform.localScale.y, teslaKill.transform.localScale.z);
            }
            else if(coord == 1)
            {
                electricity.transform.localScale = new Vector3(teslaKill.transform.localScale.x, teslaKill.transform.localScale.y * -1, teslaKill.transform.localScale.z);
            }
            else
            {
                electricity.transform.localScale = new Vector3(teslaKill.transform.localScale.x, teslaKill.transform.localScale.y, teslaKill.transform.localScale.z * -1);
            }
            yield return new WaitForSeconds(electricRate);
        }
        yield return null;
    }
}

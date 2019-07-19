using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeslaCharge : MonoBehaviour
{
    [SerializeField] AudioClip idle, chargeup;
    [SerializeField] TeslaFire fireScript;
    [SerializeField] AudioSource teslaLoop;
    bool teslaCharging = false;
    private void OnTriggerEnter(Collider other)
    {
        if(!teslaCharging && other.transform.root.tag == "Player")
        {
            StartCoroutine(teslaChargeup());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.root.tag == "Player")
        {
            StartCoroutine(teslaPowerDown());
        }
    }

    IEnumerator teslaChargeup()
    {
        teslaCharging = true;
        teslaLoop.loop = false;
        teslaLoop.Stop();
        teslaLoop.clip = null;
        teslaLoop.PlayOneShot(chargeup);
        yield return new WaitForSeconds(chargeup.length);
        teslaCharging = false;
        teslaLoop.loop = true;
        teslaLoop.clip = idle;
        teslaLoop.Play();
        fireScript.teslaReady = true;
    }

    IEnumerator teslaPowerDown()
    {
        yield return new WaitUntil(() => !teslaCharging);
        teslaLoop.Stop();
        teslaLoop.loop = false;
        fireScript.teslaReady = false;
    }
}

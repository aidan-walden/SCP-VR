using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTeleporter : Teleporter
{
    private bool isUsed = false;
    [SerializeField] GameObject naziOfficer;
    [SerializeField] Transform newOfficerPos;
    protected override void OnTriggerEnter(Collider other)
    {
        if(!isUsed)
        {
            base.OnTriggerEnter(other);
            if (other.tag == "Player")
            {
                isUsed = true;
                NaziOfficer naziScript = naziOfficer.GetComponent<NaziOfficer>();
                other.transform.rotation = destination.transform.rotation;
                naziScript.StopAllCoroutines();
                naziOfficer.transform.position = newOfficerPos.position;
                naziOfficer.transform.rotation = newOfficerPos.rotation;
                naziScript.equipGun();
            }
        }
    }
}

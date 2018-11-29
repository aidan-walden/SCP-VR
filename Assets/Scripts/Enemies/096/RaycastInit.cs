using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AidanTools;

[RequireComponent(typeof(Renderer))]
public class RaycastInit : MonoBehaviour {
    public float viewDistance = Mathf.Infinity;
    public Camera playerCam;
    AidanTools.AidanTools tools;
    private ShyGuyTrigger triggerScript;
    Renderer shyGuyFace;
    void Start () {
        triggerScript = GetComponentInParent<ShyGuyTrigger>();
        tools = new AidanTools.AidanTools();
        shyGuyFace = GetComponent<Renderer>();
	}
    private void Update()
    {
        if(!triggerScript.IsRaging && shyGuyFace.isVisible)
        {
            if (tools.objectIsVisible(playerCam, this.gameObject))
            {
                triggerScript.rageMode();
            }
        }
    }
}

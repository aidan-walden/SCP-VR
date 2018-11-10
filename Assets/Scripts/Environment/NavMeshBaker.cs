using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshBaker : MonoBehaviour {

    NavMeshSurface[] surfaces;
	// Use this for initialization
	void Start () {
        surfaces = GetComponentsInChildren<NavMeshSurface>();
        foreach(NavMeshSurface surface in surfaces)
        {
            surface.BuildNavMesh();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

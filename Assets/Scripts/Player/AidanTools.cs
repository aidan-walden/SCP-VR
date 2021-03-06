﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AidanTools
{

    public class AidanTools
    {
	    public bool calculateIfInBounds(Plane[] planes, GameObject obj)
        {
            Collider objCollider = obj.GetComponent<Collider>();
            if(objCollider == null)
            {
                return false;
            }
            else
            {
                if(GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool objectIsVisible(Camera cam, GameObject obj, float maxDist = Mathf.Infinity, int mask = -1)
        {
            Collider objCollider = obj.GetComponent<Collider>();
            if(objCollider == null)
            {
                return false;
            }
            else
            {
                Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);
                if(calculateIfInBounds(planes, obj))
                {
                    RaycastHit hit;
                    if(Physics.Raycast(cam.transform.position, obj.transform.position - cam.transform.position, out hit, maxDist, mask, QueryTriggerInteraction.Ignore))
                    {
                        Debug.DrawRay(cam.transform.position, obj.transform.position - cam.transform.position, Color.green);
                        if(hit.collider.gameObject.Equals(obj))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }    
    }
}

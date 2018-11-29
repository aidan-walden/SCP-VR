using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AidanTools
{
    public class AidanTools : MonoBehaviour
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
                    Debug.Log("Object is within bounds");
                    return true;
                }
                else
                {
                    Debug.Log("Object is out of bounds");
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
                            Debug.Log("Object is visible because the raycast hit it");
                            return true;
                        }
                        else
                        {
                            Debug.Log("Object is not visible because the raycast hit " + hit.collider.gameObject.name + ", whose root is " + hit.collider.transform.root.name);
                            return false;
                        }
                    }
                    else
                    {
                        Debug.Log("Object is visible because it is in bounds but the raycast didnt hit anything");
                        return true;
                    }
                }
                else
                {
                    Debug.Log("Object is not visible because it is not in bounds");
                    return false;
                }
            }
        }      
    }
}

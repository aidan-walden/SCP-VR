using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(Rigidbody))]
public class Lever : MonoBehaviour {
    [SerializeField] Rigidbody lever;
    public float flipSpeed;
    Hand playerHand;
    public bool isOn = false;
    public enum OnPositions
    {
        Up,
        Down
    };
    public OnPositions onPosition = OnPositions.Up;
    float dialOffset;
    // Use this for initialization
    void Start()
    {
        dialOffset = transform.parent.rotation.eulerAngles.y + 180f;
        if (isOn)
        {
            if (onPosition == OnPositions.Up)
            {
                rotateLever(272f);
            }
            else
            {
                rotateLever(87f);
            }
        }
        else
        {
            if (onPosition == OnPositions.Up)
            {
                rotateLever(87f);
            }
            else
            {
                rotateLever(272f);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha7))
        {
            Debug.Log(dialOffset);
            Debug.Log(transform.eulerAngles.x);
        }
	}

    private void HandAttachedUpdate()
    {
        Vector3 eulerRotation = new Vector3(0, dialOffset, playerHand.transform.eulerAngles.x);
        transform.rotation = Quaternion.Euler(eulerRotation);
    }

    protected virtual void OnAttachedToHand(Hand hand)
    {
        StopAllCoroutines();
        playerHand = hand;
    }

    protected virtual void OnDetachedFromHand(Hand hand)
    {
        playerHand = null;
        chooseRotation();
    }

    protected virtual void OnLeverFlipped()
    {
        if(onPosition == OnPositions.Up)
        {

        }
    }

    private IEnumerator rotateToOrig(Vector3 rotateTo)
    {
        while (transform.rotation != Quaternion.Euler(rotateTo))
        {
            Debug.Log("Rotating...");
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotateTo), Time.deltaTime * flipSpeed);
            yield return null;
        }
        yield return null;
    }

    void rotateLever(float rotation)
    {
        Debug.Log("Starting rotation...");
        StartCoroutine(rotateToOrig(new Vector3(rotation, dialOffset, 0)));
    }

    void chooseRotation()
    {
        float eulerX = transform.eulerAngles.x;
        if(eulerX > 0)
        {
            rotateLever(272f);
        }
        else
        {
            rotateLever(87f);
        }
        OnLeverFlipped();
    }
}

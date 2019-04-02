using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;
[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Throwable))]
[RequireComponent(typeof(Rigidbody))]
public class Lever : MonoBehaviour {
    public UnityEvent OnLeverEnabled, OnLeverDisabled;
    [SerializeField] Rigidbody lever;
    public float flipSpeed;
    Hand playerHand;
    private bool isOn = false;
    public bool IsOn
    {
        get
        {
            return IsOn;
        }
        set
        {
            isOn = value;
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
                OnLeverEnabled.Invoke();
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
                OnLeverDisabled.Invoke();
            }
        }
    }
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
        IsOn = isOn;
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
        Vector3 eulerRotation = new Vector3(playerHand.transform.eulerAngles.x, dialOffset, 0);
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

    private IEnumerator rotateToOrig(Vector3 rotateTo)
    {
        while (transform.rotation != Quaternion.Euler(rotateTo))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(rotateTo), Time.deltaTime * flipSpeed);
            if(Quaternion.Angle(transform.rotation, Quaternion.Euler(rotateTo)) < 1)
            {
                transform.rotation = Quaternion.Euler(rotateTo);
            }
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
        if(eulerX > 90)
        {
            if(onPosition == OnPositions.Up)
            {
                IsOn = true;
            }
            else
            {
                IsOn = false;
            }
        }
        else
        {
            if (onPosition == OnPositions.Down)
            {
                IsOn = true;
            }
            else
            {
                IsOn = false;
            }
        }
    }
}

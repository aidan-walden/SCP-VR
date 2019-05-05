using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR.Extras;

namespace Valve.VR.InteractionSystem
{
    [RequireComponent(typeof(SteamVR_LaserPointer))]
    public class VRUIInput : MonoBehaviour
    {
        private SteamVR_LaserPointer laserPointer;
        private Hand trackedController;
        public SteamVR_Action_Boolean triggerClicked;

        private void OnEnable()
        {
            laserPointer = GetComponent<SteamVR_LaserPointer>();
            laserPointer.PointerIn -= HandlePointerIn;
            laserPointer.PointerIn += HandlePointerIn;
            laserPointer.PointerOut -= HandlePointerOut;
            laserPointer.PointerOut += HandlePointerOut;

            trackedController = GetComponent<Hand>();
            if (trackedController == null)
            {
                trackedController = GetComponentInParent<Hand>();
            }
            //trackedController.TriggerClicked -= HandleTriggerClicked;
            //trackedController.TriggerClicked += HandleTriggerClicked;
            triggerClicked.AddOnStateDownListener(HandleTriggerClicked, trackedController.handType);
        }

        private void HandleTriggerClicked(SteamVR_Action_Boolean actionIn, SteamVR_Input_Sources inputSource)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                ExecuteEvents.Execute(EventSystem.current.currentSelectedGameObject, new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
            }
        }

        private void HandlePointerIn(object sender, PointerEventArgs e)
        {
            var button = e.target.GetComponent<Button>();
            if (button != null)
            {
                button.Select();
                Debug.Log("HandlePointerIn", e.target.gameObject);
            }
            else
            {
                Toggle toggle = e.target.GetComponent<Toggle>();
                if(toggle != null)
                {
                    toggle.Select();
                }
            }

        }

        private void HandlePointerOut(object sender, PointerEventArgs e)
        {

            var button = e.target.GetComponent<Button>();
            if (button != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("HandlePointerOut", e.target.gameObject);
            }
            else
            {
                Toggle toggle = e.target.GetComponent<Toggle>();
                if(toggle != null)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
            }
        }
    }
}
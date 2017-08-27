using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public GameObject pointer;

    private GameObject pointerRight;
    private GameObject pointerLeft;
    private GameObject targetRight;
    private GameObject targetLeft;

    private QtQuickGUI targetGuiRight = null;
    private Vector2 lastHitRight;
    private QtQuickGUI targetGuiLeft = null;
    private Vector2 lastHitLeft;

    public Transform leftHand;
    public Transform rightHand;

	// Use this for initialization
	void Start () {
        pointerRight = Instantiate(pointer, rightHand);
        pointerRight.SetActive(false);
        pointerRight.name = "rightPointer";
        pointerLeft = Instantiate(pointer, leftHand);
        pointerLeft.SetActive(false);
        pointerLeft.name = "leftPointer";
        targetRight = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        targetRight.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        targetRight.GetComponent<Collider>().enabled = false;
        targetLeft = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        targetLeft.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        targetLeft.GetComponent<Collider>().enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        // Recenter View
        if (OVRInput.GetUp(OVRInput.Button.Two))
        {
            OVRManager.display.RecenterPose();
        }

        // Primary
        if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
        {
            pointerRight.SetActive(true);

            pointerRight.transform.localPosition = new Vector3(0f, -0.02f, 0f);

            RaycastHit hit;
            if (Physics.Raycast(rightHand.transform.position, rightHand.transform.forward, out hit, 100))
            {
                //Debug.Log("hit!");
                Vector3 scale = new Vector3(1f, 1f, 1f);
                scale.z = hit.distance;
                pointerRight.transform.localScale = scale;

                targetRight.transform.position = hit.point;
                targetRight.SetActive(true);

                GameObject hitObject = hit.collider.gameObject;
                QtQuickGUI guiObject = hitObject.GetComponent<QtQuickGUI>();
                if (guiObject)
                {
                    if (targetGuiRight && targetGuiRight != guiObject)
                    {
                        // If there was previously another target, send a release event
                        sendTouchReleaseEvent(targetGuiRight, lastHitRight);
                    }
                    
                    lastHitRight = new Vector2(-(hit.textureCoord.x - 1.0f), hit.textureCoord.y);

                    if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
                    {
                        // push
                        targetGuiRight = guiObject;
                        sendTouchStartEvent(guiObject, lastHitRight);
                    }
                    else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
                    {
                        // release
                        sendTouchReleaseEvent(guiObject, lastHitRight);
                        targetGuiRight = null;
                    }
                    else if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                    {
                        // move
                        sendTouchMoveEvent(guiObject, lastHitRight);
                    }
                }
            }

        } else {
            if (targetGuiRight)
            {
                // If we released the hand trigger, send a release event
                sendTouchReleaseEvent(targetGuiRight, lastHitRight);
                targetGuiRight = null;
            }
            pointerRight.SetActive(false);
            targetRight.SetActive(false);
        }

        // Secondary
        if (OVRInput.Get(OVRInput.RawButton.LHandTrigger))
        {
            pointerLeft.SetActive(true);

            pointerLeft.transform.localPosition = new Vector3(0f, -0.02f, 0f);

            RaycastHit hit;
            if (Physics.Raycast(leftHand.transform.position, leftHand.transform.forward, out hit, 100))
            {
                //Debug.Log("hit!");
                Vector3 scale = new Vector3(1f, 1f, 1f);
                scale.z = hit.distance;
                pointerLeft.transform.localScale = scale;

                targetLeft.transform.position = hit.point;
                targetLeft.SetActive(true);

                GameObject hitObject = hit.collider.gameObject;
                QtQuickGUI guiObject = hitObject.GetComponent<QtQuickGUI>();
                if (guiObject)
                {
                    if (targetGuiLeft && targetGuiLeft != guiObject)
                    {
                        // If there was previously another target, send a release event
                        sendTouchReleaseEvent(targetGuiLeft, lastHitLeft);
                    }
                    
                    lastHitLeft = new Vector2(-(hit.textureCoord.x - 1.0f), hit.textureCoord.y);

                    if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
                    {
                        // push
                        targetGuiLeft = guiObject;
                        sendTouchStartEvent(guiObject, lastHitLeft);
                    }
                    else if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
                    {
                        // release
                        sendTouchReleaseEvent(guiObject, lastHitLeft);
                        targetGuiLeft = null;
                    }
                    else if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
                    {
                        // move
                        sendTouchMoveEvent(guiObject, lastHitLeft);
                    }
                }
            }

        }
        else
        {
            if (targetGuiLeft)
            {
                // If we released the hand trigger, send a release event
                sendTouchReleaseEvent(targetGuiLeft, lastHitLeft);
                targetGuiLeft = null;
            }
            pointerLeft.SetActive(false);
            targetLeft.SetActive(false);
        }

    }

    private void sendTouchStartEvent(QtQuickGUI target, Vector2 position)
    {
        //Debug.Log("sendTouchStart");
        target.RegisterTouchStartEvent(position.x, position.y, 0);
    }

    private void sendTouchReleaseEvent(QtQuickGUI target, Vector2 position)
    {
        //Debug.Log("sendTouchRelease");
        target.RegisterTouchEndEvent(position.x, position.y, 0);
    }

    private void sendTouchMoveEvent(QtQuickGUI target, Vector2 position)
    {
        //Debug.Log("sendTouchMove");
        target.RegisterTouchMoveEvent(position.x, position.y, 0);
    }
}
